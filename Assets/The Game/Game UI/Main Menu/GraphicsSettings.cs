using TMPro;
using UnityEngine;

public class GraphicsSettings : MonoBehaviour
{
    public TMP_Text setQualityButton;
    public TMP_Text setVsyncText;
    public VsyncController vsyncController; // Reference to the VsyncController script
    private int nextQualityLevel;
    private int currentQualityLevel;

    private void Start()
    {
        LoadQualitySettings();
        UpdateButtonText();
    }

    public void ToggleQuality()
    {
        currentQualityLevel = QualitySettings.GetQualityLevel();

        // Determine the next quality level based on the current one
        nextQualityLevel = currentQualityLevel + 1;

        // If next level exceeds the maximum, reset
        if (nextQualityLevel > 2) 
        {
            nextQualityLevel = 0;
        }

        // Change the quality settings
        QualitySettings.SetQualityLevel(nextQualityLevel);
        SaveQualitySettings();

        // Use the Vsync settings from the VsyncController
        int vsyncCount = vsyncController.GetVsyncCountForQuality(nextQualityLevel);
        ApplyVsync(vsyncCount);

        UpdateButtonText();
        vsyncController.UpdateVsyncButtonText();
    }

    private void ApplyVsync(int vsyncCount)
    {
        QualitySettings.vSyncCount = vsyncCount;
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

        if (setVsyncText != null)
        {
            setVsyncText.text = vsyncController.GetVsyncTextForQuality(QualitySettings.GetQualityLevel());
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
