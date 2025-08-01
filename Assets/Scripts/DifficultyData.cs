using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "Data/Difficulty")]
public class DifficultyData : ScriptableObject
{
    [SerializeField] private List<AnimalType> animalTypes; // In order of animals enum
    public List<AnimalType> AnimalTypes => animalTypes;

    [SerializeField] private List<float> spawnChances;
    public List<float> SpawnChances => spawnChances;
}
