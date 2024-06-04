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
        bool isFpsCounterEnabled = PlayerPrefs.GetInt(FpsToggleKey, 1) == 1; // Default is enabled (1)
        _fpsToggle.isOn = isFpsCounterEnabled;
        _fpsText.gameObject.SetActive(isFpsCounterEnabled);

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
