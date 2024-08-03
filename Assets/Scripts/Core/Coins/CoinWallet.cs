using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

    public bool CanSpendCoins(int cost)
    {
        if (TotalCoins.Value < cost) return false;

        return true;
    }

    public void SpendCoins(int cost)
    {
        Debug.Log("spending coins..");
        TotalCoins.Value -= cost;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent<Coin>(out var coin)) return;

        int coinValue = coin.Collect();

        if (!IsServer) return;

        TotalCoins.Value += coinValue;
    }
}
