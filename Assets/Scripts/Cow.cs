using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : Animal
{
    private void Awake()
    {
        baseMoveSpeed = 1.0f;
        moveRadius = 4.0f;
        goldValue = 3;
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
