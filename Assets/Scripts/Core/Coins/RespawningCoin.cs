using System;
using UnityEngine;
public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> OnCollected;

    private Vector3 prevPos;

    private void Update()
    {
        if (prevPos != transform.position)
        {
            Show(true);
        }

        prevPos = transform.position;
    }

    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }

        if (alreadyCollected) return 0;

        alreadyCollected = true;

        OnCollected?.Invoke(this);

        return coinValue;
    }

    public void Reset()
    {
        alreadyCollected = false;
    }
}
