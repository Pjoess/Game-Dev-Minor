using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeScript : MonoBehaviour
{
    public static TimeScript instance;
    [SerializeField] private float hitStopTime = 0.1f;

    private bool stopped = false;

    private void Start()
    {
        instance = this;
    }

    public void hitStop()
    {
        if(!stopped)
        {
            StartCoroutine(hitStopCoroutine());
        }
    }

    private IEnumerator hitStopCoroutine()
    {
        stopped = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitStopTime);
        Time.timeScale = 1f;
        stopped = false;
    }
}
