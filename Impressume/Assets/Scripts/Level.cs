﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Level : MonoBehaviour
{
    [SerializeField] float delayInSeconds = 1.5f;

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadBossFight()
    {
        FindObjectOfType<GameSession>().GetScore();
        SceneManager.LoadScene(3);
    }

    public void LoadResume()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadGame()
    {
        FindObjectOfType<GameSession>().ResetGame();
        SceneManager.LoadScene(1);
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("Game Over");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
