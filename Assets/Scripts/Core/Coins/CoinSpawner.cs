using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private RespawningCoin respawningCoin;

    [Header("Settings")]
    [SerializeField] private int maxCoins = 50;
    [SerializeField] private int coinValue = 10;

    [SerializeField] private Vector2 xSpawnRange;
    [SerializeField] private Vector2 ySpawnRange;

    [SerializeField] private LayerMask layersMask;

    private Collider2D[] coinbuffer = new Collider2D[1];
    private float coinRadius;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        coinRadius = respawningCoin.GetComponent<CircleCollider2D>().radius;

        for (int i = 0; i < maxCoins; i++)
        {
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        RespawningCoin coinInstance = Instantiate(
            respawningCoin,
            GetSpawnPoint(),
            Quaternion.identity
            );

        coinInstance.SetValue(coinValue);
        coinInstance.GetComponent<NetworkObject>().Spawn();

        coinInstance.OnCollected += HandleCoinCollected;
    }

    private void HandleCoinCollected(RespawningCoin coin)
    {
        coin.transform.position = GetSpawnPoint();
        coin.Reset();
    }

    private Vector2 GetSpawnPoint()
    {
        float x = 0;
        float y = 0;

        while (true)
        {
            x = Random.Range(xSpawnRange.x, xSpawnRange.y);
            y = Random.Range(ySpawnRange.x, ySpawnRange.y);
            Vector2 spawnPoint = new Vector2(x, y);
            int numColliders = Physics2D.OverlapCircleNonAlloc(spawnPoint, coinRadius, coinbuffer, layersMask);

            if (numColliders == 0)
            {
                return spawnPoint;
            }
        }
    }
}
