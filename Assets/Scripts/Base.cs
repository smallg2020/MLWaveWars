using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Health
{
  [SerializeField]
  int money = 0;
  [SerializeField]
  int moneyPerSec = 1;

  [SerializeField]
  Unit[] unitsUnlocked;

  [SerializeField]
  Transform spawnT;
  [SerializeField]
  AgentAI myAI, otherAI;
  [SerializeField]
  Base otherBase;
  [SerializeField]
  int currentMoney;

  private void Start()
  {
    ResetBase();
    StartCoroutine(AccumulateMoneyCR());
  }

  public void AddMoney(int _money)
  {
    currentMoney += _money;
  }

  public int GetCurrentMoney()
  {
    return currentMoney;
  }

  public Unit[] GetUnits()
  {
    return unitsUnlocked;
  }

  public void SpawnUnit(Unit _unit)
  {
    if (!_unit)
    {
      return;
    }
    int cost = _unit.GetCost();
    if (currentMoney < cost)
    {
      return;
    }
    AddMoney(-cost);
    Unit newUnit = Instantiate(_unit, spawnT);
    newUnit.gameObject.SetActive(true);
    newUnit.transform.SetPositionAndRotation(spawnT.position, spawnT.rotation);
  }

  IEnumerator AccumulateMoneyCR()
  {
    do
    {
      yield return new WaitForSeconds(1);
      AddMoney(moneyPerSec);
    } while (true);
  }
  public void LostWar()
  {
    myAI.AddReward(-1);
    otherAI.AddReward(1);
    myAI.EndEpisode();
    otherAI.EndEpisode();
    otherBase.ResetBase();
    ResetBase();    
  }

  public void ResetBase()
  {    
    for (int i = 0; i < spawnT.childCount; i++)
    {
      Destroy(spawnT.GetChild(i).gameObject);
    }
    ResetHealth();
    currentMoney = money;
  }

}
