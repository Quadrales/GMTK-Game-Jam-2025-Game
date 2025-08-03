using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoArea : MonoBehaviour
{
    private Player owner;

    public void SetOwner(Player player)
    {
        owner = player;
    }

    public Player GetOwner() {
        return owner;
    }
}
