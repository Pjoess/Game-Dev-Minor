using UnityEngine;
using System.Collections;

public class DayNightTimer : MonoBehaviour
{
    private float cycleDuration = 300f;
    private float startRotation = 90f;
    private float endRotation = 0f;
    private float waitTime = 200f;

    void Start()
    {
        // // Set the initial rotation's x-angle to 100
        // transform.localRotation = Quaternion.Euler(100f, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

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
