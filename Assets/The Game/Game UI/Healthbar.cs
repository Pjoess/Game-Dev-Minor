using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Player player;
    [SerializeField] TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        slider.maxValue = player.MaxHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = player.HealthPoints;
        text.text = $"{player.HealthPoints} / {player.MaxHealthPoints}";
    }
}
