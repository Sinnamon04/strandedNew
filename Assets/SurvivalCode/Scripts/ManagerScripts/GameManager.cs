using TMPro;
using UnityEngine;
using UnityEngine.UI; // Required for using UI elements like Text

public class GameManager : MonoBehaviour
{
    public DayNightController dayNightController; // Reference to your DayNightCycle script
    public int daysToWin = 3; // The number of days the player needs to survive to win
    public TextMeshProUGUI winText; // A UI Text element to display the win message

    public GameObject victory;

    private int currentDay = 0;
    private float previousTimeOfDay = 0;
    private bool hasWon = false;

    void Awake()
    {
        // Ensure the win text is hidden at the start of the game
        victory.gameObject.SetActive(false);
    }

    void Update()
    {
        // If the game has already been won, do nothing
        if (hasWon) return;

        // Check if the timeOfDay has wrapped around from 1 back to 0, indicating a new day
        if (dayNightController.timeOfDay < previousTimeOfDay)
        {
            currentDay++;
        }

        previousTimeOfDay = dayNightController.timeOfDay;

        // Check if the win condition has been met
        if (currentDay >= daysToWin)
        {
            hasWon = true;
            TriggerWin();
        }
    }

    void TriggerWin()
    {
        // Display the win message
        if (victory != null)
        {
            TextMeshProUGUI winText = victory.GetComponentInChildren<TextMeshProUGUI>();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            victory.gameObject.SetActive(true);
        }

        
        
    }
}