using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private Dictionary<int, Vector3[]> cameraPositions = new Dictionary<int, Vector3[]>
    {
        { 0, new Vector3[] { new Vector3(-17.86f, 0, -10f), new Vector3(0, 0, -10f), new Vector3(17.77f, 0, -10f) } },
        { 1, new Vector3[] { new Vector3(-17.86f, 0, -10f), new Vector3(-17.86f, 10, -10f), new Vector3(-17.86f, 20, -10f) } },
        { 2, null},
        { 3, new Vector3[] { new Vector3(0, 0, -10f), new Vector3(17.81f, 0, -10f), new Vector3(9.21f, 10f, -10f) } },
        { 4, null},
        { 5, null}
    };

    public int roomIndex = 0;
    public static CameraController instance;
    public Dictionary<int, (int, int)> longVerticalRooms = new Dictionary<int, (int, int)>
    {
        { 1, (2, 2) }
    };
    public Dictionary<int, (int, int)> longHorizontalRooms = new Dictionary<int, (int, int)>
    {
    };
    public GameObject player;

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

    void LateUpdate()
    {

    }

    private void trackPlayer()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (longVerticalRooms[currentLevelIndex].Item1 == roomIndex)
        {
            float bottomBorder = cameraPositions[currentLevelIndex][roomIndex].y;
            float topBorder = cameraPositions[currentLevelIndex][roomIndex].y + longVerticalRooms[currentLevelIndex].Item2 * 5;
            float playerY = player.transform.position.y;
            if (playerY > bottomBorder && playerY < topBorder)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, playerY, Camera.main.transform.position.z);
            }
        }
        if (longHorizontalRooms[currentLevelIndex].Item1 == roomIndex)
        {
            float leftBorder = cameraPositions[currentLevelIndex][roomIndex].x;
            float rightBorder = cameraPositions[currentLevelIndex][roomIndex].x + longHorizontalRooms[currentLevelIndex].Item2 * 8.89f;
            float playerX = player.transform.position.x;
            if (playerX > leftBorder && playerX < rightBorder)
            {
                Camera.main.transform.position = new Vector3(playerX, Camera.main.transform.position.x, Camera.main.transform.position.z);
            }
        }
    }
}
