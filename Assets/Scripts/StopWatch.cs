using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StopWatch : MonoBehaviour
{
    public TMP_Text timerText;  // Reference to the UI Text component
    private float elapsedTime;
    private bool isRunning;

    private void Awake(){
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid memory leaks
    }

     void Start()
    {
        ResetStopwatch();
        StartStopwatch();
        AssignTimerText();
    }

    void Update()
    {
        if (isRunning && Time.timeScale > 0f)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    public void StartStopwatch()
    {
        isRunning = true;
    }

    public void StopStopwatch()
    {
        isRunning = false;
    }

    public void ResetStopwatch()
    {
        elapsedTime = 0f;
        UpdateTimerDisplay();
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 100) % 100);

        if (timerText != null)
        {
            timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
        }
        else
        {
            Debug.LogWarning("TimerText is not assigned in the Inspector!");
        }
    }

    private void AssignTimerText()
    {
        GameObject timerTextObject = GameObject.FindGameObjectWithTag("TimerText");
        if (timerTextObject != null)
        {
            timerText = timerTextObject.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogWarning("TimerText GameObject with tag 'TimerText' is not found in the scene!");
            timerText = null; // Clear reference if not found
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignTimerText(); // Reassign the timerText reference after the scene is loaded
    }
}
