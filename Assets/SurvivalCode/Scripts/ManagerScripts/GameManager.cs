using TMPro;
using UnityEngine;
using UnityEngine.UI; // Required for using UI elements like Text

public class GameManager : MonoBehaviour
{
    public DayNightController dayNightController; // Reference to the DayNightCycle script
    public int daysToWin = 3; 
    public TextMeshProUGUI winText; // A UI Text element to display the win message

    public GameObject victory;

    private int currentDay = 0;
    private float previousTimeOfDay = 0;
    private bool hasWon = false;
    public static bool isInventoryOpen = false;
    public static bool IsShopOpen = false;
    public static bool IsGameOver = false;
    public static bool enables = false;

    public static bool IsPlayerInputFrozen = false;

    // Initialize static variables
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsPlayerInputFrozen = false;
        isInventoryOpen = false;
        IsShopOpen = false;
        IsGameOver = false;
        enables = false;

        Time.timeScale = 1f;
        


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

    // Handles the win condition
    public void TriggerWin()
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