using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private DifficultyData difficultyData;

    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;
    private Vector3 player1StartPos = new Vector3(8, 8, 0);
    private Vector3 player2StartPos = new Vector3(20, 8, 0);
    [SerializeField] private TextMeshProUGUI player1GoldText;
    [SerializeField] private TextMeshProUGUI player2GoldText;

    [SerializeField] private GameObject cowPrefab;
    [SerializeField] private GameObject pigPrefab;
    [SerializeField] private GameObject chickenPrefab;

    [SerializeField] private BoxCollider2D animalSpawnArea;
    [SerializeField] private LayerMask animalLayer;
    private Coroutine coroutine;

    private int maxAnimals = 30;
    private int spawnedAnimalsCount = 0;

    public void SetDifficultyData(DifficultyData data)
    {
        difficultyData = data;
    }

    public void TrySendAnimalsToBarn(Collider2D lassoCollider, Player player)
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(animalLayer);
        filter.useLayerMask = true;

        // List of animals inside the lasso
        List<Collider2D> hits = new List<Collider2D>();
        lassoCollider.OverlapCollider(filter, hits);

        if (hits.Count == 0)
        {
            return;
        }

        foreach (var hit in hits)
        {
            Animal animal = hit.GetComponent<Animal>();

            // Increment player gold
            if (player != null)
            {
                player.goldCount += animal.goldValue;
                UpdatePlayerGoldText(player.goldCount, player);
                spawnedAnimalsCount -= 1;
            }

            // Destroy animal object
            GameObject animalObject = animal.gameObject;
            Destroy(animalObject);
        }
    }

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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

    private void UpdatePlayerGoldText(int goldValue, Player player)
    {
        PlayerInput input = player.GetComponent<PlayerInput>();

        if (input.playerIndex == 0)
        {
            player1GoldText.SetText(goldValue.ToString());
        }
        else
        {
            player2GoldText.SetText(goldValue.ToString());
        }
    }

    private IEnumerator SpawnAnimalsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.2f, 3f));

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
