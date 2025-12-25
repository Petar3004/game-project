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
    public HashSet<int> unlockedLevels = new HashSet<int>();
    private string levelPath;
    private string unlockedLevelsPath;
    private string unlockedHintsPath;

    void Awake()
    {
        levelPath = Application.persistentDataPath + "/savedLevel.gd";
        unlockedLevelsPath = Application.persistentDataPath + "/unlockedLevels.gd";
        unlockedHintsPath = Application.persistentDataPath + "/unlockedHints.gd";
    }

    void Start()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentLevelIndex != 0)
        {
            gameStarted = true;
            ManagersRoot.instance.sceneController.GoToLevel(currentLevelIndex);
        }

        RetrieveProgress();
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

    public void ResetProgress()
    {
        if (File.Exists(levelPath))
        {
            File.Delete(levelPath);
        }
        savedLevel = -1;

        if (File.Exists(unlockedLevelsPath))
        {
            File.Delete(unlockedLevelsPath);
        }
        unlockedLevels.Clear();

        if (File.Exists(unlockedHintsPath))
        {
            File.Delete(unlockedHintsPath);
        }
        ManagersRoot.instance.hintManager.unlockedHints.Clear();
    }

    public void SaveProgress()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream levelFile = File.Create(levelPath);
        FileStream unlockedLevelsFile = File.Create(unlockedLevelsPath);
        FileStream unlockedHintsFile = File.Create(unlockedHintsPath);
        savedLevel = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(savedLevel);
        bf.Serialize(levelFile, savedLevel);
        bf.Serialize(unlockedLevelsFile, unlockedLevels);
        bf.Serialize(unlockedHintsFile, ManagersRoot.instance.hintManager.unlockedHints);
        levelFile.Close();
        unlockedLevelsFile.Close();
        unlockedHintsFile.Close();
    }

    private void RetrieveProgress()
    {
        if (File.Exists(levelPath))
        {
            FileInfo info = new FileInfo(levelPath);
            if (info.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(levelPath, FileMode.Open);
                savedLevel = (int)bf.Deserialize(file);
                file.Close();
            }
        }
        if (File.Exists(unlockedLevelsPath))
        {
            FileInfo info = new FileInfo(unlockedLevelsPath);
            if (info.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(unlockedLevelsPath, FileMode.Open);
                unlockedLevels.Clear();
                foreach (int level in (HashSet<int>)bf.Deserialize(file))
                {
                    unlockedLevels.Add(level);
                }
                file.Close();
            }
        }
        if (File.Exists(unlockedHintsPath))
        {
            FileInfo info = new FileInfo(unlockedHintsPath);
            if (info.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(unlockedHintsPath, FileMode.Open);
                ManagersRoot.instance.hintManager.unlockedHints.Clear();
                foreach (KeyValuePair<string, HintType> hint in (Dictionary<string, HintType>)bf.Deserialize(file))
                {
                    ManagersRoot.instance.hintManager.unlockedHints.Add(hint.Key, hint.Value);
                }
                file.Close();
            }
        }
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
