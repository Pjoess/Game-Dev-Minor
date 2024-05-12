using TMPro;
using Unity.VisualScripting;
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
    }

    void Update()
    {
        UpdateQualitySettingsButtonText();
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
            // If no quality level is set yet, set it to high and enable VSync
            QualitySettings.SetQualityLevel(2);
            QualitySettings.vSyncCount = 1;
            SaveQualitySettings();
        }
        else
        {
            // Load the quality level from PlayerPrefs
            int qualityLevel = PlayerPrefs.GetInt("QualityLevel");
            QualitySettings.SetQualityLevel(qualityLevel);
        }
    }
}
