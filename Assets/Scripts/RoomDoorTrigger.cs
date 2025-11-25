using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RoomDoorType
{
    HORIZONTAL,
    VERTICAL
}

public enum Move
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public class RoomDoorTrigger : MonoBehaviour
{
    public RoomDoorType doorType;
    public GameObject door;
    private Move move;

    // Change room and move player depending on the door
    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            switch (doorType)
            {
                case RoomDoorType.VERTICAL:
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
                case RoomDoorType.HORIZONTAL:
                    if (player.transform.position.y < door.transform.position.y)
                    {
                        move = Move.UP;
                        CameraController.instance.MoveCameraToNextRoom();
                    }
                    else
                    {
                        move = Move.DOWN;
                        CameraController.instance.MoveCameraToPreviousRoom();
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            if ((move == Move.RIGHT && player.transform.position.x < door.transform.position.x) || (move == Move.UP && player.transform.position.y < door.transform.position.y))
            {
                CameraController.instance.MoveCameraToPreviousRoom();
            }
            else if ((move == Move.LEFT && player.transform.position.x > door.transform.position.x) || (move == Move.DOWN && player.transform.position.y > door.transform.position.y))
            {
                CameraController.instance.MoveCameraToNextRoom();
            }
        }

        if ((move == Move.RIGHT && player.transform.position.x > door.transform.position.x) || (move == Move.UP && player.transform.position.y > door.transform.position.y))
        {
            LockDoor();
        }
    }

    private void LockDoor()
    {
        door.GetComponent<BoxCollider2D>().isTrigger = false;
    }
}