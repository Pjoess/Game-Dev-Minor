using TMPro;
using UnityEngine;

public class GraphicsSettings : MonoBehaviour
{
    public TMP_Text setQualityButton;
    public VsyncController vsyncController;
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
        UpdateButtonText();
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
