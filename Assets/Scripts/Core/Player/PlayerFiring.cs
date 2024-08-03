using Unity.Netcode;
using UnityEngine;

public class PlayerFiring : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject serverProjectile;
    [SerializeField] private GameObject clientProjectile;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Collider2D playerBody;
    [SerializeField] private CoinWallet wallet;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float muzzleFlashDuration;
    [SerializeField] private float fireRate;
    [SerializeField] private int costToFire;

    private float muzzleFlashTimer;
    private float timer;
    private bool shouldFire;

    public override void OnNetworkSpawn()
    {
        inputReader.PrimaryFireEvent += HandlePrimaryFire;
    }
    public override void OnNetworkDespawn()
    {
        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }

    private void Update()
    {
        if (muzzleFlashTimer > 0f)
        {
            muzzleFlashTimer -= Time.deltaTime;

            if (muzzleFlashTimer <= 0)
            {
                muzzleFlash.SetActive(false);
            }
        }

        if (!IsOwner) return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (!shouldFire) return;

        if (timer > 0) return;


        if (!wallet.CanSpendCoins(costToFire)) return;

        PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);

        SpawnDummyProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);
        
        timer = 1 / fireRate;
    }

    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (!wallet.CanSpendCoins(costToFire)) return;

        wallet.SpendCoins(costToFire);

        GameObject projectile = Instantiate(
            serverProjectile,
            spawnPos,
            Quaternion.identity);

        projectile.transform.up = direction;

        if (projectile.TryGetComponent<DealDamageOnContact>(out var dealDamageOnContact))
        {
            dealDamageOnContact.SetOwner(OwnerClientId);
        }

        MoveProjectile(projectile);

        SpawnDummyProjectileClientRpc(spawnPos, direction);
    }

    [ClientRpc]
    private void SpawnDummyProjectileClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (IsOwner) return;

        SpawnDummyProjectile(spawnPos, direction);
    }

    private void SpawnDummyProjectile(Vector3 spawnPos, Vector3 direction)
    {
        muzzleFlash.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;

        GameObject projectile = Instantiate(
            clientProjectile,
            spawnPos,
            Quaternion.identity);

        projectile.transform.up = direction;

        MoveProjectile(projectile);
    }

    private void MoveProjectile(GameObject projectile)
    {
        Physics2D.IgnoreCollision(playerBody, projectile.GetComponent<Collider2D>());

        if (projectile.TryGetComponent(out Rigidbody2D rb))
        {
            rb.velocity = projectile.transform.up * projectileSpeed;
        }
    }

    private void HandlePrimaryFire(bool shouldFire)
    {
        this.shouldFire = shouldFire;
    }
}
