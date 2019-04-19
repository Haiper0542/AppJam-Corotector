using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject UI_Main;
    [SerializeField] private GameObject UI_Select;
    [SerializeField] private GameObject UI_Gurdian;
    [SerializeField] private GameObject UI_Surprise;

    private void Awake()
    {
        UI_Select.SetActive(false);
        UI_Main.SetActive(true);
        UI_Gurdian.SetActive(false);
        UI_Surprise.SetActive(false);
    }

    public void Panel_SelectOn()
    {
        UI_Gurdian.SetActive(false);
        UI_Surprise.SetActive(false);
        UI_Main.SetActive(false);
        UI_Select.SetActive(true);
    }

    public void Panel_MainMenuOn()
    {
        UI_Main.SetActive(true);
        UI_Select.SetActive(false);
    }

    public void Panel_Guardian()
    {
        UI_Select.SetActive(false);
        UI_Gurdian.SetActive(true);
    }

    public void Panel_Surprise()
    {
        UI_Select.SetActive(false);
        UI_Surprise.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
