using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostFXController : MonoBehaviour
{

    public static PostFXController instance;
    public float vigentteDecaySpeed;

    private Volume vol;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        vol = GetComponent<Volume>();
        vol.profile.TryGet(out vignette);
        vol.profile.TryGet(out chromaticAberration);
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

    public void StartSlowMoEffect(float slowMoTime)
    {
        StartCoroutine(DoSlowMoEffect(slowMoTime));
    }

    private IEnumerator DoSlowMoEffect(float slowMoTime)
    {
        while(chromaticAberration.intensity.value < 0.9f)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, 1, slowMoTime * Time.unscaledDeltaTime);
            yield return null;
        }
        while (chromaticAberration.intensity.value > 0.01f)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, 0, slowMoTime * 2 * Time.unscaledDeltaTime);
            yield return null;
        }
        chromaticAberration.intensity.value = 0;
    }
}
