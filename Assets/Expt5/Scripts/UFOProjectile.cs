using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class UFOProjectile : MonoBehaviour
{
    [SerializeField] float velocity;
    Rigidbody2D myRb2D;

    private void Awake()
    {
        myRb2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        myRb2D.velocity = transform.right * velocity;
        Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
