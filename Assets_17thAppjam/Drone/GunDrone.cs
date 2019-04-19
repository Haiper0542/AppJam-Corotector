using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDrone : DroneAIBase { //총쏘는 드론

    [Header("[레이저]")]
    public GameObject demoLaser;
    public float bulletSpeed = 10;
    public float laserOffDelay = 0.5f;
    private float laserTime= 0.1f;
    bool laserBool = false;
    public AudioClip readyClip;
    public AudioClip shotClip;
    public AudioClip shieldClip;
    bool isReadyClip = true;

    [Header("[공격관련 정보]")]
    public float attackDelay = 5f;
    public Transform muzzle; //총구
    public GameObject spark;

    private float attackTime = 0;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (laserTime > 0)
        {
            if (laserBool)
                laserTime -= Time.deltaTime;
        }
        else
        {
            laserBool = false;
            demoLaser.SetActive(true);
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
    }

    public override void AttackStart()
    {
        if (isDead)
        {
            demoLaser.SetActive(false);
            return;
        }

        base.AttackStart();

        attackTime = 0;

        demoLaser.SetActive(true);
    }

    public override void Attack()
    {
        if (isDead)
            return;

        attackTime += Time.deltaTime;
        if (attackTime > attackDelay - 0.8f && isReadyClip)
        {
            isReadyClip = false;
            audioSource.PlayOneShot(readyClip);
        }

        if(attackTime < 1)
            ChaseRot();

        demoLaser.transform.localPosition = new Vector3(0, 0, -10);
        demoLaser.transform.localScale =
            new Vector3(demoLaser.transform.localScale.x, demoLaser.transform.localScale.y, 20);

        RaycastHit hit;

        if (attackTime > attackDelay)
        {
            attackTime = -0.3f;

            bool flag = false;
            
            if (Physics.Raycast(muzzle.position, muzzle.rotation * Vector3.forward, out hit, enemyLayer))
            {
                if (hit.transform.CompareTag("Player"))
                    hit.transform.GetComponent<PlayerCtrl>().TakeDamage(damage);
                if (hit.transform.CompareTag("Core"))
                    hit.transform.GetComponent<CoreCtrl>().TakeDamage(damage);

                if (hit.transform.CompareTag("Shield"))
                {
                    flag = true;
                    Instantiate(spark, hit.point, Quaternion.Euler(hit.normal));
                }
                isReadyClip = true;
            }

            if (flag)
                audioSource.PlayOneShot(shieldClip);
            else
                audioSource.PlayOneShot(shotClip);

            demoLaser.SetActive(false);
            laserTime = laserOffDelay;
            laserBool = true;
        }
    }

    public override void Death()
    {
        base.Death();
        demoLaser.SetActive(false);
    }
}
