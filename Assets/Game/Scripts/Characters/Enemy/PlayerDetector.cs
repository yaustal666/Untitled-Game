using System;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public event Action<Transform> PlayerDetected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerDetected?.Invoke(collision.gameObject.transform);
        }
    }
}