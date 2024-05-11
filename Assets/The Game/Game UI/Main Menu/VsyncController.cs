using UnityEngine;
using TMPro;

public class VsyncController : MonoBehaviour
{
    public TMP_Text vsyncButtonText;
    private bool isVsyncEnabled = true; // Default Vsync is enabled

    void Start()
    {
        LoadVsyncState(); // Load Vsync state when the game starts
        UpdateVsyncButtonText(); // Update the text of the button based on Vsync state
    }

    public void ToggleVSync()
    {
        isVsyncEnabled = !isVsyncEnabled; // Toggle Vsync state
        ApplyVsync(isVsyncEnabled ? 1 : 0); // Apply Vsync state
        SaveVsyncState(); // Save Vsync state
        UpdateVsyncButtonText(); // Update the text of the button based on Vsync state
    }

    public void ApplyVsync(int vsyncCount)
    {
        QualitySettings.vSyncCount = vsyncCount; // Apply Vsync count
    }

    private void SaveVsyncState()
    {
        PlayerPrefs.SetInt("Vsync", isVsyncEnabled ? 1 : 0); // Save Vsync state
        PlayerPrefs.Save(); // Save PlayerPrefs to disk
    }

    private void LoadVsyncState()
    {
        int vsyncState = PlayerPrefs.GetInt("Vsync", 1); // Load Vsync state, default is enabled (1)
        isVsyncEnabled = vsyncState == 1;
        ApplyVsync(vsyncState);
    }

    public void UpdateVsyncButtonText()
    {
        if (vsyncButtonText != null)
        {
            vsyncButtonText.text = isVsyncEnabled ? "Vsync On" : "Vsync Off";
        }
    }

    public bool IsVsyncEnabled()
    {
        return isVsyncEnabled;
    }
}
