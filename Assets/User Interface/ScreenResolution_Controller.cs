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

    [System.Obsolete]
    void Start()
    {
        Resolution();
    }

    [System.Obsolete]
    private void Resolution()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

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

            if (res.width == Screen.width && res.height == Screen.height)
            {
                currentResolutionIndex = filteredResolutions.IndexOf(res);
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
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
    }
}
