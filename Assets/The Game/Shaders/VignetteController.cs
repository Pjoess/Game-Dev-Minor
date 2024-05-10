using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteController : MonoBehaviour
{

    public static VignetteController instance;
    public float vigentteDecaySpeed;

    private Vignette vignette;
    private Volume vol;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        vol = GetComponent<Volume>();
        vol.profile.TryGet<Vignette>(out vignette);
    }

    // Update is called once per frame
    void Update()
    {
        if (vignette.intensity.value > 0)
        {
            vignette.intensity.value -= vigentteDecaySpeed * Time.deltaTime;
        }
    }

    public void SetVignetterInstesity(float intensity)
    {
        vignette.intensity.value = intensity;
    }
}
