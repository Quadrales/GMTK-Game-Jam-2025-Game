using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    protected float baseMoveSpeed;
    protected float moveRadius;
    protected int goldValue;
    protected Rigidbody2D rb;

    private Vector2 nextPosition;
    private Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        ComputeNextPosition();
    }

    public virtual void Move()
    {
        Vector3 distanceToNextPoint = new Vector3(nextPosition.x, nextPosition.y, 0) - transform.position;
        Vector3 direction = distanceToNextPoint.normalized;

        if (distanceToNextPoint.magnitude >= 0.1f)
        {
            transform.position += direction * baseMoveSpeed * Time.deltaTime;
        }
        else
        {
            if (coroutine == null) // Make sure the coroutine isn't called every frame
            {
                coroutine = StartCoroutine(WaitAtPoint());
            }
        }
    }

    IEnumerator WaitAtPoint()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 4.0f));
        ComputeNextPosition();
        coroutine = null;
    }

    private void ComputeNextPosition()
    {
        nextPosition = Random.insideUnitCircle * moveRadius;
    }

    protected abstract void MakeSound();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Object collision occurred");
        ComputeNextPosition();
    }
}
