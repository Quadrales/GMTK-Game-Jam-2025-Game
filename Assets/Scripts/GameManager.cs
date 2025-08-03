using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameManager : MonoBehaviour
{
    private DifficultyData difficultyData;

    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;
    private Vector3 player1StartPos = new Vector3(8, 8, 0);
    private Vector3 player2StartPos = new Vector3(20, 8, 0);

    [SerializeField] private GameObject cowPrefab;
    [SerializeField] private GameObject pigPrefab;
    [SerializeField] private GameObject chickenPrefab;

    [SerializeField] private BoxCollider2D animalSpawnArea;
    private Coroutine coroutine;

    private int maxAnimals = 25;
    private int spawnedAnimalsCount = 0;

    public void SetDifficultyData(DifficultyData data)
    {
        difficultyData = data;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject player1 = Instantiate(player1Prefab, player1StartPos, Quaternion.identity);
        GameObject player2 = Instantiate(player2Prefab, player2StartPos, Quaternion.identity);

        AssignPlayerInputs(player1, player2);

        coroutine = StartCoroutine(SpawnAnimalsCoroutine());
    }

    private void AssignPlayerInputs(GameObject player1, GameObject player2)
    {
        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;

        PlayerInput p1Input = player1.GetComponent<PlayerInput>();
        PlayerInput p2Input = player2.GetComponent<PlayerInput>();

        // Assign devices manually so both players can use same device
        // Player1 - WASD
        InputUser.PerformPairingWithDevice(keyboard, p1Input.user);
        InputUser.PerformPairingWithDevice(mouse, p1Input.user);
        p1Input.SwitchCurrentControlScheme("Keyboard&Mouse", keyboard, mouse);

        // Player2 - Arrow Keys
        InputUser.PerformPairingWithDevice(keyboard, p2Input.user);
        InputUser.PerformPairingWithDevice(mouse, p2Input.user);
        p2Input.SwitchCurrentControlScheme("Keyboard&Mouse", keyboard, mouse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnAnimalsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 5f));

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
        float prevSpawnChance = spawnChances[0];

        // Loop through each animal and check if it should be spawned
        for (int i = 0; i < animalTypes.Count; i++)
        {
            AnimalType animal = animalTypes[i];
            float spawnChance = spawnChances[i];

            // Add additional chance of previous animal for handling lower chances than first
            if (i > 0)
            {
                spawnChance += prevSpawnChance;
            }

            // Spawn animal based on random probability
            if (spawnChance != 0.0f)
            {
                if (randomProbability <= spawnChance)
                {
                    return animal;
                }
            }

            // Set previous spawn chance for next spawn attempt
            prevSpawnChance = spawnChance;
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
