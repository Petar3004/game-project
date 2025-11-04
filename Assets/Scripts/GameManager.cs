using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Where the player respawns after death
    [Header("Player Respawn Settings")]
    public Transform playerSpawnPoint;   // Drag the Room1 start here
    private GameObject player;

    // Reference to the time manager
    private TimeManager_Alexis timeManager;

    void Awake()
    {
        // Singleton pattern – only one GameManager should exist
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Find the player and timer once the level starts
        player = GameObject.FindGameObjectWithTag("Player");
        timeManager = FindObjectOfType<TimeManager_Alexis>();
    }

    // Called when the player dies or the timer hits 0
public void RestartGame()
{
    Debug.Log("Restarting game...");

    // Send the player back to Room 1
    RoomTransitionManager roomManager = FindObjectOfType<RoomTransitionManager>();
    if (roomManager != null)
        roomManager.ResetToRoomOne();

    // Reset the 60 second timer
    if (timeManager != null)
        timeManager.ResetTimer();
}

}
