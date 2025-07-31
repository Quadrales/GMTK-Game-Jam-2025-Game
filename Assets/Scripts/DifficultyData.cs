using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "Data/Difficulty")]
public class DifficultyData : ScriptableObject
{
    public enum Animals
    {
        Cow,
        Pig,
        Chicken
    };

    [SerializeField] private List<float> animalSpawnChances; // In order of animals enum
}
