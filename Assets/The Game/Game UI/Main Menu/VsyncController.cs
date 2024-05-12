using UnityEngine;
using TMPro;

public class VsyncController : MonoBehaviour
{
    public TMP_Text vsyncText;
    public GraphicsSettings graphicsSettings;
    private bool isVsyncEnabled;

    void Start()
    {
        LoadVsyncState();
    }

    void Update()
    {
        UpdateVsyncButtonText();
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
        PlayerPrefs.SetInt("QualityLevel", isVsyncEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadVsyncState()
    {
        int vsyncState = PlayerPrefs.GetInt("QualityLevel", 1);
        isVsyncEnabled = vsyncState == 1;
        ApplyVsync(vsyncState);
        UpdateVsyncButtonText();
    }

    public void UpdateVsyncButtonText()
    {
        vsyncText.text = isVsyncEnabled ? "Vsync On" : "Vsync Off";
    }

    public bool IsVsyncEnabled()
    {
        return isVsyncEnabled = QualitySettings.vSyncCount > 0;
    }
}
