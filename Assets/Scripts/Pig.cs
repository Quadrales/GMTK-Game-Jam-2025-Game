using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : Animal
{
    private void Awake()
    {
        baseMoveSpeed = 2.0f;
        moveRadius = 6.0f;
        goldValue = 5;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected override void MakeSound()
    {
    }
}
