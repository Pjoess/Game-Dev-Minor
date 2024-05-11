using UnityEngine;
using TMPro;

public class VsyncController : MonoBehaviour
{
    public static VsyncController instance;
    public TMP_Text vsyncButtonText;
    private bool isVsyncEnabled = true; // Default Vsync is enabled

    void Start()
    {
        instance = this;
        LoadVsyncState(); // Load Vsync state when the game starts
        UpdateVsyncButtonText(); // Update the text of the button based on Vsync state
        // Debug logs for testing
        Debug.Log("Vsync Controller initialized.");
        Debug.Log("Initial Vsync state: " + (isVsyncEnabled ? "Enabled" : "Disabled"));
    }

    public void ToggleVSync()
    {
        isVsyncEnabled = !isVsyncEnabled; // Toggle Vsync state
        QualitySettings.vSyncCount = isVsyncEnabled ? 1 : 0; // Apply Vsync state
        SaveVsyncState(); // Save Vsync state
        UpdateVsyncButtonText(); // Update the text of the button based on Vsync state

        // Debug log for testing
        Debug.Log("Vsync toggled. New state: " + (isVsyncEnabled ? "Enabled" : "Disabled"));
    }

    private void SaveVsyncState()
    {
        PlayerPrefs.SetInt("VsyncEnabled", isVsyncEnabled ? 1 : 0); // Save Vsync state
        PlayerPrefs.Save(); // Save PlayerPrefs to disk

        // Debug log for testing
        Debug.Log("Vsync state saved: " + (isVsyncEnabled ? "Enabled" : "Disabled"));
    }

    public void LoadVsyncState()
    {
        int vsyncState = PlayerPrefs.GetInt("VsyncEnabled", 1); // Load Vsync state, default is enabled (1)
        isVsyncEnabled = vsyncState == 1;
        QualitySettings.vSyncCount = vsyncState;

        // Debug log for testing
        Debug.Log("Vsync state loaded: " + (isVsyncEnabled ? "Enabled" : "Disabled"));
    }

    private void UpdateVsyncButtonText()
    {
        if (vsyncButtonText != null)
        {
            vsyncButtonText.text = isVsyncEnabled ? "Vsync On" : "Vsync Off";
        }
    }
}
