using UnityEngine;

public class RoomTransitionManager : MonoBehaviour
{
    [Header("Room Settings")]
    public Transform[] roomStartPoints;   // one spawn point for each room
    private int currentRoomIndex = 0;

    private GameObject player;

    void Start()
    {
        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player");

        // Move the player to Room 1 at the start
        if (roomStartPoints.Length > 0 && player != null)
        {
            player.transform.position = roomStartPoints[0].position;
            currentRoomIndex = 0;
        }
    }

    // Call from RoomTransition when player touches the exitTrigger
    public void GoToNextRoom()
    {
        // check if there is another room ahead
        if (currentRoomIndex < roomStartPoints.Length - 1)
        {
            currentRoomIndex++;
            MovePlayerToCurrentRoom();
        }
        else
        {
            Debug.Log("Level Complete!");
            // We can then later load a new scene or show victory screen here
        }
    }

    // This Moves the player to the current room’s spawn point
    private void MovePlayerToCurrentRoom()
    {
        if (player != null)
        {
            player.transform.position = roomStartPoints[currentRoomIndex].position;

            // Reset the velocity so the player doesn't carry any momentum
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }
    }

    // Called by GameManager when the player dies
    public void ResetToRoomOne()
    {
        currentRoomIndex = 0;
        MovePlayerToCurrentRoom();
    }
}
