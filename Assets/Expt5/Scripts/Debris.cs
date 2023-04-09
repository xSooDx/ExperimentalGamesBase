using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    [SerializeField] float minSize = 0.75f, maxSize = 1.25f;
    [SerializeField] Collider2D myCollider;
    private void Awake()
    {
        transform.localScale = Vector3.one * Random.Range(minSize, maxSize);
        gameObject.layer = 31;
        StartCoroutine(EnableCollision());
    }

    IEnumerator EnableCollision()
    {
        yield return new WaitForSeconds(0.25f);
        gameObject.layer = LayerMask.NameToLayer("Debris");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.rigidbody.GetComponent<Health>().TakeDamage(1);
        }
        else if (!collision.gameObject.CompareTag("Debris"))
        {
            Destroy(gameObject);
        }
    }
}
    