using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameObject player;
    private bool isRestarting = false;  // prevents infinite loops
    // Map of rooms and their (2) corresponding spawn points
    private Dictionary<int, Vector3[]> spawnPoints = new Dictionary<int, Vector3[]>
    {
        { 0, new Vector3[] { new Vector3(-8f, -3.2f, 0), new Vector3(8f, -3.2f, 0) } },
        { 1, new Vector3[] { new Vector3(-8f, -3.2f, 0), new Vector3(8f, -3.2f, 5) } },
        { 2, new Vector3[] { new Vector3(-8f, -3.2f, 0), new Vector3(8f, -3.2f, 10) } }
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

    public void MovePlayerToSpawnPoint(int roomIndex, DoorType door)
    {
        player = GameObject.FindGameObjectWithTag("PlayerObject");
        switch (door)
        {
            case DoorType.ENTRANCE:
                player.transform.position = spawnPoints[roomIndex][0];
                break;
            case DoorType.EXIT:
                player.transform.position = spawnPoints[roomIndex][1];
                break;
        }
        // Reset velocity after player is moved
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    // Called when an object is enabled
    void OnEnable()
    {
        // Call OnSceneLoaded each time a new scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Called when an object is disabled
    void OnDisable()
    {
        // Stop calling each time
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isRestarting = false;
    }

    // Called when the player dies or the timer hits 0
    public void RestartGame()
    {
        if (isRestarting)
        {
            return;
        }
        isRestarting = true;
        SceneController.instance.GoToRoom(0, DoorType.ENTRANCE);
    }
}
