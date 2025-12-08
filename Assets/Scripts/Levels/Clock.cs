using UnityEngine;

public class Clock : MonoBehaviour
{
    private string riddle;
    private string[] hours = { "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TLEWVE" };
    private string hourStr;
    private int hour;
    private int minuteSteps;
    public ClockControlGear shortHandGear;
    public ClockControlGear longHandGear;
    private int hoursTargetAngle;
    private int minutesTargetAngle;
    private int numRiddlePieces = 0;


    void Start()
    {
        RandomizeRiddle();
    }

    void Update()
    {
        if (AnswerIsCorrect())
        {
            Debug.Log("LEVEL COMPLETE!");
        }
    }

    void RandomizeRiddle()
    {
        hour = Random.Range(0, hours.Length - 1) + 1;
        hourStr = hours[hour - 1];
        minuteSteps = Random.Range(1, longHandGear.clockFaceDivisions + 1);

        int hourStepAngle = 360 / shortHandGear.clockFaceDivisions;
        int minuteStepAngle = 360 / longHandGear.clockFaceDivisions;

        hoursTargetAngle = hour * -hourStepAngle % 360;
        if (hoursTargetAngle < 0) hoursTargetAngle += 360;
        minutesTargetAngle = minuteSteps * -minuteStepAngle % 360;
        if (minutesTargetAngle < 0) minutesTargetAngle += 360;

        riddle =
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
        numRiddlePieces++;
        if (numRiddlePieces == 3)
        {
            Debug.Log(riddle);
        }
    }
}
