using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Animal
{
    private void Awake()
    {
        baseMoveSpeed = 6.5f;
        goldValue = 8;
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ComputeNextPosition();
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
