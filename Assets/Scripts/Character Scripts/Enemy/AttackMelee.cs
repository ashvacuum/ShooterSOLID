using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMelee : MonoBehaviour
{
    [SerializeField]private float effectiveRange;
    [SerializeField] Transform attackDirection;
    [SerializeField] private float attackRate;
    float lastTimeAttacked;

    [SerializeField] int damage;
    Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        lastTimeAttacked = 0;
    }

    private void Update()
    {
        if(Time.time - lastTimeAttacked > attackRate)
        {
            Attack();
        }
    }

    void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up, attackDirection.position, effectiveRange);
        if(hit.collider != null && hit.collider.GetComponent<IDamage>() != null)
        {
            Debug.Log("I hit a " + hit.collider.tag);
            hit.collider.GetComponent<IDamage>().ModifyHealth(-damage);
        }
        lastTimeAttacked = Time.time;
    }
}
