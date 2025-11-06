using UnityEngine;

public class DayNightController : MonoBehaviour
{
    // The main directional light for the sun
    [Tooltip("The Directional Light representing the sun.")]
    public Light sun;

    // How long a full day-night cycle should be in real-world seconds
    [Tooltip("Duration of a full day in seconds.")]
    public float secondsPerDay = 120f;

    // A value from 0 (midnight) to 1 (next midnight)
    [Tooltip("Current time of day (0.0 to 1.0).")]
    [Range(0, 1)]
    public float timeOfDay = 0.5f; // Start at noon

    // --- Lighting Preset Gradients ---

    [Header("Lighting & Color")]
    [Tooltip("Controls the ambient light color based on time of day. This provides 'moonlight' at night.")]
    public Gradient ambientColor;

    [Tooltip("Controls the sun's direct light color based on time of day (e.g., orange at sunset).")]
    public Gradient directionalColor;

    [Tooltip("Controls the scene's fog color based on time of day.")]
    public Gradient fogColor;


    [Header("Win Condition")]
    [Tooltip("How many days must pass for the player to win.")]
    public int daysToWin = 5;

    [Tooltip("The current day count. Starts at 1.")]
    public int currentDay = 1;

    // Private variable to stop the win logic from firing more than once
    private bool isGameWon = false;



    void Update()
    {
        // If the game is already won, we don't need to do anything.
        if (isGameWon)
        {
            return;
        }

        // 1. Advance the time of day
        timeOfDay += Time.deltaTime / secondsPerDay;
        
        // 2. --- NEW: Check if a day has passed ---
        if (timeOfDay >= 1.0f)
        {
            timeOfDay %= 1.0f; // Loop time back to 0
            DayPassed();       // Call the new DayPassed function
        }
        // --- END NEW ---

        // 3. Update all lighting based on the new time
        UpdateLighting(timeOfDay);
    }

    /// <summary>
    /// Updates the sun's rotation and all environment lighting colors.
    /// </summary>
    /// <param name="timePercent">The current time as a value from 0 to 1.</param>
    private void UpdateLighting(float timePercent)
    {
        //  Sun Rotation 
        float sunRotation = (timePercent - 0.25f) * 360f;
        sun.transform.localRotation = Quaternion.Euler(sunRotation, -30f, 0f);

        //  Light & Fog Colors 
        float colorTime = timePercent;

        // Set the colors
        RenderSettings.ambientLight = ambientColor.Evaluate(colorTime);
        DynamicGI.UpdateEnvironment(); // The fix we added earlier
        RenderSettings.fogColor = fogColor.Evaluate(colorTime);
        
        if (sun != null)
        {
            sun.color = directionalColor.Evaluate(colorTime);
        }
    }

    // Function to handle a day passing 
    private void DayPassed()
    {
        currentDay++;
        Debug.Log("A new day has begun! It is now Day " + currentDay);

        // Check if the win condition is met
        if (currentDay > daysToWin)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        isGameWon = true;
        Debug.Log("YOU WIN! You survived " + daysToWin + " days.");


    }

}