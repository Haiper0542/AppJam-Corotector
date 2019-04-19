using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    #region Variables

    [SerializeField] private float TeleportCooldown = 5f;

    public bool isTeleportUsed = false;

    private float tcooldown = 0;
    [SerializeField] private float health = 100f;

    [SerializeField] private GameObject WeaponGuardianL;
    [SerializeField] private GameObject WeaponGuardianR;
    [SerializeField] private GameObject WeaponSurpriseL;
    [SerializeField] private GameObject WeaponSurpriseR;

    #endregion

    #region LifeCycle Method

    private void Update()
    {
        
        if(isTeleportUsed)
        {
            ResetTeleport();
        }
    }

    #endregion

    #region Other Methods

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            health = 0;
            Death();
        }
    }

    public void GuardianWeaponOn()
    {
        WeaponGuardianL.SetActive(true);
        WeaponGuardianR.SetActive(true);
        WeaponSurpriseL.SetActive(false);
        WeaponSurpriseR.SetActive(false);
    }

    public void SurpriseWeaponOn()
    {
        WeaponGuardianL.SetActive(false);
        WeaponGuardianR.SetActive(false);
        WeaponSurpriseL.SetActive(true);
        WeaponSurpriseR.SetActive(true);
    }

    private void ResetTeleport()
    {
        tcooldown += Time.deltaTime;
        if (tcooldown >= TeleportCooldown)
        {
            tcooldown = 0;
            isTeleportUsed = false;
        }
    }

    private void Death()
    {
        GameManager.instance.isGameOver = true;
        SceneManager.LoadScene(2);
    }

    #endregion
}
