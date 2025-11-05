using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

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

    public void GoToNextRoom()
    {
        int currentRoomIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentRoomIndex == 2)
        {
            Debug.Log("You finished the prototype!");
        }
        else
        {
            StartCoroutine(LoadRoomAndMovePlayer(currentRoomIndex + 1, DoorType.ENTRANCE));
        }
    }

    public void GoToPreviousRoom()
    {
        int currentRoomIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadRoomAndMovePlayer(currentRoomIndex - 1, DoorType.EXIT));
    }

    public void GoToRoom(int roomIndex, DoorType door)
    {
        StartCoroutine(LoadRoomAndMovePlayer(roomIndex, door));
    }

    private IEnumerator LoadRoomAndMovePlayer(int roomIndex, DoorType door)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(roomIndex);
        while (!asyncLoad.isDone)
            yield return null;

        GameManager.instance.MovePlayerToSpawnPoint(roomIndex, door);
    }
}
