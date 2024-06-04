using UnityEngine;
using System.Collections;

public class DayNightTimer : MonoBehaviour
{
    public float cycleDuration = 50.0f;
    public float startRotation = 100.0f;
    public float endRotation = 0f;
    public float waitTime = 30.0f;

    void Start()
    {
        StartCoroutine(RotateDayNightCycle());
    }

    private IEnumerator RotateDayNightCycle()
    {
        while (true)
        {
            yield return StartCoroutine(Rotate(startRotation, endRotation));
            yield return new WaitForSeconds(waitTime);
            yield return StartCoroutine(Rotate(endRotation, startRotation));
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator Rotate(float fromAngle, float toAngle)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < cycleDuration)
        {
            elapsedTime += Time.deltaTime;
            float cycleProgress = elapsedTime / cycleDuration;
            float rotationAngle = Mathf.Lerp(fromAngle, toAngle, cycleProgress);
            transform.localRotation = Quaternion.Euler(rotationAngle, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
            yield return null;
        }

        transform.localRotation = Quaternion.Euler(toAngle, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }
}
