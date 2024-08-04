using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab; //enemy prefab
    [SerializeField]
    private float spawnRadius = 20f; // spawn radius
    [SerializeField]
    private float spawnInterval = 10f; // Time between spawns
    [SerializeField]
    private int maxEnemies = 10; // max num of enemeis in lvl
    [SerializeField]
    private float navMeshSampleDistance = 2f; // navmesh sample distance for 1f or 2f spawn

    private Transform playerTransform;
    private int enemyCount = 0;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

       
        InvokeRepeating("SpawnEnemy", spawnInterval, spawnInterval); //spawn
    }

    private void SpawnEnemy()
    {
        if (playerTransform == null || enemyPrefab == null || enemyCount >= maxEnemies)
        {
            return;
        }

       
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius; //random spawn position
        randomDirection += playerTransform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, navMeshSampleDistance, NavMesh.AllAreas))
        {
           
            Instantiate(enemyPrefab, hit.position, Quaternion.identity);  // spawn enemy at sampled location
            enemyCount++;
        }
    
    }

    public void EnemyDestroyed()
    {
    
        enemyCount--;
    }
}
