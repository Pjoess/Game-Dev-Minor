using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private new Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothness = 5f; // Smoothness factor for health bar update

    private void Awake()
    {
        camera = Camera.main;
        slider = GetComponent<Slider>();
        target = transform.parent; // Assign the target to the parent object
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        StartCoroutine(SmoothHealthUpdate(currentValue / maxValue));
    }

    private IEnumerator SmoothHealthUpdate(float targetValue)
    {
        float currentHealth = slider.value;

        while (Mathf.Abs(currentHealth - targetValue) > 0.01f)
        {
            currentHealth = Mathf.MoveTowards(currentHealth, targetValue, smoothness * Time.deltaTime);
            slider.value = currentHealth;
            yield return null;
        }

        slider.value = targetValue;
    }

    private void Update()
    {
        if (camera != null && target != null)
        {
            transform.rotation = camera.transform.rotation;
            transform.position = target.position + offset;
        }
    }
}
