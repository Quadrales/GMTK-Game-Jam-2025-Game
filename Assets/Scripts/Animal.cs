using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    protected float baseMoveSpeed;
    protected float moveRadius;
    protected int goldValue;

    private Vector2 nextPosition;
    private Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        nextPosition = Random.insideUnitCircle * moveRadius;
    }

    public virtual void Move()
    {
        Vector3 distanceToNextPoint = new Vector3(nextPosition.x, nextPosition.y, 0) - transform.position;
        Vector3 direction = distanceToNextPoint.normalized;
        Debug.Log(distanceToNextPoint);
        Debug.Log(direction);

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
        yield return new WaitForSeconds(Random.RandomRange(1.5f, 4.0f));
        nextPosition = Random.insideUnitCircle * moveRadius;
        coroutine = null;
    }

    protected abstract void MakeSound();
}
