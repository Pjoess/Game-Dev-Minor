using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{

    public static ScreenShakeManager Instance { get; private set; }

    CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin perlinChannel;
    private float shakeTimer;

    void Awake()
    {
        Instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        perlinChannel = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        perlinChannel.m_AmplitudeGain = intensity;
        shakeTimer = time;
        StartCoroutine(DoShake());
    }

    private IEnumerator DoShake()
    {
        while (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            yield return null;
        }
        perlinChannel.m_AmplitudeGain = 0f;
    }
}
