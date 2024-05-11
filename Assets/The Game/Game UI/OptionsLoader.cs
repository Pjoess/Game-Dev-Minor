using UnityEngine;

public class OptionsLoader : MonoBehaviour
{
    void Start()
    {
        // Load Vsync setting from PlayerPrefs, defaulting to 1 (enabled) if not found
        int savedVsync = PlayerPrefs.GetInt("Vsync", 1);
        QualitySettings.vSyncCount = savedVsync;
        Debug.Log("Vsync setting loaded: " + QualitySettings.vSyncCount);
    }

    void OnApplicationQuit()
    {
        // Save current Vsync setting to PlayerPrefs
        PlayerPrefs.SetInt("Vsync", QualitySettings.vSyncCount);
        PlayerPrefs.Save();
        Debug.Log("Vsync setting saved: " + QualitySettings.vSyncCount);
    }
}
