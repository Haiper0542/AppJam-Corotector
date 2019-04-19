using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    #region Variables

    [SerializeField] private float GeneratingTime = 3f;
    
    [Space, SerializeField] private GameObject NormalDrone;
    [SerializeField] private Transform PlayerTr;


    private int SpecialSpawnCount = 3;

    #endregion

    #region Life Cycle Methods

    public void StartGame()
    {
        StartCoroutine(GenerateSystem());
    }

    #endregion

    #region Coroutine

    private IEnumerator GenerateSystem()
    {
        int spawncount = 0;

        while(!GameManager.instance.isGameOver)
        {
            spawncount++;

            if (spawncount > SpecialSpawnCount)
            {
                if(Random.Range(0,10) < 3)
                {
                    spawncount = 0;
                    GameObject drone = Instantiate(NormalDrone, this.transform);
                    drone.GetComponent<NormalDrone>().target = PlayerTr;
                    drone.GetComponent<NormalDrone>().targetList = new Transform[1];
                    drone.GetComponent<NormalDrone>().targetList[0] = PlayerTr;
                }
                else
                {

                }

            } else
            {
                GameObject drone = Instantiate(NormalDrone, this.transform);
                drone.GetComponent<NormalDrone>().target = PlayerTr;
                drone.GetComponent<NormalDrone>().targetList = new Transform[1];
                drone.GetComponent<NormalDrone>().targetList[0] = PlayerTr;
            }
            yield return new WaitForSeconds(GeneratingTime);
        }
    }

    #endregion
}
