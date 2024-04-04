using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Volume_Manager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    void Start()
    {
        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume",1);
            Load();
        } else {
            Load();
        }
    }

    public void ChangeVolume(){
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    // Load and Save data
    public void Load(){
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    public void Save(){
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

    public void Back(){
        SceneManager.LoadSceneAsync(0);
    }
}
