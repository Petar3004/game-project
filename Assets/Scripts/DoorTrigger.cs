using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DoorType
{
    ROOM,
    LEVEL
}

public enum Move
{
    LEFT,
    RIGHT
}

public class DoorTrigger : MonoBehaviour
{
    public DoorType doorType;
    public GameObject door;
    private Move move;

    // Change room and move player depending on the door
    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            switch (doorType)
            {
                case DoorType.ROOM:
                    if (player.transform.position.x < door.transform.position.x)
                    {
                        move = Move.RIGHT;
                        CameraController.instance.MoveCameraToNextRoom();
                    }
                    else
                    {
                        move = Move.LEFT;
                        CameraController.instance.MoveCameraToPreviousRoom();
                    }
                    break;
                case DoorType.LEVEL:
                    if (player.transform.position.x < door.transform.position.x)
                    {
                        SceneController.instance.GoToNextLevel();
                    }
                    else
                    {
                        SceneController.instance.GoToPreviousLevel();
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            if (doorType == DoorType.ROOM && move == Move.RIGHT && player.transform.position.x < door.transform.position.x)
            {
                CameraController.instance.MoveCameraToPreviousRoom();
            }
            else if (doorType == DoorType.ROOM && move == Move.LEFT && player.transform.position.x > door.transform.position.x)
            {
                CameraController.instance.MoveCameraToNextRoom();
            }
        }
    }
}