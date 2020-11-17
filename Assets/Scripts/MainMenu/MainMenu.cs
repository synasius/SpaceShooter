﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSinglePlayer()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadCoOpMode()
    {
        SceneManager.LoadScene(2);
    }
}
