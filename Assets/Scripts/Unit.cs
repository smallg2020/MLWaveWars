using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Unit : Health
{
  [SerializeField]
  int damage = 1;
  [SerializeField]
  int speed = 1;
  [SerializeField]
  int cost = 1;

  [SerializeField]
  LayerMask canHit;

  bool canMove = true;
  public Base myBase;

  // Update is called once per frame
  void Update()
  {
    canMove = true;
    Ray inFront = new Ray(transform.position + Vector3.up * 0.3f, transform.forward);
    RaycastHit hit;
    Debug.DrawRay(inFront.origin, inFront.direction, Color.red);
    if (Physics.Raycast(inFront, out hit, 10 * Time.deltaTime, canHit))
    {
      canMove = false;
      Rigidbody oRB = hit.rigidbody;
      if (oRB)
      {
        //Debug.Log($"{name} hit {hit.rigidbody.name}");
        if (oRB.TryGetComponent(out Health _health))
        {
          _health.TakeDamage(damage);
        }
        if (oRB.TryGetComponent(out Unit _unit))
        {
          //TakeDamage(_unit.damage);
        }
        else
        {
          Destroy(gameObject);
        }
      }
    }
    if (canMove)
    {
      transform.position += transform.forward * speed * Time.deltaTime;
    }
  }

  public int GetCost()
  {
    return cost;
  }  

}
