using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region Variables

    public static DataManager instance;

    public PlayerClass playerClass;

    private bool isStart = false;
    #endregion

    #region LifeCycle Methods

    private void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    #endregion

    #region Other Methods

    public void SelectClass(int id)
    {
        switch (id)
        {
            case 1:
                playerClass = PlayerClass.Guardian;
                break;
            case 2:
                playerClass = PlayerClass.Surprise;
                break;
        }
    }

    private void Update()
    {
        if(GameManager.instance!=null&&!isStart)
        {
            GameManager.instance.GameReady();
            isStart = true;
        }

    }

    #endregion

}
