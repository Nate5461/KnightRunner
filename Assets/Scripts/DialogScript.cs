using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogScript : MonoBehaviour
{
    public GameObject dialogBox;       // Reference to the dialog box UI
    public TMP_Text dialogText;           // Reference to the text in the dialog box

    private bool isPlayerNear = false;
    private bool isDialogOpen = false;

    private int dialogState = 0;

    private void Start()
    {
        if (dialogBox == null)
            {
                Debug.LogError("Dialog Box is not assigned in the inspector.");
            }

            if (dialogText == null)
            {
                Debug.LogError("Dialog Text is not assigned in the inspector.");
            }

        dialogBox.SetActive(false);
    }


    void Update()
    {
        // Open dialog when the player presses a key near the NPC
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            OpenDialog();
        }

        if (isDialogOpen)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (dialogState == 0)
                {
                    contDialog();
                }
                else if (dialogState == 1)
                {
                    contDialog2();
                }
            }
            else if (dialogState == 2)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    Debug.Log("PlayMiniGame");
                    PlayMiniGame();
                }
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    Debug.Log("CloseDialog");
                    CloseDialog();
                    isDialogOpen = false;
                }
            } else if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseDialog();
            }
        }
    }


    public void contDialog()
    {
        dialogText.text = "I challenge you to a game of chess! Beat me and I grant you my powers! If I win you restart the WHOLE game! (Press C...)";
        dialogState = 1;
    
    }


    public void contDialog2()
    {
        dialogText.text = "Are you up for the challenge? (y/n)";
        dialogState = 2;
    }

    public void OpenDialog()
    {
        dialogBox.SetActive(true);
        dialogText.text = "Hey You! (Press C to continue...)";
        isDialogOpen = true;
    }

    public void CloseDialog()
    {
        dialogBox.SetActive(false);
    }

    public void PlayMiniGame()
    {
        SceneManager.LoadScene("chess_minigame"); // Load the mini-game scene
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            CloseDialog();
        }
    }
}
