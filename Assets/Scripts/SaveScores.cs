using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveScores : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text timerText;
    public StopWatch stopwatch;
    private GameController gameController;
    float timer;

    void Start()
    {
    
        timer = PlayerPrefs.GetFloat("ElapsedTime", 0);

        string hours = Mathf.Floor(timer / 3600).ToString("00");
        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");

        timerText.text = $"{hours}:{minutes}:{seconds}";

    }
    
    public void SetScores()
    {

        List<float> scores = new List<float>();
        List<string> names = new List<string>();

        scores.Add(PlayerPrefs.GetFloat("Score_1"));
        scores.Add(PlayerPrefs.GetFloat("Score_2"));
        scores.Add(PlayerPrefs.GetFloat("Score_3"));
        scores.Add(PlayerPrefs.GetFloat("Score_4"));
        scores.Add(PlayerPrefs.GetFloat("Score_5"));

        names.Add(PlayerPrefs.GetString("Name_1"));
        names.Add(PlayerPrefs.GetString("Name_2"));
        names.Add(PlayerPrefs.GetString("Name_3"));
        names.Add(PlayerPrefs.GetString("Name_4"));
        names.Add(PlayerPrefs.GetString("Name_5"));

        for (int i = 0; i < 5; i++)
        {
            if(scores[i]<=0)
            {
                scores[i] = 1000f;
            }

            if(timer < scores[i])
            {
                scores.Insert(i, timer);
                names.Insert(i, inputField.text);
                break;
            }
        }

        PlayerPrefs.SetFloat("Score_1", scores[0]);
        PlayerPrefs.SetFloat("Score_2", scores[1]);
        PlayerPrefs.SetFloat("Score_3", scores[2]);
        PlayerPrefs.SetFloat("Score_4", scores[3]);
        PlayerPrefs.SetFloat("Score_5", scores[4]);


        PlayerPrefs.SetString("Name_1", names[0]);
        PlayerPrefs.SetString("Name_2", names[1]);
        PlayerPrefs.SetString("Name_3", names[2]);
        PlayerPrefs.SetString("Name_4", names[3]);
        PlayerPrefs.SetString("Name_5", names[4]);

        SceneManager.LoadScene("LoadLeaderboard");

    }
}
