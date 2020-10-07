using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    public GameObject exitMenu;

    private void Start()
    {
        CloseExitMenu();
    }

    public void CloseExitMenu()
    {
        exitMenu.SetActive(false);
    }

    public void OpenExitMenu()
    {
        exitMenu.SetActive(true);
    }
}
