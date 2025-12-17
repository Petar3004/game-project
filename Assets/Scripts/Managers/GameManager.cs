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

    // Map of levels and their spawn points
    private Dictionary<int, Vector3[]> spawnPoints = new Dictionary<int, Vector3[]>
    {
        { 1, new Vector3[] { new Vector3(-26f, -3.2f, 0f), new Vector3(-7.76f, -3.2f, 0f), new Vector3(27.83f, -3.2f, 0f) } },
        { 2, new Vector3[] { new Vector3(-11.5f, -4.1f, 0f), new Vector3(-24.68f, 5.78f, 0), new Vector3(-11.53f, 15.84f, 0) } },
        { 3, new Vector3[] { new Vector3(0.12f, -3.7f, 0) } },
        { 4, new Vector3[] { new Vector3(-7.8f, -3.75f, 0), Vector3.zero, Vector3.zero } },
        { 5, new Vector3[] { new Vector3(-20f, -3f, 0f), Vector3.zero, Vector3.zero } },
        { 6, new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero } },
        { 7, new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero } },
        { 8, new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero } },
        { 9, new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero } }
    };

    void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            gameStarted = true;
        }
    }

    void Start()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        ManagersRoot.instance.playerManager.SpawnPlayer(spawnPoints[currentLevelIndex][0]);
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
        ManagersRoot.instance.playerManager.SpawnPlayer(spawnPoints[currentLevelIndex][roomIndex - 1]);
    }

    public void MovePlayerToLevel(int levelIndex)
    {
        ManagersRoot.instance.sceneController.GoToLevel(levelIndex);
        MovePlayerToRoom(1);
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
        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);

        yield return null; // wait 1 frame
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == index);

        ManagersRoot.instance.timeManager.ResetTimer();

        int roomIndex = 1;
        ManagersRoot.instance.cameraController.roomIndex = roomIndex;
        ManagersRoot.instance.playerManager.SpawnPlayer(
            spawnPoints[index][roomIndex - 1]
        );

        SaveProgress();
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
