using TMPro;
using UnityEngine;

public class GraphicsSettings : MonoBehaviour
{
    public TMP_Text setQualityButton;
    public VsyncController vsyncController;
    private int nextQualityLevel;
    private int currentQualityLevel;

    void Start()
    {
        LoadQualitySettings();
        LoadVsyncState();
    }

    void Update()
    {
        UpdateQualitySettingsButtonText();
    }

    // Save the current quality settings and VSync state before quitting the application.
    void OnApplicationQuit() 
    {
        SaveQualitySettings();
        SaveVsyncState();
    }

    public void ToggleQuality()
    {
        currentQualityLevel = QualitySettings.GetQualityLevel();
        nextQualityLevel = currentQualityLevel + 1;

        if (nextQualityLevel > 2)
        {
            nextQualityLevel = 0;
        }

        // Change the quality settings
        QualitySettings.SetQualityLevel(nextQualityLevel);
        SaveQualitySettings();
        UpdateQualitySettingsButtonText();
    }

    private void UpdateQualitySettingsButtonText()
    {
        vsyncController.vsyncText.text = QualitySettings.vSyncCount > 0 ? "Vsync On" : "Vsync Off";

        switch (QualitySettings.GetQualityLevel())
        {
            case 0:
                setQualityButton.text = "Low";
                break;
            case 1:
                setQualityButton.text = "Medium";
                break;
            case 2:
                setQualityButton.text = "High";
                break;
        }
    }

    private void SaveQualitySettings()
    {
        int qualityLevel = QualitySettings.GetQualityLevel();
        PlayerPrefs.SetInt("QualityLevel", qualityLevel);
        PlayerPrefs.Save();
    }

    private void LoadQualitySettings()
    {
        if (!PlayerPrefs.HasKey("QualityLevel"))
        {
            // If no quality level is set yet, set it to high
            QualitySettings.SetQualityLevel(2);
            SaveQualitySettings();
        }
        else
        {
            // Load the quality level from PlayerPrefs
            int qualityLevel = PlayerPrefs.GetInt("QualityLevel");
            QualitySettings.SetQualityLevel(qualityLevel);
        }
    }

    private void SaveVsyncState()
    {
        PlayerPrefs.SetInt("VSyncState", QualitySettings.vSyncCount);
        PlayerPrefs.Save();
    }

    private void LoadVsyncState()
    {
        int vsyncState = PlayerPrefs.GetInt("VSyncState", 1);
        QualitySettings.vSyncCount = vsyncState;
        vsyncController.UpdateVsyncButtonText();
    }
}
