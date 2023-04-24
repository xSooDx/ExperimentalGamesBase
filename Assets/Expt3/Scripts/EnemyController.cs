using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject m_turret;
    [SerializeField] GameObject m_projectilePrefab;
    [SerializeField] Transform m_projectileSpawn;
    [SerializeField] float m_timeToFire = 1f;

    float fireTimer;

    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        fireTimer = Random.Range(0f, m_timeToFire);
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        m_turret.transform.LookAt(target);
        if (Physics.Raycast(m_projectileSpawn.position, m_projectileSpawn.forward, out RaycastHit hit))
        {
            fireTimer -= Time.fixedDeltaTime;
            if (hit.transform.CompareTag("Player"))
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if(fireTimer <= 0f)
        {
            Instantiate(m_projectilePrefab, m_projectileSpawn.position, m_projectileSpawn.rotation);
            fireTimer = m_timeToFire;
        }
        
    }
}
