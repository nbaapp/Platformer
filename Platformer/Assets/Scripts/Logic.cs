using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Logic : MonoBehaviour
{
    public GameObject target;
    public GameObject player;
    public Text scoreText;
    public GameObject gameOverScreen;
    public float xMin = -46;
    public float xMax = 46;
    public float yMin = -22;
    public float yMax = 25;
    private Vector3 spawnPoint;
    private int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        NewTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void NewTarget()
    {
        spawnPoint = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), -1);
        Instantiate(target, spawnPoint, Quaternion.identity);
    }

    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();
        if (score % 10 == 0)
        {
            NewTarget();
        }
    }
}
