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
