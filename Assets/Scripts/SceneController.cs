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
        int roomIndex = SceneManager.GetActiveScene().buildIndex;
        if (roomIndex == 2)
        {
            Debug.Log("You finished the prototype!");
        }
        else
        {
            StartCoroutine(LoadRoomAndMovePlayer(roomIndex + 1, DoorType.ENTRANCE));
        }
    }

    public void GoToPreviousRoom()
    {
        int roomIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadRoomAndMovePlayer(roomIndex - 1, DoorType.EXIT));
    }

    private IEnumerator LoadRoomAndMovePlayer(int roomIndex, DoorType door)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(roomIndex);
        while (!asyncLoad.isDone)
            yield return null;

        GameManager.instance.MovePlayerToSpawnPoint(roomIndex, door);
    }
}
