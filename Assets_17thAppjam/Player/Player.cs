using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public enum ClassState { Guardian, Surprise }
    public ClassState nowClass;

    [Header("[Guardian]")]
    public int gHealth;
    public int gAttack;
    public GunCtrl gGun;
    public GameObject shield;

    [Header("[Surprise]")]
    public int sHealth;
    public int sAttack;
    public GunCtrl sGun;

    [Header("[NowInfo]")]
    public int health;
    public int attack;
    public GunCtrl nowGun;
    public GameObject nowShield;

    private void Start()
    {
        SetClass(ClassState.Guardian);
    }

    public void SetClass(ClassState _class) //클래스 장비 부분
    {
        nowClass = _class;
        switch (_class)
        {
            case ClassState.Guardian:
                health = gHealth;
                attack = gAttack;
                nowGun = Instantiate(gGun, transform);
                nowShield = Instantiate(shield, transform);
                break;
            case ClassState.Surprise:
                health = sHealth;
                attack = sAttack;
                nowGun = Instantiate(sGun, transform);
                break;
        }
    }

    void Update()
    {
        if(nowGun != null)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                nowGun.Shot();
            }
            if (Input.GetKey(KeyCode.R))
            {
                nowGun.isReloading = true;
            }
            else
            {
                nowGun.isReloading = false;
            }
        }
    }

    public void SpecialAbillity()
    {
        switch (nowClass)
        {
            case ClassState.Guardian:

                break;
            case ClassState.Surprise:

                break;
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            health = 0;
            Death();
        }
    }

    public void Death()
    {
        Debug.Log("Death");
    }

}