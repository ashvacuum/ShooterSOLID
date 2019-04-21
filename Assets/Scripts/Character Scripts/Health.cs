using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IDamage
{
    [SerializeField] private int maxHealth;

    private int currentHealth;

    public float GetHealthPercentage
    {
        get
        {
            float current = currentHealth;
            float max = maxHealth;
            return current/max;
        }
    }
    

    private void Start()
    {
        currentHealth = maxHealth;

    }

    /// <summary>
    /// Used to heal or deal damage
    /// </summary>
    /// <param name="amount">if negative will deal damage, if positive will heal</param>
    public void ModifyHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }    
    }
}
