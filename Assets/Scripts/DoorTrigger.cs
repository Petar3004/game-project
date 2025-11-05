using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    ENTRANCE,
    EXIT
}

public class DoorTrigger : MonoBehaviour
{
    public DoorType doorType;

    // Change scene and move player depending on the door
    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            switch (doorType)
            {
                case DoorType.ENTRANCE:
                    SceneController.instance.GoToPreviousRoom();
                    break;
                case DoorType.EXIT:
                    SceneController.instance.GoToNextRoom();
                    break;
            }
        }
    }
}
