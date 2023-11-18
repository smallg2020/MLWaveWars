using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
  [SerializeField]
  int health = 1;
  [SerializeField]
  Image healthBar;
  [SerializeField]
  int currentHealth;

  private void Start()
  {
    ResetHealth();
  }

  public void ResetHealth()
  {
    currentHealth = health;
    healthBar.fillAmount = currentHealth * 1.0f / health;
  }

  public void TakeDamage(int _damage)
  {
    if (currentHealth < 1)
    {
      return;
    }
    currentHealth -= _damage;
    currentHealth = Mathf.Max(currentHealth, 0);
    healthBar.fillAmount = currentHealth * 1.0f / health;
    if (currentHealth < 1)
    {
      if (TryGetComponent(out Base _base))
      {
        _base.LostWar();
      }
      else
      {
        Destroy(gameObject, 0.01f);
      }
    }
  }

  public int GetHealth()
  {
    return currentHealth;
  }

  public int GetMaxHealth()
  {
    return health;
  }
}
