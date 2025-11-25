using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public Rigidbody2D rb;
    private Dictionary<int, Vector3[]> cameraPositions;
    public int currentRoomIndex = 0;
    public int numberOfRooms;
    public static CameraController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (cameraPositions == null || cameraPositions.Count == 0)
        {
            cameraPositions = new Dictionary<int, Vector3[]>
            {
                { 0, new Vector3[] { new Vector3(-17.86f, 0, -10f), new Vector3(0, 0, -10f), new Vector3(17.77f, 0, -10f) } },
                { 1, new Vector3[] { new Vector3(-17.86f, 0, -10f), new Vector3(-17.86f, 10, -10f), new Vector3(-17.86f, 20, -10f) } }
            };
        }
    }

    public void MoveCameraToNextRoom()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        currentRoomIndex += 1;
        Camera.main.transform.position = cameraPositions[currentLevelIndex][currentRoomIndex];
    }

    public void MoveCameraToPreviousRoom()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        currentRoomIndex -= 1;
        Camera.main.transform.position = cameraPositions[currentLevelIndex][currentRoomIndex];
    }

    public void MoveCameraToRoom(int roomIndex)
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        currentRoomIndex = roomIndex;
        Camera.main.transform.position = cameraPositions[currentLevelIndex][roomIndex];
    }
}
