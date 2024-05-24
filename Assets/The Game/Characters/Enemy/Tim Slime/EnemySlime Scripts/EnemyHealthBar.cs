using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private new Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    void Awake()
    {
        camera = Camera.main;
        slider = GetComponent<Slider>();
        target = transform.parent; // Assign the target to the parent object
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue/maxValue;
    }
    void Update()
    {
        transform.rotation = camera.transform.rotation;
        transform.position = target.position + offset;
    }
}
