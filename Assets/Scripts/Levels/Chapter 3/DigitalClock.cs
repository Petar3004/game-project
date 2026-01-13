using System.Text;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DigitalClock : MonoBehaviour
{
    private int digit1 = 0;
    private int digit2 = 0;
    private int digit3 = 0;
    private int digit4 = 0;
    public TMP_Text time;
    public TMP_Text riddleTextUI;
    public TMP_Text numPiecesUI;
    public GameObject portal;
    private string riddleStr;
    private int numPieces = 0;
    private int maxNumPieces = 3;
    private int[] targetValues = { 9, 9, 9, 9 };

    void Start()
    {
        time.text = "00:00";

        RandomizeRiddle();

        UpdatePiecesUI(0);
        riddleTextUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (AnswerIsCorrect())
        {
            portal.SetActive(true);
        }
    }

    private bool AnswerIsCorrect()
    {
        return digit1 == targetValues[0] && digit2 == targetValues[1] && digit3 == targetValues[2] && digit4 == targetValues[3];
    }

    public void IncrementDigit(int index)
    {
        int value;
        switch (index)
        {
            case 1:
                digit1 = (digit1 + 1) % 3;
                value = digit1;
                break;
            case 2:
                if (digit1 < 2)
                {
                    digit2 = (digit2 + 1) % 10;
                }
                else
                {
                    digit2 = (digit2 + 1) % 4;
                }
                value = digit2;
                break;
            case 3:
                digit3 = (digit3 + 1) % 6;
                value = digit3;
                break;
            case 4:
                digit4 = (digit4 + 1) % 10;
                value = digit4;
                break;
            default:
                Debug.Log("No index for incrementing passed");
                value = -1;
                break;
        }
        UpdateTimeUI(index, value);
    }

    private void UpdateTimeUI(int index, int value)
    {
        StringBuilder newTime = new StringBuilder(time.text);
        char digit = (char)('0' + value);

        if (index <= 2)
        {
            newTime[index - 1] = digit;
        }
        else
        {
            newTime[index] = digit;
        }
        time.text = newTime.ToString();
    }

    public void GetRiddlePiece()
    {
        numPieces++;
        if (numPieces == maxNumPieces)
        {
            UpdateRiddleUI();
        }

        UpdatePiecesUI(numPieces);
    }

    private void UpdateRiddleUI()
    {
        riddleTextUI.text = riddleStr;
        numPiecesUI.gameObject.SetActive(false);
        riddleTextUI.gameObject.SetActive(true);
    }

    private void UpdatePiecesUI(int numPieces)
    {
        numPiecesUI.text = numPieces + "/" + maxNumPieces;
    }

    private void RandomizeRiddle()
    {
        int[] positions = { 1, 2, 3, 4 };
        for (int i = 0; i < positions.Length; i++)
        {
            int j = Random.Range(i, positions.Length);
            (positions[i], positions[j]) = (positions[j], positions[i]);
        }

        targetValues[0] = Random.Range(0, 3);
        if (targetValues[0] <= 2)
        {
            targetValues[1] = Random.Range(0, 10);
        }
        else
        {
            targetValues[1] = Random.Range(0, 5);
        }
        targetValues[2] = Random.Range(0, 6);
        targetValues[3] = Random.Range(0, 10);

        riddleStr =
        positions[0] + "=" + targetValues[positions[0] - 1] + "\n" +
        positions[1] + "=" + targetValues[positions[1] - 1] + "\n" +
        positions[2] + "=" + targetValues[positions[2] - 1] + "\n" +
        positions[3] + "=" + targetValues[positions[3] - 1] + "\n";
    }
}
