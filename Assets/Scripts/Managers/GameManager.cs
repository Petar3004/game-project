using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    public bool gameStarted = false;

    void Start()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentLevelIndex != 0)
        {
            gameStarted = true;
            MovePlayerToLevel(currentLevelIndex);
        }
    }

    void Update()
    {
        if (player == null)
        {
            player = ManagersRoot.instance.playerManager.Player;
        }

        if (player == null)
        {
            return;
        }

        ManagersRoot.instance.cameraController.TrackPlayer(player);

        HandleInput();
    }

    public void MovePlayerToRoom(int roomIndex)
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        ManagersRoot.instance.playerManager.SpawnPlayer(currentLevelIndex, roomIndex - 1);
    }

    public void MovePlayerToLevel(int levelIndex)
    {
        ResetLevelParameters();
        ManagersRoot.instance.sceneController.GoToLevel(levelIndex);
    }

    public void MovePlayerToNextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        MovePlayerToLevel(currentLevelIndex + 1);

        SaveProgress();
    }

    public void MovePlayerToPreviousLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        MovePlayerToLevel(currentLevelIndex - 1);

        SaveProgress();
    }

    // Called when the player dies or the timer hits 0
    public void RestartLevel()
    {
        ManagersRoot.instance.sceneController.GoToLevel(SceneManager.GetActiveScene().buildIndex);
        ResetLevelParameters();
        SaveProgress();
    }

    private void ResetLevelParameters()
    {
        ManagersRoot.instance.timeManager.ResetTimer();
        ManagersRoot.instance.abilityManager.abilityIsActive = false;
        ManagersRoot.instance.cameraController.roomIndex = 1;
        ManagersRoot.instance.abilityManager.UpdateAbility();
    }

    public void SaveProgress()
    {
        // TODO save level in storage
    }

    private void HandleInput()
    {
        if (gameStarted && !ManagersRoot.instance.pauseManager.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ManagersRoot.instance.cameraController.MoveCameraToRoom(1);
                MovePlayerToRoom(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ManagersRoot.instance.cameraController.MoveCameraToRoom(2);
                MovePlayerToRoom(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ManagersRoot.instance.cameraController.MoveCameraToRoom(3);
                MovePlayerToRoom(3);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                RestartLevel();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                ManagersRoot.instance.pauseManager.Pause();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ManagersRoot.instance.pauseManager.Resume();
            }
        }
    }
}
