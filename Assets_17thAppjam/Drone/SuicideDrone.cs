using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideDrone : DroneAIBase { //자폭드론

    [Header("[폭발 범위]")]
    public float radius = 5;

    public GameObject explosion;

    public override void AttackStart()
    {
        if (isDead)
            return;
        base.AttackStart();
    }

    public override void Chase()
    {
        if (isDead)
            return;
        base.Chase();
    }

    public override void Attack()
    {
        if (isDead)
            return;
        Instantiate(explosion, transform.position, Quaternion.identity);
        foreach (Collider col in Physics.OverlapSphere(transform.position, radius, enemyLayer))
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponent<PlayerCtrl>().TakeDamage(damage);
            }
            if (col.CompareTag("Core"))
            {
                col.GetComponent<CoreCtrl>().TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
