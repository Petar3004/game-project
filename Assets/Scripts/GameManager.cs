using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> doors;
    public static GameManager instance;
    private GameObject player;
    // private bool isRestarting = false;  // prevents infinite loops

    // Map of levels and their spawn points
    private Dictionary<int, Vector3[]> spawnPoints = new Dictionary<int, Vector3[]>
    {
        { 0, new Vector3[] { new Vector3(-26f, -3.2f, 0f), Vector3.zero, Vector3.zero } },
        { 1, new Vector3[] { new Vector3(-11.5f, -4.1f, 0f), Vector3.zero, Vector3.zero } },
        { 2, new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero } },
        { 3, new Vector3[] { new Vector3(-7.8f, -3.75f, 0), Vector3.zero, Vector3.zero } },
        { 4, new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero } },
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
        player = GameObject.FindGameObjectWithTag("PlayerObject");
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        player.transform.position = spawnPoints[currentLevelIndex][roomIndex];
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
        // if (isRestarting)
        // {
        //     return;
        // }
        // isRestarting = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void UnlockDoors()
    {
        foreach (GameObject door in doors)
        {
            door.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
