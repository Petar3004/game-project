using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    public RoomDoorOrientation door;
    private Move move;

    // Change room and move player depending on the door
    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            switch (door.doorType)
            {
                case RoomDoorType.VERTICAL:
                    if (player.transform.position.x < transform.position.x)
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
                    if (player.transform.position.y < transform.position.y)
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
            if ((move == Move.RIGHT && player.transform.position.x < transform.position.x) || (move == Move.UP && player.transform.position.y < transform.position.y))
            {
                CameraController.instance.MoveCameraToPreviousRoom();
            }
            else if ((move == Move.LEFT && player.transform.position.x > transform.position.x) || (move == Move.DOWN && player.transform.position.y > transform.position.y))
            {
                CameraController.instance.MoveCameraToNextRoom();
            }
        }
    }
}
