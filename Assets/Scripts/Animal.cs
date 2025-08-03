using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    public int goldValue;
    protected float baseMoveSpeed;
    protected Animator animator;

    private Vector2 nextPosition;
    private int spawnOffsetX = 7;
    private int spawnOffsetY = 9;
    private int spawnSizeX = 17;
    private int spawnSizeY = 9;
    private Coroutine coroutine;

    public virtual void Move()
    {
        Vector3 distanceToNextPoint = new Vector3(nextPosition.x, nextPosition.y, 0) - transform.position;
        Vector3 direction = distanceToNextPoint.normalized;

        if (distanceToNextPoint.magnitude >= 0.1f)
        {
            transform.position += direction * baseMoveSpeed * Time.deltaTime;
            if (animator != null)
            {
                animator.SetFloat("DistanceAway", distanceToNextPoint.magnitude);
            }
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
        yield return new WaitForSeconds(Random.Range(0.5f, 2.2f));
        ComputeNextPosition();
        coroutine = null;
    }

    protected void ComputeNextPosition()
    {
        // I know this way of doing this is bad but this is the price to pay for using an abstract class
        // which cannot have serialized fields
        nextPosition = new Vector2(Random.Range(spawnOffsetX, spawnSizeX + spawnOffsetX),
                                   Random.Range(spawnOffsetY, spawnSizeY + spawnOffsetY));
    }

    protected abstract void MakeSound();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Stop the wait coroutine if currently active, to immediately readjust animal movement
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        ComputeNextPosition();
    }
}
