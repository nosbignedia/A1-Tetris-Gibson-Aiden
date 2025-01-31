using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using System.Data;
public class TetrisManager : MonoBehaviour
{

    private TetrisGrid grid;
    private TetrisSpawner tetrisSpawner;
    public GameObject gameOverText;
    [SerializeField]
    TextMeshProUGUI scoreText;
    public int score;
    public float timeRemaining = 90;
    public bool timeIsRunning = true;
    public TMP_Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        timeIsRunning = true;
        grid = FindObjectOfType<TetrisGrid>();
        tetrisSpawner = FindObjectOfType<TetrisSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGameOver();
        scoreText.text = "Score: " + score;
        if (timeIsRunning)
        {
            if(timeRemaining >= 0) 
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            Debug.Log("GAMEOVER");
        }


    }

    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt (timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format ("{0:00} : {1:00}", minutes , seconds);
    }

    public void CalculateScore(int linesCleared)
    {
        switch (linesCleared) 
        {

            case 1:
                score += 100;
                timeRemaining *= 1.1f;
                break;
            case 2:
                score += 300;
                timeRemaining *= 1.2f;
                break;
            case 3:
                score += 600;
                timeRemaining *= 1.3f;
                break;
            case 4:
                score += 900;
                timeRemaining *= 1.4f;
                break;
            default:
                score += 100;
                timeRemaining *= 1.5f;
                break;

        }

    }


    public void CheckGameOver()
    {
        for(int i = 0; i < grid.width; i++)
        {
            if (grid.IsCellOccupied(new Vector2Int(i, grid.height - 1)))
            {
                Debug.Log("Game Over!");
                gameOverText.SetActive(true);
                tetrisSpawner.enabled = (false);
                Invoke("Reloadscene", 5);
              
            }
        }
        
    }

    public void RelaodScene()
    {
        SceneManager.LoadScene("Tetris");
    }

}
