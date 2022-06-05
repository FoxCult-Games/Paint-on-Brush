using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject ChoosePlayers;

    [SerializeField] private GameObject[] playersPanels;

    void Start()
    {
        GoToMainMenu();
    }

    public void GoToMainMenu(){
        ChoosePlayers.SetActive(false);

        MainMenu.SetActive(true);
    }

    public void GoToChoosePlayers(){
        MainMenu.SetActive(false);

        ChoosePlayers.SetActive(true);
    }

    public void Exit(){
        Application.Quit();
    }
}
