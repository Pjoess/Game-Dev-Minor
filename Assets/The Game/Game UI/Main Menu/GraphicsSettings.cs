using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraphicsSettings : MonoBehaviour
{
    public TMP_Text setQualityButton;
    public VsyncController vsyncController; // Reference to the VsyncController script

    private void Start()
    {
        LoadQualitySettings();
        UpdateButtonText();
    }

    public void ToggleQuality()
    {
        int currentQualityLevel = QualitySettings.GetQualityLevel();

        // Determine the next quality level based on the current one
        int nextQualityLevel = currentQualityLevel + 1;

        // If next level exceeds the maximum, wrap around to the first level
        if (nextQualityLevel > 2)
            nextQualityLevel = 0;

        QualitySettings.SetQualityLevel(nextQualityLevel);
        SaveQualitySettings();

        // Toggle Vsync along with quality settings
        ToggleVsync(nextQualityLevel);

        UpdateButtonText();
        vsyncController.UpdateVsyncButtonText(); // Update Vsync button text

        Debug.Log("Quality level toggled. Current level: " + nextQualityLevel);
    }

    private void ToggleVsync(int qualityLevel)
    {
        // If Vsync is enabled, set it based on the current quality level
        if (vsyncController != null && vsyncController.IsVsyncEnabled()) // Call IsVsyncEnabled() as a method
        {
            int vsyncCount = qualityLevel == 2 ? 1 : 0; // Enable Vsync only at High quality level
            vsyncController.ApplyVsync(vsyncCount);
            vsyncController.UpdateVsyncButtonText(); // Update the text of the Vsync button based on Vsync state
        }
    }

    private void UpdateButtonText()
    {
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
        Debug.Log("Quality settings saved. Level: " + qualityLevel);
    }

    private void LoadQualitySettings()
    {
        if (PlayerPrefs.HasKey("QualityLevel"))
        {
            int qualityLevel = PlayerPrefs.GetInt("QualityLevel");
            QualitySettings.SetQualityLevel(qualityLevel);
            Debug.Log("Quality settings loaded. Level: " + qualityLevel);
        }
        else
        {
            // Set default quality level to High if nothing is saved yet
            QualitySettings.SetQualityLevel(2); // High quality
            SaveQualitySettings();
        }
    }
}
