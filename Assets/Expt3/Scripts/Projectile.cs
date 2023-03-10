using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float velocity;
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] float explosionForce = 100f;
    [SerializeField] float maxDamage = 25f;
    [SerializeField] ParticleSystem explosionParticles;
    Rigidbody m_rigidbody;

    bool isQuitting = false;
    // Start is called before the first frame update
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        m_rigidbody.velocity = transform.forward * velocity;
        Destroy(this.gameObject, 5f);
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (!isQuitting)
        {
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            // ToDo: Play particle effects
            foreach (Collider c in colliders)
            {
                if (c.attachedRigidbody)
                {
                    Health hc = c.attachedRigidbody.GetComponent<Health>();
                    c.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    if (hc)
                    {
                        hc.TakeDamage(maxDamage * (1f - ((c.ClosestPoint(transform.position) - transform.position).magnitude / explosionRadius)));
                    }
                }
                else
                {
                    Health hc = c.transform.GetComponent<Health>();
                    if (hc)
                    {
                        hc.TakeDamage(maxDamage * (1f - ((c.ClosestPoint(transform.position) - transform.position).magnitude / explosionRadius)));
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }
}
