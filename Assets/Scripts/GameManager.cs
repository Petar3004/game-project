using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Rigidbody2D player;
    private Dictionary<int, Vector3[]> spawnPoints = new Dictionary<int, Vector3[]> // two spawn points for each room
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
        player = GameObject.FindGameObjectWithTag("PlayerObject").GetComponent<Rigidbody2D>();
        switch (door)
        {
            case DoorType.ENTRANCE:
                player.transform.position = spawnPoints[roomIndex][0];
                break;
            case DoorType.EXIT:
                player.transform.position = spawnPoints[roomIndex][1];
                break;
        }
        // Reset the velocity so the player doesn't carry any momentum
        player.linearVelocity = Vector2.zero;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RestartGame();
    }

    // Called when the player dies or the timer hits 0
    public void RestartGame()
    {
        Respawn();
    }

    public void Respawn()
    {
        SceneController.instance.GoToRoom(0, DoorType.ENTRANCE);
    }
}
