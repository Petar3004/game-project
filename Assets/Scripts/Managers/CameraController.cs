using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private Dictionary<int, Vector3[]> cameraPositions = new Dictionary<int, Vector3[]>
    {
        { 1, new Vector3[] { new Vector3(-17.86f, 0, -10f), new Vector3(0, 0, -10f), new Vector3(35.56f, 0, -10f) } },
        { 2, new Vector3[] { new Vector3(-17.86f, 0, -10f), new Vector3(-17.86f, 10, -10f), new Vector3(-17.86f, 20, -10f) } },
        { 3, new Vector3[] { new Vector3(0, 0, -10f) } },
        { 4, new Vector3[] { new Vector3(0, 0, -10f), new Vector3(17.81f, 0, -10f), new Vector3(9.21f, 10f, -10f) } },
        { 5, new Vector3[] { new Vector3(-17.701f, 0, -10f), new Vector3(17.81f, 0, -10f), new Vector3(9.45f, 20.3f, -10f) } },
        { 6, new Vector3[] { new Vector3(0, 0, -10f) } }
    };

    public int roomIndex = 1;
    // (level, room), height
    private Dictionary<(int, int), int> longVerticalRooms = new Dictionary<(int, int), int>
    {
        { (2, 3), 4 },
        { (3, 1), 2 },
        { (4, 3), 2 },
        { (5, 1), 2 },
        { (5, 2), 2 },
        { (5, 3), 4 },
        { (6, 1), 2 }
    };
    // (level, room), width
    private Dictionary<(int, int), int> longHorizontalRooms = new Dictionary<(int, int), int>
    {
        { (1, 2), 2 },
        { (5, 1), 2 },
        { (5, 2), 2 },
        { (6, 1), 4 }
    };

    public void MoveCameraToNextRoom()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        roomIndex++;
        Camera.main.transform.position = cameraPositions[currentLevelIndex][roomIndex - 1];
    }

    public void MoveCameraToPreviousRoom()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        roomIndex--;
        Camera.main.transform.position = cameraPositions[currentLevelIndex][roomIndex - 1];
    }

    public void MoveCameraToRoom(int targetRoomIndex)
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        roomIndex = targetRoomIndex;
        Camera.main.transform.position = cameraPositions[currentLevelIndex][roomIndex - 1];
    }

    public void TrackPlayer(GameObject player)
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (longVerticalRooms.ContainsKey((currentLevelIndex, roomIndex)))
        {
            float bottomBorder = cameraPositions[currentLevelIndex][roomIndex - 1].y;
            float offset = (longVerticalRooms[(currentLevelIndex, roomIndex)] - 1) * 10;
            float topBorder = bottomBorder + offset;
            float playerY = player.transform.position.y;
            if (playerY > bottomBorder && playerY < topBorder)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, playerY, Camera.main.transform.position.z);
            }
        }
        if (longHorizontalRooms.ContainsKey((currentLevelIndex, roomIndex)))
        {
            float leftBorder = cameraPositions[currentLevelIndex][roomIndex - 1].x;
            float offset = (longHorizontalRooms[(currentLevelIndex, roomIndex)] - 1) * 17.78f;
            float rightBorder = leftBorder + offset;
            float playerX = player.transform.position.x;
            if (playerX > leftBorder && playerX < rightBorder)
            {
                Camera.main.transform.position = new Vector3(playerX, Camera.main.transform.position.y, Camera.main.transform.position.z);
            }
        }
    }
}
