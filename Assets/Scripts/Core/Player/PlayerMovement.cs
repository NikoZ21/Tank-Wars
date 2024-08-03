using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rb;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private float turningRate = 30;

    private Vector2 previousMovement;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        inputReader.MoveEvent += HandleMove;
    }

    private void Update()
    {
        if (!IsOwner) return;

        float zRotation = previousMovement.x * -turningRate * Time.deltaTime;
        bodyTransform.Rotate(0f, 0f, zRotation);
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        rb.velocity = (Vector2)bodyTransform.up * previousMovement.y * moveSpeed;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        inputReader.MoveEvent += HandleMove;
    }

    private void HandleMove(Vector2 movement)
    {
        previousMovement = movement;
    }
}
