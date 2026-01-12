using System.Text;
using NUnit.Framework;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class DigitalClock : MonoBehaviour
{
    private int digit1 = 0;
    private int digit2 = 0;
    private int digit3 = 0;
    private int digit4 = 0;
    public TMP_Text time;

    void Start()
    {
        time.text = "00:00";
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
}
