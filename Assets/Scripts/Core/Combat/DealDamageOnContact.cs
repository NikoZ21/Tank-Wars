using Unity.Netcode;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private ulong ownerClientId;

    public void SetOwner(ulong ownerClientId)
    {
        this.ownerClientId = ownerClientId;
    }

    private void OnTriggerEnter2D(Collider2D colObj)
    {
        Debug.Log("hit something");

        if (colObj.attachedRigidbody == null) return;

        if (!colObj.TryGetComponent<NetworkObject>(out var networkObject)) return;

        if (ownerClientId == networkObject.OwnerClientId) return;

        if (!colObj.TryGetComponent<Health>(out var health)) return;

        Debug.Log("dealing damage");

        health.TakeDamage(damage);
    }
}
