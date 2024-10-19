using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public float damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player Health = other.GetComponent<Player>();
            if (Health != null)
            {
                Health.health -= damage;
            }
        }
    }
}
