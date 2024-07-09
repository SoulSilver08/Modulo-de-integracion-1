using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DEVMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    public void CerrarMenu() 
    {
        gameObject.SetActive(false);
    }

    public void MainScreen() 
    {
        SceneManager.LoadScene(0);
    }
}
