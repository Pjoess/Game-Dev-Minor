using UnityEngine;
using TMPro;

public class VsyncController : MonoBehaviour
{
    public TMP_Text vsyncText;
    private bool isVsyncEnabled = true;

    void Start()
    {
        LoadVsyncState();
        UpdateVsyncButtonText();
    }

    public void ToggleVSync()
    {
        isVsyncEnabled = !isVsyncEnabled; // Toggle Vsync state
        ApplyVsync(isVsyncEnabled ? 1 : 0);
        UpdateVsyncButtonText();
        SaveVsyncState();
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
        UpdateVsyncButtonText();
    }

    public void UpdateVsyncButtonText()
    {
        if (vsyncText != null)
        {
            vsyncText.text = isVsyncEnabled ? "Vsync On" : "Vsync Off";
        }
    }

    public bool IsVsyncEnabled()
    {
        return isVsyncEnabled;
    }
}
