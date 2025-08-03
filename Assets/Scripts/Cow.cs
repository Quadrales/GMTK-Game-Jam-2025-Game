using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : Animal
{
    private void Awake()
    {
        baseMoveSpeed = 1.2f;
        goldValue = 3;
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
