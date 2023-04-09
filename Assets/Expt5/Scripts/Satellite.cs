using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoodUtils;

[RequireComponent(typeof(Rigidbody2D))]
public class Satellite : MonoBehaviour
{
    [SerializeField] GameObject[] debrisObjects;
    [SerializeField] float maxDebrisCount, minDebrisCount;
    [SerializeField] float explosionForce = 10f;
    [SerializeField] float explosionRadius = 3f;
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] ParticleSystem explosionParticles;

    bool isBreaking = false;
    Vector3 lastKnownVelocity;

    Rigidbody2D myRb2D;

    private void Awake()
    {
        myRb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        lastKnownVelocity = myRb2D.velocity;
    }

    private void FixedUpdate()
    {
        lastKnownVelocity = myRb2D.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Planet"))
        {
            if (!isBreaking)    
            {
                isBreaking = true;
                //Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + collision.relativeVelocity, Color.red, 2f);
                float derbrisCount = Random.Range(minDebrisCount, maxDebrisCount);
                float debrisSpawnAngle = 360 / derbrisCount;
                for (int i = 0; i < derbrisCount; i++)
                {

                    GameObject debri = Instantiate(
                        debrisObjects[Random.Range(0, debrisObjects.Length)],
                        (Vector3)collision.contacts[0].point + Quaternion.Euler(0, 0, debrisSpawnAngle * i) * transform.up * 0.15f,
                        Quaternion.identity
                    );

                    Rigidbody2D debriRb = debri.GetComponent<Rigidbody2D>();

                    debriRb.velocity = lastKnownVelocity;
                    debriRb.AddExplosionForce(explosionForce, collision.contacts[0].point, explosionRadius, 0, ForceMode2D.Impulse);
                    debriRb.angularVelocity = 90f + Random.Range(-360f, 360f) * 4f;
                }

                float angle = Mathf.Atan2(lastKnownVelocity.y, lastKnownVelocity.x) * Mathf.Rad2Deg;
                Instantiate(explosionParticles, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));

                Instantiate(pickupPrefab, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }   
        else
        {
            collision.rigidbody.GetComponent<Health>().TakeDamage(5);
            Destroy(this.gameObject);
        }
    }
}
