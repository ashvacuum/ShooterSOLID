using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamage
{
    [SerializeField] private int maxHealth;

    private int currentHealth;

    

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
        if (currentHealth > 0)
        {
            currentHealth += amount;
        }
        else
        {
            //death script
        }    
    }
}
