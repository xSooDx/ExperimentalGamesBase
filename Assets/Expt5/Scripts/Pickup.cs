using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float rotationSpeed = 30f;

    private void Update()
    {
        transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.attachedRigidbody.CompareTag("Player"))
        {
            UFOGameManager.Instance.AddToScore(1);
            Destroy(this.gameObject);
        }
    }
}
