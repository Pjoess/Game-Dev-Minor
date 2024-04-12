using UnityEngine;

public class HowToPlayScript : MonoBehaviour
{
    [SerializeField] private GameObject closePanel;

    public void ClosePanel()
    {
        if(closePanel != null){
            closePanel.SetActive(false);
            Time.timeScale = 1;
        } else {
            closePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
