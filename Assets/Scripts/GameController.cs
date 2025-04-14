using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private StopWatch stopWatch;
    public float currentScore;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void loadLeaderboard()
    {
        SceneManager.LoadScene("LoadLeaderboard");
    }

    private void Start()
    {
        stopWatch = FindObjectOfType<StopWatch>(); // Find the StopWatch object in the scene
        if (stopWatch != null)
        {
            stopWatch.StartStopwatch(); // Start the stopwatch
        }
        else
        {
            Debug.LogError("StopWatch is not initialized in Start!");
        }
    }

    public void StartGame()
    {
        Debug.Log("StartGame");
        SceneManager.LoadScene("cave_level");
    }

    public void EndGame()
{
    if (stopWatch == null)
    {
        stopWatch = FindObjectOfType<StopWatch>(); // Try finding StopWatch again
    }

    if (stopWatch != null)
    {
        StopStopwatch();
        PlayerPrefs.SetFloat("ElapsedTime", stopWatch.GetElapsedTime());
    }
    else
    {
        Debug.LogError("StopWatch is not initialized in EndGame!");
    }

    SceneManager.LoadScene("SaveScore");
}

    public float GetElapsedTime()
    {
        return currentScore = stopWatch.GetElapsedTime();
    }

    private void StopStopwatch()
    {
        if (stopWatch != null)
        {
            stopWatch.StopStopwatch(); // Stop the stopwatch
        }
    }



}
