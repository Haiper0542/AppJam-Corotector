using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDrone : DroneAIBase
{

    [Header("[정보]")]
    public bool isMoving = true;
    public bool isReloading = false;

    public float attackstartDist = 3;

    [Header("[총알]")]
    public float bulletSpeed = 10;
    public int bulletCount = 5;
    public AudioClip shotClip;
    public AudioClip shieldClip;

    public float reloadTerm = 3;

    [Header("[레이저]")]
    public GameObject demoLaser;
    public float laserOffDelay = 0.5f;

    [Header("[공격관련 정보]")]
    public float attackDelay = 5f;
    public Transform muzzle; //총구
    public GameObject spark;

    public float attackTime = 0;
    private float reloadTime = 0;

    public override void Start()
    {
        base.Start();
        reloadTime = reloadTerm;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            Death();

        switch (nowState)
        {
            case State.Death:
                GetComponent<Rigidbody>().AddForce(transform.forward * speed * 0.5f * Time.deltaTime);
                deathDelay -= Time.deltaTime;
                if (deathDelay < 0)
                {
                    Destroy(gameObject);
                }
                break;
        }

        if (isDead)
        {
            demoLaser.SetActive(false);
            return;
        }

        if (isReloading)
        {
            if (reloadTime > 0)
                reloadTime -= Time.deltaTime;
            else
            {
                bulletCount = 5;
                isMoving = false;
                isReloading = false;
                demoLaser.SetActive(false);
            }
        }


        if (Vector3.SqrMagnitude(target.position - transform.position) > stoppingDist * stoppingDist)
        {
            isMoving = true;
            Chase();
        }
        if (Vector3.SqrMagnitude(target.position - transform.position) < attackstartDist * attackstartDist)
        {
            Attack();
        }

        RaycastHit[] allhit = Physics.RaycastAll(muzzle.position, muzzle.rotation * Vector3.forward, enemyLayer);
        foreach (RaycastHit h in allhit)
        {
            if (h.transform.CompareTag("Shield"))
            {
                float dist = h.distance * 0.98f;
                demoLaser.transform.localPosition = new Vector3(0, 0, -dist * 0.5f);
                demoLaser.transform.localScale =
                    new Vector3(demoLaser.transform.localScale.x, demoLaser.transform.localScale.y, dist);
            }
        }
        base.Update();
    }

    public override void AttackStart()
    {
        if (isDead)
            return;
        demoLaser.SetActive(true);
        base.AttackStart();
    }

    public override void Chase()
    {
        if (isDead || !isMoving)
            return;

        demoLaser.SetActive(false);
        base.Chase();
    }

    public override void Attack()
    {
        if (isDead || isReloading)
            return;

        isMoving = false;

        attackTime += Time.deltaTime;

        if(attackTime < 1)
            ChaseRot();

        demoLaser.transform.localPosition = new Vector3(0, 0, -10);
        demoLaser.transform.localScale =
            new Vector3(demoLaser.transform.localScale.x, demoLaser.transform.localScale.y, 20);

        RaycastHit hit;

        if (attackTime > attackDelay)
        {
            attackTime = 0;
            bulletCount--;
            try
            {
                audioSource.PlayOneShot(shotClip);
            }
            catch {  }
            
            if (Physics.Raycast(muzzle.position, muzzle.rotation * Vector3.forward, out hit, enemyLayer))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.CompareTag("Player"))
                    hit.transform.GetComponent<PlayerCtrl>().TakeDamage(damage);
                if (hit.transform.CompareTag("Core"))
                    hit.transform.GetComponent<CoreCtrl>().TakeDamage(damage);

                if (hit.transform.CompareTag("Shield"))
                {
                    audioSource.PlayOneShot(shieldClip);
                    Instantiate(spark, hit.point, Quaternion.Euler(hit.normal));
                }
            }

            if (bulletCount <= 0)
            {
                reloadTime = reloadTerm;
                isMoving = true;
                isReloading = true;
                demoLaser.SetActive(true);
            }
        }
    }

    public override void Death()
    {
        base.Death();
        demoLaser.SetActive(false);
    }
}
