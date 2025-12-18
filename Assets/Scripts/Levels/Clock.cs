using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private string riddleStr;
    private string[] hours = { "TLEWVE", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", };
    private string hourStr;
    private int hour;
    private int minuteSteps;
    public ClockControlGear shortHandGear;
    public ClockControlGear longHandGear;
    private int hoursTargetAngle;
    private int minutesTargetAngle;
    private int numPieces = 0;
    public int maxNumPieces = 3;
    private int hourStepAngle;
    private int minuteStepAngle;
    public TMP_Text riddleTextUI;
    public TMP_Text numPiecesUI;


    void Start()
    {
        hourStepAngle = 360 / shortHandGear.clockFaceDivisions;
        minuteStepAngle = 360 / longHandGear.clockFaceDivisions;

        RandomizeRiddle();

        UpdatePiecesUI(0);
        riddleTextUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (AnswerIsCorrect())
        {
            ManagersRoot.instance.sceneController.GoToMainMenu();
        }
    }

    void RandomizeRiddle()
    {
        hour = Random.Range(0, hours.Length - 1);
        hourStr = hours[hour];
        minuteSteps = Random.Range(1, longHandGear.clockFaceDivisions + 1);

        hoursTargetAngle = hour * -hourStepAngle % 360;
        if (hoursTargetAngle < 0)
        {
            hoursTargetAngle += 360;
        }
        minutesTargetAngle = minuteSteps * -minuteStepAngle % 360;
        if (minutesTargetAngle < 0)
        {
            minutesTargetAngle += 360;
        }

        riddleStr =
        "You can truly catch me once a day.\n" +
        "My short hand says " + hourStr + ", what does yours say?\n" +
        "My long one has taken " + minuteSteps + " steps to the right.\n" +
        "If you don't want to die, guess which time am I.";
    }

    bool AnswerIsCorrect()
    {
        int currentHoursAngle = shortHandGear.handOrientation;
        int currentMinutesAngle = longHandGear.handOrientation;
        return currentHoursAngle == hoursTargetAngle && currentMinutesAngle == minutesTargetAngle;
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
}
