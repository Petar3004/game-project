using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    public bool gameStarted = false;
    public bool chapterComplete = false;
    public int savedLevel = -1;
    public List<int> unlockedLevels = new List<int>();

    void Start()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentLevelIndex != 0)
        {
            gameStarted = true;
            ManagersRoot.instance.sceneController.GoToLevel(currentLevelIndex);
        }

        if (File.Exists(Application.persistentDataPath + "/savedLevel.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedLevel.gd", FileMode.Open);
            savedLevel = (int)bf.Deserialize(file);
            file.Close();
        }
        if (File.Exists(Application.persistentDataPath + "/unlockedLevels.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/unlockedLevels.gd", FileMode.Open);
            unlockedLevels = (List<int>)bf.Deserialize(file);
            file.Close();
        }
        if (File.Exists(Application.persistentDataPath + "/unlockedHints.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/unlockedHints.gd", FileMode.Open);
            ManagersRoot.instance.hintManager.unlockedHints = (Dictionary<string, HintType>)bf.Deserialize(file);
            file.Close();
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

    // Called when the player dies or the timer hits 0
    public void RestartLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        ManagersRoot.instance.sceneController.GoToLevel(currentLevelIndex);
    }

    public void ResetLevelParameters()
    {
        ManagersRoot.instance.timeManager.ResetTimer();
        ManagersRoot.instance.abilityManager.abilityIsActive = false;
        ManagersRoot.instance.cameraController.roomIndex = 1;
        ManagersRoot.instance.abilityManager.UpdateAbility();
        ManagersRoot.instance.hintManager.HideSmallHint();
    }

    public void SaveProgress()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream levelFile = File.Create(Application.persistentDataPath + "/savedLevel.gd");
        FileStream unlockedLevelsFile = File.Create(Application.persistentDataPath + "/unlockedLevels.gd");
        FileStream unlockedHintsFile = File.Create(Application.persistentDataPath + "/unlockedHints.gd");
        int savedLevel = SceneManager.GetActiveScene().buildIndex;
        bf.Serialize(levelFile, savedLevel);
        bf.Serialize(unlockedLevelsFile, unlockedLevels);
        bf.Serialize(unlockedHintsFile, ManagersRoot.instance.hintManager.unlockedHints);
        levelFile.Close();
        unlockedLevelsFile.Close();
        unlockedHintsFile.Close();
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
                ManagersRoot.instance.pauseManager.Pause(showPauseScreen: true);
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
