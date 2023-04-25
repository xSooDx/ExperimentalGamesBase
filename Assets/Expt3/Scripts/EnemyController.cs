using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject m_turret;
    [SerializeField] GameObject m_projectilePrefab;
    [SerializeField] Transform m_projectileSpawn;
    [SerializeField] float m_timeToFire = 1f;
    [SerializeField] float m_lookSpeed = 10f;

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
        if (target == null) return;
        m_turret.transform.rotation = Quaternion.Lerp(m_turret.transform.rotation, Quaternion.LookRotation((target.position - m_projectileSpawn.position).normalized), m_lookSpeed * Time.deltaTime);
        //m_turret.transform.LookAt(target);
        if (Physics.Raycast(m_projectileSpawn.position, m_projectileSpawn.forward, out RaycastHit hit))
        {
            fireTimer -= Time.deltaTime;
            if (hit.transform.CompareTag("Player"))
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (fireTimer <= 0f)
        {
            Instantiate(m_projectilePrefab, m_projectileSpawn.position, m_projectileSpawn.rotation);
            fireTimer = m_timeToFire;
        }
    }

    void OnDestroy()
    {
        EnemySpawner.spawnCount--;
    }
}
