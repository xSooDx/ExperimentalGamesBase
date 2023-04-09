using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteSpawner : MonoBehaviour
{
    [SerializeField] GravitySource gravitySource;
    [SerializeField] float minSpawnHeight;
    [SerializeField] float maxSpawnHeight;
    [SerializeField] GameObject satellitePrefab;
    [SerializeField] float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnerCorutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnerCorutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            Spawn();
        }
    }

    void Spawn()
    {
        Vector3 spawnPosition = Random.insideUnitCircle.normalized * Random.Range(minSpawnHeight, maxSpawnHeight);
        GameObject satellite = Instantiate(satellitePrefab, spawnPosition, Quaternion.identity);
        satellite.GetComponent<Rigidbody2D>().velocity = gravitySource.GetOrbitalVel(spawnPosition, Random.Range(0, 2) == 0 ? OrbitalDirection.ClockWise : OrbitalDirection.CounterClockWise);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minSpawnHeight);
        Gizmos.DrawWireSphere(transform.position, maxSpawnHeight);
    }
}
