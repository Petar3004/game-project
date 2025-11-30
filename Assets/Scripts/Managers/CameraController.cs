using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private Dictionary<int, Vector3[]> cameraPositions;

    public int roomIndex = 0;
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
                { 1, new Vector3[] { new Vector3(-17.86f, 0, -10f), new Vector3(-17.86f, 10, -10f), new Vector3(-17.86f, 20, -10f) } },
                { 2, null},
                { 3, new Vector3[] { new Vector3(0, 0, -10f), new Vector3(17.81f, 0, -10f), new Vector3(9.21f, 10f, -10f) } },
                { 4, null},
                { 5, null}
            };
        }

    }

    public void MoveCameraToNextRoom()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        roomIndex += 1;
        Camera.main.transform.position = cameraPositions[currentLevelIndex][roomIndex];
    }

    public void MoveCameraToPreviousRoom()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        roomIndex -= 1;
        Camera.main.transform.position = cameraPositions[currentLevelIndex][roomIndex];
    }

    public void MoveCameraToRoom(int targetRoomIndex)
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        roomIndex = targetRoomIndex;
        Camera.main.transform.position = cameraPositions[currentLevelIndex][roomIndex];
    }
}
