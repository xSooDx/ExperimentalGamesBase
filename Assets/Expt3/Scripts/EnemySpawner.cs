using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static int spawnCount = 0;
    public static int maxSpawnCount = 32;
    public Vector3 size;

    public GameObject enemyPrefab;

    public float maxSpawnInterval = 8f;
    public float minSpawnInterval = 4f;
    public LayerMask floorMask;
    public LayerMask playerMask;
    public bool canSpawn = true;

    IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            if (!canSpawn) continue;

            Vector3 spawnPoint = new Vector3(Random.Range(-size.x, size.x), size.y, Random.Range(-size.z, size.z)) / 2;
            Debug.DrawRay(transform.position + spawnPoint, Vector3.down, Color.red, 5f);
            if (Physics.Raycast(transform.position + spawnPoint, Vector3.down, out RaycastHit hit, size.y, floorMask))
            {                
                Instantiate(enemyPrefab, hit.point, Quaternion.identity);
                spawnCount++;
            }
            else
            {
                Debug.Log("WTF");
            }
        }
    }

    void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, size / 2, Quaternion.identity, playerMask);
        canSpawn = hits.Length == 0 && spawnCount < maxSpawnCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawner());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, size);
    }
}
