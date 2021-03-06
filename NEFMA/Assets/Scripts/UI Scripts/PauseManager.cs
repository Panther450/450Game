﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {

    public GameObject pauseMenu;
    public EventSystem eventSys;
    public Selectable resumeButton;

    public Sprite agniPause;
    public Sprite delilahPause;
    public Sprite rykerPause;
    public Sprite kittyPause;

    private Image pauseImage;

    int _pausedPlayer = 0;

    void Start()
    {

        pauseImage = pauseMenu.transform.FindChild("PausePanel").GetComponent<Image>();

    }

    public void pauseGame(int playerInput)
    {
        _pausedPlayer = playerInput;

        Time.timeScale = 0;
        Globals.gamePaused = true;
        // open pause Menu
        pauseMenu.SetActive(true);
        eventSys.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_"+playerInput;
        eventSys.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_"+playerInput;
        eventSys.GetComponent<StandaloneInputModule>().submitButton = "Select_"+playerInput;
        setPauseBackground(playerInput);
        resumeButton.OnSelect(null);
    }

    public void playGame()
    {
        Time.timeScale = 1;
        Globals.gamePaused = false;
        // close pause Menu
        pauseMenu.SetActive(false);
        eventSys.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_-1";
        eventSys.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_-1";
        eventSys.GetComponent<StandaloneInputModule>().submitButton = "Submit";
    }

    public void restartLevel()
    {
        //Debug.Log("Calling Restart Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public Player getPlayer(int playerInput)
    {
        for (int i = 0; i < Globals.players.Count; ++i)
        {
            if (Globals.players[i].InputNum == playerInput)
                return Globals.players[i];
        }
        return null;
    }

    public void PlayerDropOut()
    {
        for (int i = 0; i < Globals.players.Count; ++i)
        {
            Player player = Globals.players[i];

            if(player.InputNum == _pausedPlayer)
            {
                if(player.GO != null)
                {
                    Destroy(player.GO);
                }
                --Globals.livingPlayers;
                Globals.players.Remove(player);
                
                break;
            }
        }
        for (int j = 0; j < Globals.players.Count; ++j)
        {
            Globals.players[j].Number = j;
        }
        playGame();
    }

    void setPauseBackground(int playerInput)
    {

        Player player = getPlayer(playerInput);
        if (player != null)
        {
            if (player.Name == "Agni")
                pauseImage.sprite = agniPause;

            else if (player.Name == "Kitty")
                pauseImage.sprite = kittyPause;

            else if (player.Name == "Ryker")
                pauseImage.sprite = rykerPause;

            else if (player.Name == "Delilah")
                pauseImage.sprite = delilahPause;
        }
        else
        {
            pauseImage.sprite = agniPause;
        }
    }
}
