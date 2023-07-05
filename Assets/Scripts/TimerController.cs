using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public static TimerController Agent;
    [HideInInspector] public Text timerText;
    private float rawTime;
    [HideInInspector] public bool canRunTimer;
    private string minutes, seconds, milliseconds;

    private void Awake() { Agent = this; }

    private void Start() { timerText = GameplayUIManager.Agent.TimerText; }

    private void Update()
    {
        if (canRunTimer)
        {
            rawTime += 1 * Time.deltaTime;
            DisplayTimer();

            if (rawTime > 60)
                GameplayUIManager.Agent.TimerText.color = Color.red;
        }
    }

    private void DisplayTimer()
    {
        minutes = Mathf.Floor(rawTime / 60).ToString("00");
        seconds = Mathf.Floor(rawTime % 60).ToString("00");
        milliseconds = Mathf.Floor((Time.time * 100) % 100).ToString("00");
        timerText.text = minutes + ":" + seconds + ":" + milliseconds;
    }

    public float GetMinutes() { return Mathf.Floor(rawTime / 60); }
}
