using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _statisticsCamera;
    [SerializeField] private GameObject _mainManu;
    [SerializeField] private GameObject _statisticsManu;

    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void BackMainMenu()
    {
        _statisticsCamera.SetActive(false);
        _statisticsManu.SetActive(false);
        _mainCamera.SetActive(true);
        _mainManu.SetActive(true);
    }
    public void Statistics()
    {
        _mainCamera.SetActive(false);
        _mainManu.SetActive(false);
        _statisticsCamera.SetActive(true);
        _statisticsManu.SetActive(true);
    }
}
