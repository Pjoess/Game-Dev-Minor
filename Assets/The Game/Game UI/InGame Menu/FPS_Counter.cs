using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _fpsText;
    [SerializeField] private Toggle _fpsToggle;
    [SerializeField] private float _hudRefreshRate = 1f;

    private float _timer;
    private const string FpsToggleKey = "FpsToggle";

    private void Start()
    {
        // Load the saved state from PlayerPrefs
        int savedState = PlayerPrefs.GetInt(FpsToggleKey, -1);
        if (savedState == -1)
        {
            // If no saved state exists, turn off the FPS counter and toggle
            PlayerPrefs.SetInt(FpsToggleKey, 0);
            _fpsToggle.isOn = false;
            _fpsText.gameObject.SetActive(false);
        }
        else
        {
            // Set the toggle's state based on the loaded value
            _fpsToggle.isOn = savedState == 1;
            _fpsText.gameObject.SetActive(savedState == 1);
        }

        // Add listener to the toggle
        _fpsToggle.onValueChanged.AddListener(OnFpsToggleChanged);
    }

    private void Update()
    {
        if (_fpsText.gameObject.activeSelf && Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            _fpsText.text = "FPS: " + fps;
            _timer = Time.unscaledTime + _hudRefreshRate;
        }
    }

    private void OnFpsToggleChanged(bool isOn)
    {
        // Save the state to PlayerPrefs
        PlayerPrefs.SetInt(FpsToggleKey, isOn ? 1 : 0);
        PlayerPrefs.Save();

        // Enable or disable the FPS counter
        _fpsText.gameObject.SetActive(isOn);
    }
}
