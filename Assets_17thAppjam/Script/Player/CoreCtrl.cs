using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreCtrl : MonoBehaviour
{
    #region Variables

    [SerializeField]  private float health = 100f;

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            health = 0;
            Death();
        }
    }

    private void Death()
    {
        GameManager.instance.isGameOver = true;
        SceneManager.LoadScene(2);
    }

    #endregion
}
