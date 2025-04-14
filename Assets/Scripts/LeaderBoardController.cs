using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LeaderBoardController : MonoBehaviour
{

    public TMP_Text score1, score2, score3, score4, score5, name1, name2, name3, name4, name5;

    void Start()
    {
        score1.text = PlayerPrefs.GetFloat("Score_1").ToString();
        score2.text = PlayerPrefs.GetFloat("Score_2").ToString();
        score3.text = PlayerPrefs.GetFloat("Score_3").ToString();
        score4.text = PlayerPrefs.GetFloat("Score_4").ToString();
        score5.text = PlayerPrefs.GetFloat("Score_5").ToString();

        name1.text = PlayerPrefs.GetString("Name_1");
        name2.text = PlayerPrefs.GetString("Name_2");
        name3.text = PlayerPrefs.GetString("Name_3");
        name4.text = PlayerPrefs.GetString("Name_4");
        name5.text = PlayerPrefs.GetString("Name_5");

        if (score1.text=="1000" || score1.text == "0")
        {
            score1.text = "---";
        }

        if (score2.text == "1000" || score2.text == "0")
        {
            score2.text = "---";
        }

        if (score3.text == "1000" || score3.text == "0")
        {
            score3.text = "---";
        }

        if (score4.text == "1000" || score4.text == "0")
        {
            score4.text = "---";
        }

        if (score5.text == "1000" || score5.text == "0")
        {
            score5.text = "---";
        }

        if (name1.text == "")
        {
            name1.text = "---";
        }
        if (name2.text == "")
        {
            name2.text = "---";
        }
        if (name3.text == "")
        {
            name3.text = "---";
        }

        if (name4.text == "")
        {
            name4.text = "---";
        }

        if (name5.text == "")
        {
            name5.text = "---";
        }
    }

    public void closeScene(){
        Debug.Log("Closing Scene");
        SceneManager.LoadScene("MainMenu");
    }
}
