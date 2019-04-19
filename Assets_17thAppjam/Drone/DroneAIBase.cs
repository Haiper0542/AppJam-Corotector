using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DroneAIBase : MonoBehaviour {

    public enum State { Chase, Attack, Death }
    public State nowState;

    public Transform[] targetList;

    public Vector3 via;
    public Vector3 upVia;
    public int viaIndex = 0;
    public Transform target;

    [Header("[정보]")]
    public int health = 50;

    public int speed = 5;
    public int rotSpeed = 5;

    public int damage = 5;

    public float stoppingDist = 5;
    private float attackingDist;

    [Header("[죽을때]")]
    public float deathDelay = 3;
    public bool isDead = false;

    [Header("[사운드 관리]")]
    public AudioSource audioSource;
    public AudioClip deathClip;

    public LayerMask enemyLayer;

    public virtual void Start()
    {
        via = (transform.position - target.position) * 0.5f + (Vector3)(Random.insideUnitCircle * 1.5f) + Vector3.up * 0.7f;
        upVia = target.position + Vector3.up * Random.Range(3.5f, 6f) + (Vector3)(Random.insideUnitCircle * 5);
        audioSource = GetComponent<AudioSource>();
        attackingDist = stoppingDist * 1.3f;

        target = targetList[Random.Range(0, targetList.Length)];
    }

    public virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
            Death();

        switch (nowState)
        {
            case State.Chase:   //추격
                Chase();
                StateUpdate();
                break;
            case State.Attack:  //공격
                Attack();
                StateUpdate();
                break;
            case State.Death:
                GetComponent<Rigidbody>().AddForce(transform.forward * speed * 0.5f * Time.deltaTime);
                deathDelay -= Time.deltaTime;
                if(deathDelay < 0)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void StateUpdate()
    {
        switch (nowState)
        {
            case State.Chase:   //추격상태일때 멈추는 거리안으로 들어오면
                if (Vector3.SqrMagnitude(transform.position - target.position) < stoppingDist * stoppingDist)
                {
                    AttackStart();
                }
                break;
            case State.Attack:  //공격상태일때 공격 가능 범위 밖으로 나가면
                if (Vector3.SqrMagnitude(transform.position - target.position) > attackingDist * attackingDist)
                {
                    ChaseStart();
                }
                break;
        }
    }

    public virtual void Chase()
    {
        if (viaIndex == 2)
        {
            Debug.DrawLine(transform.position, target.position);

            ChaseRot();
            transform.position += Quaternion.LookRotation(target.position - transform.position) * Vector3.forward * speed * Time.deltaTime;
        }
        else if (viaIndex == 1)
        {
            Debug.DrawLine(transform.position, upVia);

            ChaseViaRot2();
            transform.position += Quaternion.LookRotation(upVia - transform.position) * Vector3.forward * speed * Time.deltaTime;
            if (Vector3.SqrMagnitude(transform.position - upVia) < 4)
            {
                viaIndex = 2;
            }
        }
        else
        {
            Debug.DrawLine(transform.position, via);

            ChaseViaRot();
            transform.position += Quaternion.LookRotation(via - transform.position) * Vector3.forward * speed * Time.deltaTime;
            if(Vector3.SqrMagnitude(transform.position - via) < 4)
            {
                viaIndex = 1;
            }
        }
    }

    public void ChaseRot()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(transform.position - target.position),
            Time.deltaTime * rotSpeed
            );
    }

    public void ChaseViaRot()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(transform.position - via),
            Time.deltaTime * rotSpeed
            );
    }
    public void ChaseViaRot2()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(transform.position - upVia),
            Time.deltaTime * rotSpeed
            );
    }

    public virtual void AttackStart()
    {
        nowState = State.Attack;
    }

    public virtual void ChaseStart()
    {
        nowState = State.Chase;
    }

    public abstract void Attack();

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        isDead = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddTorque(
            Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), ForceMode.Impulse);
        health = 0;
        nowState = State.Death;
        audioSource.PlayOneShot(deathClip);
    }
}
