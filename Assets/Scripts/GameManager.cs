using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private DifficultyData difficultyData;

    [SerializeField] private GameObject playerPrefab;
    private Vector3 player1StartPos = new Vector3(8, 8, 0);
    private Vector3 player2StartPos = new Vector3(20, 8, 0);

    [SerializeField] private GameObject cowPrefab;
    [SerializeField] private GameObject pigPrefab;
    [SerializeField] private GameObject chickenPrefab;

    [SerializeField] private BoxCollider2D animalSpawnArea;
    private Coroutine coroutine;

    private int maxAnimals = 30;
    private int spawnedAnimalsCount = 0;

    public void SetDifficultyData(DifficultyData data)
    {
        difficultyData = data;
    }

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(playerPrefab, player1StartPos, Quaternion.identity);
        Instantiate(playerPrefab, player2StartPos, Quaternion.identity);

        coroutine = StartCoroutine(SpawnAnimalsCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnAnimalsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 6f));

            AnimalType animal = TrySpawnAnimal();
            if (!animal.Equals(AnimalType.None))
            {
                SpawnAnimal(animal);
            }
        }
    }

    private AnimalType TrySpawnAnimal()
    {
        List<AnimalType> animalTypes = difficultyData.AnimalTypes;
        List<float> spawnChances = difficultyData.SpawnChances;
        float randomProbability = Random.value;

        // Loop through each animal and check if it should be spawned
        for (int i = 0; i < animalTypes.Count; i++)
        {
            AnimalType animal = animalTypes[i];
            Debug.Log("Animal: " + animal);
            float spawnChance = spawnChances[i];

            // Add additional chance of previous animal for handling lower chances than first
            if (i > 0)
            {
                spawnChance += spawnChances[i - 1];
            }

            if (spawnChance != 0.0f)
            {
                if (randomProbability <= spawnChance)
                {
                    return animal;
                }
            }
        }
        return AnimalType.None;
    }

    private void SpawnAnimal(AnimalType animal)
    {
        if (spawnedAnimalsCount >= maxAnimals) return;

        GameObject animalPrefab = (animal) switch
        {
            (AnimalType.Cow) => cowPrefab,
            (AnimalType.Pig) => pigPrefab,
            (AnimalType.Chicken) => chickenPrefab,
            _ => null
        };

        if (animalPrefab == null) return;

        // Spawn animal at random position in spawn area
        Vector3 spawnPosition = ComputeRandomSpawnPosition();
        Instantiate(animalPrefab, spawnPosition, Quaternion.identity);
        spawnedAnimalsCount++;
    }

    private Vector3 ComputeRandomSpawnPosition()
    {
        Vector2 size = animalSpawnArea.size;

        float x = Random.Range(0, size.x / 2f) + animalSpawnArea.offset.x;
        float y = Random.Range(0, size.y / 2f) + animalSpawnArea.offset.y;

        return new Vector3(x, y, 0);
    }
}
