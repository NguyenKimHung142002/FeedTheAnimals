using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.IO;
public class PlayerCanvasUiController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI highestScoreTxt;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    private bool isPause = false;
    private int score = 0;
    private int highestScore;
    private bool isGameOver = false;
    private void Start()
    {
        Time.timeScale = 1;
        isGameOver = false;
        ResumeGame();
        LoadGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameOver == false)
        {

            if (isPause == false)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }

        }
    }
    public void PauseGame()
    {
        isPause = true;
        pauseMenu.SetActive(isPause);
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        StartCoroutine(SetGameOverMenu());
    }
    IEnumerator SetGameOverMenu ()
    {
        yield return new WaitForSeconds(1.5f); 
        isGameOver = true;
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void SaveGame()
    {
        SaveData saveData = new SaveData();
        saveData.score = score;
        if (score >= highestScore)
        {
            saveData.highestScore = score;
        }
        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(Application.dataPath + "/saveDataJson.json", json);
    }
    public void LoadGame()
    {
        string json = File.ReadAllText(Application.dataPath + "/saveDataJson.json");
        if (json != null)
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            highestScore = saveData.highestScore;
            UpdateHighestScore();
        }
    }
    public void ResumeGame()
    {
        isPause = false;
        pauseMenu.SetActive(isPause);
        Time.timeScale = 1;
    }
    public void RestartGame()
    {
        SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restart");
        
    }
    public void UpdateHealth()
    {
        healthBar.value--;
        if (healthBar.value <= 0)
        {
            Debug.Log("Lost");
        }
    }

    public void UpdateScore(int addScore)
    {
        score += addScore;
        scoreTxt.text = "Score: " + score.ToString("0000");
        if (score > highestScore)
        {
            highestScore = score;
            UpdateHighestScore();
        }

    }
    public void UpdateHighestScore()
    {
        highestScoreTxt.text = "Higest Score: " + highestScore.ToString("0000");
    }

    public void Pause()
    {
        isPause = true;

    }
}
