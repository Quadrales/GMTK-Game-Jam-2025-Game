using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;

    private DifficultyData difficultyData;

    private Vector3 player1StartPos = new Vector3(-8, -3, 0);
    private Vector3 player2StartPos = new Vector3(8, 3, 0);


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting game...");
        Instantiate(player1Prefab, player1StartPos, Quaternion.identity);
        Instantiate(player2Prefab, player2StartPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDifficultyData(DifficultyData data)
    {
        difficultyData = data;
    }
}
