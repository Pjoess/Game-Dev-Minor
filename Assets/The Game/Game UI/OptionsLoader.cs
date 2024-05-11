using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        VsyncController.instance.LoadVsyncState();
    }
}
