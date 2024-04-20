using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Item", menuName = "ScriptableObjects/Health Item")]
public class HealthItem : ScriptableObject
{
    public Color color;

    [Range(0, 100)] public int dropChance;

    public int healthRestored;
}
