using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField] private RawImage pantallaCarga;
    [SerializeField] private GameObject pantallaPrincipal;
    
    public void NewGame() 
    {
        SceneManager.LoadScene("MockUpInv");
    }

    public void LoadGame() 
    {
        pantallaPrincipal.SetActive(false);
        pantallaCarga.gameObject.SetActive(true);
    }

    public void CloseGame() 
    {
        Application.Quit();
    }
}
