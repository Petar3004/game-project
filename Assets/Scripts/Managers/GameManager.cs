using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameObject player;

    // Map of levels and their spawn points
    private Dictionary<int, Vector3[]> spawnPoints = new Dictionary<int, Vector3[]>
    {
        { 0, new Vector3[] { new Vector3(-26f, -3.2f, 0f), new Vector3(-7.76f, -3.2f, 0f), new Vector3(10f, -3.2f, 0f) } },
        { 1, new Vector3[] { new Vector3(-11.5f, -4.1f, 0f), new Vector3(-24.68f, 5.78f, 0), new Vector3(-11.53f, 15.84f, 0) } },
        { 2, new Vector3[] { new Vector3(0.12f, -3.7f, 0) } },
        { 3, new Vector3[] { new Vector3(-7.8f, -3.75f, 0), Vector3.zero, Vector3.zero } },
        { 4, new Vector3[] { new Vector3(-20f, -3f, 0f), Vector3.zero, Vector3.zero } },
        { 5, new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero } },
        { 6, new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero } },
        { 7, new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero } },
        { 8, new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero } }
    };

    void Awake()
    {
        // Singleton pattern – only one GameManager should exist
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

    public void MovePlayerToRoom(int roomIndex)
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        player.transform.position = spawnPoints[currentLevelIndex][roomIndex];
        // Reset velocity after player is moved
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    public void MovePlayerToLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
        MovePlayerToRoom(0);
    }

    public void MovePlayerToNextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        MovePlayerToLevel(currentLevelIndex + 1);
    }

    public void MovePlayerToPreviousLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        MovePlayerToLevel(currentLevelIndex - 1);
    }

    // Called when the player dies or the timer hits 0
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        TimeManager.instance.ResetTimer();
        CameraController.instance.roomIndex = 0;
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("PlayerObject");
        }

        CameraController.instance.TrackPlayer(player);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CameraController.instance.MoveCameraToRoom(0);
            MovePlayerToRoom(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CameraController.instance.MoveCameraToRoom(1);
            MovePlayerToRoom(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CameraController.instance.MoveCameraToRoom(2);
            MovePlayerToRoom(2);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }
}
