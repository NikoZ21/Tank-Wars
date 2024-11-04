using Unity.Netcode;
using UnityEngine;

public abstract class Coin : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    protected int coinValue = 10;

    protected bool alreadyCollected;

    public abstract int Collect();
    
    public void SetValue(int value)
    {
        this.coinValue = value;
    }

    protected void Show(bool isEnabled)
    {
        spriteRenderer.enabled = isEnabled;
    }
}
