using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameObject player;
    private bool isRestarting = false;  // prevents infinite loops
    // Map of rooms and their (2) corresponding spawn points
    private Vector3[] spawnPoints = new Vector3[] { new Vector3(-26f, -3.2f, 0) };

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

    public void MovePlayerInLevel(int roomIndex)
    {
        player = GameObject.FindGameObjectWithTag("PlayerObject");
        player.transform.position = spawnPoints[roomIndex];
        // Reset velocity after player is moved
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    public void MovePlayerToLevel(int levelIndex)
    {
        // TODO
    }

    // // Called when an object is enabled
    // void OnEnable()
    // {
    //     // Call OnSceneLoaded each time a new scene loads
    //     SceneManager.sceneLoaded += OnSceneLoaded;
    // }

    // // Called when an object is disabled
    // void OnDisable()
    // {
    //     // Stop calling each time
    //     SceneManager.sceneLoaded -= OnSceneLoaded;
    // }

    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     isRestarting = false;
    // }

    // Called when the player dies or the timer hits 0
    public void RestartLevel()
    {
        Debug.Log(isRestarting);
        // if (isRestarting)
        // {
        //     return;
        // }
        // isRestarting = true;
        MovePlayerInLevel(0);
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.ResetHealth();
        Debug.Log(playerHealth.currentHealth);
        CameraController.instance.MoveCameraToRoom(0);
        TimeManager.instance.ResetTimer();
    }
}
