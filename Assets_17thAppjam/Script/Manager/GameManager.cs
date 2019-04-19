using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerClass { MainMenu, Guardian, Surprise }

public class GameManager : MonoBehaviour
{

    #region Variables

    public static GameManager instance;


    public Transform VRRoomTr;
    public bool isGameOver = false;
    public PlayerClass playerClass
    {
        get
        {
                return DataManager.instance.playerClass;
        }
    }
    public PlayerCtrl playerctrl;
    public Text StartGameText;

    [Header("Generator")]
    [SerializeField] private float GeneratingTime = 3f;

    [Space, SerializeField] private GameObject NormalDrone;
    [SerializeField] private GameObject SuicideDrone, SniperDrone;
    [SerializeField] private Transform PlayerTr;
    [SerializeField] private Transform CoreTr;
    private int SpecialSpawnCount = 3;
    [Space, SerializeField] private Transform[] generatePoint;


    #endregion

    #region LifeCycle Methods

    private void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(gameObject);
        
    }

    #endregion

    #region EnemyGenerate
    

    private IEnumerator StartGame()
    {
        for(int i=3;i>0;i--)
        {
            StartGameText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        StartGameText.text = "Start!";
        yield return new WaitForSeconds(1f);
        StartGameText.gameObject.SetActive(false);
        StartCoroutine(GenerateSystem());
        
        yield return null;
    }

    private IEnumerator GenerateSystem()
    {
        int spawncount = 0;

        while (!GameManager.instance.isGameOver)
        {
            spawncount++;

            Transform point = generatePoint[Random.Range(0, generatePoint.Length)];

            if (spawncount > SpecialSpawnCount)
            {
                if (Random.Range(0, 10) < 3)
                {
                    spawncount = 0;
                    GameObject drone = Instantiate(SuicideDrone, point);
                    drone.GetComponent<DroneAIBase>().target = PlayerTr;
                    drone.GetComponent<DroneAIBase>().targetList = new Transform[2];
                    drone.GetComponent<DroneAIBase>().targetList[0] = PlayerTr;
                    drone.GetComponent<DroneAIBase>().targetList[1] = CoreTr;
                }
                else
                {
                    spawncount = 0;
                    GameObject drone = Instantiate(SniperDrone, point);
                    drone.GetComponent<DroneAIBase>().target = PlayerTr;
                    drone.GetComponent<DroneAIBase>().targetList = new Transform[2];
                    drone.GetComponent<DroneAIBase>().targetList[0] = PlayerTr;
                    drone.GetComponent<DroneAIBase>().targetList[1] = CoreTr;
                }
            }
            else
            {
                GameObject drone = Instantiate(NormalDrone, point);
                drone.GetComponent<DroneAIBase>().target = PlayerTr;
                drone.GetComponent<DroneAIBase>().targetList = new Transform[2];
                drone.GetComponent<DroneAIBase>().targetList[0] = PlayerTr;
                drone.GetComponent<DroneAIBase>().targetList[1] = CoreTr;
            }
            yield return new WaitForSeconds(GeneratingTime);
        }
    }

    public void GameReady()
    {
        if (playerClass != PlayerClass.MainMenu)
        {
            switch (playerClass)
            {
                case PlayerClass.Guardian:
                    playerctrl.GuardianWeaponOn();
                    break;
                case PlayerClass.Surprise:
                    playerctrl.SurpriseWeaponOn();
                    break;
            }
        }
        StartCoroutine(StartGame());
    }


    #endregion

}
