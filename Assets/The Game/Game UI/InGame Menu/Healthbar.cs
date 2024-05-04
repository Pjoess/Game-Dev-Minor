using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Player_Manager player;
    [SerializeField] TMP_Text text;

    private float smoothness = 50f; // Adjust this value for the speed of health bar update

    void Start()
    {
        player = FindObjectOfType<Player_Manager>();
        slider.maxValue = player.MaxHealthPoints;
    }

    void Update()
    {
        StartCoroutine(SmoothHealthUpdate(player.HealthPoints));
    }

    IEnumerator SmoothHealthUpdate(float targetHealth)
    {
        float currentHealth = slider.value;

        while (currentHealth != targetHealth)
        {
            currentHealth = Mathf.MoveTowards(currentHealth, targetHealth, smoothness * Time.deltaTime);
            slider.value = currentHealth;
            text.text = $"{Mathf.RoundToInt(currentHealth)}";
            yield return null;
        }
    }
}
