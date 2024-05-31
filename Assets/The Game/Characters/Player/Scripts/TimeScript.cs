using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeScript : MonoBehaviour
{
    public static TimeScript instance;
    [SerializeField] private float hitStopTime = 0.1f;
    [SerializeField] private float slowMoTime = 2f;
    [SerializeField] private float slowMoTimeScale = 0.5f;

    private bool stopped = false;

    private Coroutine hitStopRoutine;

    private void Start()
    {
        instance = this;
    }

    public void HitStop()
    {
        if(!stopped)
        {
            hitStopRoutine = StartCoroutine(HitStopCoroutine());
        }
    }

    private IEnumerator HitStopCoroutine()
    {
        stopped = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitStopTime);
        Time.timeScale = 1f;
        stopped = false;
    }

    public void SlowMo()
    {
        if (hitStopRoutine != null)
        {
            StopCoroutine(hitStopRoutine);
            Time.timeScale = 1f;
            stopped = false;
        }
        if (!stopped)
        {
            StartCoroutine(SlowMoCoroutine());
        }
    }

    private IEnumerator SlowMoCoroutine()
    {
        stopped = true;
        Time.timeScale = slowMoTimeScale;
        yield return new WaitForSecondsRealtime(slowMoTime);
        Time.timeScale = 1f;
        stopped = false;
    }
}
