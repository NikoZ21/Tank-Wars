using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private void OnDestroy()
    {
        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        Instantiate(prefab, transform.position, rotation);
    }
}
