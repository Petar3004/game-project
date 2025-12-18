using UnityEngine;

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

public class RoomDoorOrientation : MonoBehaviour
{
    public RoomDoorType doorType;

    private void OnValidate()
    {
        switch (doorType)
        {
            case RoomDoorType.VERTICAL:
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                gameObject.layer = 8;
                foreach (Transform child in transform)
                {
                    child.gameObject.layer = 8;
                }
                break;

            case RoomDoorType.HORIZONTAL:
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                gameObject.layer = 7;
                foreach (Transform child in transform)
                {
                    child.gameObject.layer = 7;
                }
                break;
        }
    }
}