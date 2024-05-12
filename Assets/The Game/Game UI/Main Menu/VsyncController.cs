using UnityEngine;
using TMPro;

public class VsyncController : MonoBehaviour
{
    public TMP_Text vsyncText;
    private bool isVsyncEnabled;

    void Start()
    {
        LoadVsyncState();
    }

    public void ToggleVSync()
    {
        isVsyncEnabled = !isVsyncEnabled;
        ApplyVsync(isVsyncEnabled ? 1 : 0);
        SaveVsyncState();
    }

    public void ApplyVsync(int vsyncCount)
    {
        QualitySettings.vSyncCount = vsyncCount;
    }

    private void SaveVsyncState()
    {
        PlayerPrefs.SetInt("VSyncState", isVsyncEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadVsyncState()
    {
        int vsyncState = PlayerPrefs.GetInt("VSyncState", 1);
        isVsyncEnabled = vsyncState == 1;
        ApplyVsync(vsyncState);
        UpdateVsyncButtonText();
    }

    public void UpdateVsyncButtonText()
    {
        vsyncText.text = isVsyncEnabled ? "Vsync On" : "Vsync Off";
    }
}
