using UnityEngine;

public class OptionsLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Load Vsync state (Does not work when Options are Disabled)
        VsyncController.instance.LoadVsyncState();
        
        // Enable Vsync immediately on Game Start
        QualitySettings.vSyncCount = 1;
    }
}
