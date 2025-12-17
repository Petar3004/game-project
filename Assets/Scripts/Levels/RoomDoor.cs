using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    public RoomDoorOrientation door;
    private Move move;

    // Change room and move player depending on the door
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (door.doorType)
            {
                case RoomDoorType.VERTICAL:
                    if (other.transform.position.x < transform.position.x)
                    {
                        move = Move.RIGHT;
                        ManagersRoot.instance.cameraController.MoveCameraToNextRoom();
                    }
                    else
                    {
                        move = Move.LEFT;
                        ManagersRoot.instance.cameraController.MoveCameraToPreviousRoom();
                    }
                    break;
                case RoomDoorType.HORIZONTAL:
                    if (other.transform.position.y < transform.position.y)
                    {
                        move = Move.UP;
                        ManagersRoot.instance.cameraController.MoveCameraToNextRoom();
                    }
                    else
                    {
                        move = Move.DOWN;
                        ManagersRoot.instance.cameraController.MoveCameraToPreviousRoom();
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if ((move == Move.RIGHT && other.transform.position.x < transform.position.x) || (move == Move.UP && other.transform.position.y < transform.position.y))
            {
                ManagersRoot.instance.cameraController.MoveCameraToPreviousRoom();
            }
            else if ((move == Move.LEFT && other.transform.position.x > transform.position.x) || (move == Move.DOWN && other.transform.position.y > transform.position.y))
            {
                ManagersRoot.instance.cameraController.MoveCameraToNextRoom();
            }
        }
    }
}
