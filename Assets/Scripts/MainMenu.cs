using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private const string JANITOR_SCENE = "Game - Terry";
    private const string MISSTAKE_SCENE = "Game - Take";
    private const string DAD_SCENE = "Game - Father";

    private int _sceneId;
    
    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void SelectPassenger(int id)
    {

        _sceneId = id;
    }

    public void StartGame()
    {
        switch (_sceneId)
        {
            case 1:
                SceneManager.LoadScene(MISSTAKE_SCENE);
                return;
            case 2:
                SceneManager.LoadScene(DAD_SCENE);
                return;
            default:
                SceneManager.LoadScene(JANITOR_SCENE);
                return;
        }
    }
}