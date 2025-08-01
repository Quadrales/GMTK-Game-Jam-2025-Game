using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private DifficultyData difficultyData;

    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;
    private Vector3 player1StartPos = new Vector3(8, 3, 0);
    private Vector3 player2StartPos = new Vector3(24, 3, 0);

    [SerializeField] private GameObject cowPrefab;
    [SerializeField] private GameObject pigPrefab;

    [SerializeField] private BoxCollider2D animalSpawnArea;
    private Coroutine coroutine;

    private int maxAnimals = 25; // Use later to keep animals from spawning if there are too many on screen
    private int spawnedAnimalsCount = 0;

    public void SetDifficultyData(DifficultyData data)
    {
        difficultyData = data;
    }

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(player1Prefab, player1StartPos, Quaternion.identity);
        Instantiate(player2Prefab, player2StartPos, Quaternion.identity);

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

        Debug.Log("Spawning animal...");
        GameObject animalPrefab = (animal) switch
        {
            (AnimalType.Cow) => cowPrefab,
            (AnimalType.Pig) => pigPrefab,
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

        float x = Random.Range(-size.x / 2f, size.x / 2f);
        float y = Random.Range(-size.y / 2f, size.y / 2f);

        return new Vector3(x, y, 0);
    }
}
