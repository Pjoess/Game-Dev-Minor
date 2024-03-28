using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenResolution_Controller : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private int currentRefreshRate;
    private int currentResolutionIndex = 0;

    void Start()
    {
        LoadResolution();
    }

    private void LoadResolution()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        // Check if resolution is set in PlayerPrefs, otherwise use current screen resolution
        int screenWidth = PlayerPrefs.GetInt("ScreenWidth", Screen.currentResolution.width);
        int screenHeight = PlayerPrefs.GetInt("ScreenHeight", Screen.currentResolution.height);

        foreach (Resolution res in resolutions)
        {
            if (res.refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(res);
            }
        }

        List<string> options = new List<string>();
        foreach (Resolution res in filteredResolutions)
        {
            string aspectRatio = GetAspectRatio(res.width, res.height);
            string resolutionOption = res.width + "x" + res.height + " (" + aspectRatio + ") " + res.refreshRate + " Hz";
            options.Add(resolutionOption);

            if (res.width == screenWidth && res.height == screenHeight)
            {
                currentResolutionIndex = filteredResolutions.IndexOf(res);
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Set the resolution based on PlayerPrefs or current screen resolution
        Screen.SetResolution(screenWidth, screenHeight, true);
    }

    private string GetAspectRatio(int width, int height)
    {
        float aspectRatio = (float)width / height;
        string aspectRatioString = aspectRatio.ToString("0.##");
        return aspectRatioString;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);

        // Save selected resolution
        PlayerPrefs.SetInt("ScreenWidth", resolution.width);
        PlayerPrefs.SetInt("ScreenHeight", resolution.height);
        PlayerPrefs.Save(); // Save PlayerPrefs to disk
    }
}
