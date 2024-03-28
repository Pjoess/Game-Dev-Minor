using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenResolution_Controller : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private int currentResolutionIndex = 0;

    [System.Obsolete]
    void Start()
    {
        // Start the game in fullscreen mode
        Screen.fullScreen = true;
        
        // Set the resolution of the current device
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

        ScreenResolution();
    }

    [System.Obsolete]
    private void ScreenResolution()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        int currentWidth = Screen.currentResolution.width;
        int currentHeight = Screen.currentResolution.height;

        resolutionDropdown.ClearOptions();

        foreach (Resolution res in resolutions)
        {
            if (res.width == currentWidth && res.height == currentHeight)
            {
                filteredResolutions.Add(res);
            }
        }

        List<string> options = new List<string>();
        foreach (Resolution res in filteredResolutions)
        {
            string resolutionOption = res.width + "x" + res.height + " " + res.refreshRate + " Hz";
            options.Add(resolutionOption);

            if (res.width == currentWidth && res.height == currentHeight)
            {
                currentResolutionIndex = filteredResolutions.IndexOf(res);
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
}
