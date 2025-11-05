using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public Rigidbody2D rb;
    private Vector3[] cameraPositions;
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

        if (cameraPositions == null || cameraPositions.Length == 0)
        {
            cameraPositions = new Vector3[]
            {
            new Vector3(-17.86f, 0, -10f),
            new Vector3(0, 0, -10f),
            new Vector3(17.77f, 0, -10f)
            };
        }
    }

    public void MoveCameraToNextRoom()
    {
        currentRoomIndex += 1;
        Camera.main.transform.position = cameraPositions[currentRoomIndex];
    }

    public void MoveCameraToPreviousRoom()
    {
        currentRoomIndex -= 1;
        Camera.main.transform.position = cameraPositions[currentRoomIndex];
    }

    public void MoveCameraToRoom(int roomIndex)
    {
        currentRoomIndex = roomIndex;
        Camera.main.transform.position = cameraPositions[roomIndex];
    }
}
