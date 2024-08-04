using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] //adjustable elements
    private float healthDamage = 25f;    //slider might be adjusted to 0 here. Already Hardcoded 25 dmg into playercontroller script.
    [SerializeField]
    private float detectionRange = 10f;
    [SerializeField]
    private float chaseSpeed = 3.5f;
    [SerializeField]
    private GameObject coinPrefab; // refrence to coin prefab
    [SerializeField]
    private GameObject candyPrefab; //reference to the candy prefab

    [SerializeField]
    private GameObject batteryPrefab;

    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    private EnemySpawner enemySpawner;

    private void Start()
    {
        
        navMeshAgent = GetComponent<NavMeshAgent>(); //NavMeshAgent component
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing from the enemy object.");
            return;
        }

        navMeshAgent.speed = chaseSpeed;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player GameObject not found. Ensure it is tagged as 'Player'.");
        }

        
        enemySpawner = FindObjectOfType<EnemySpawner>();  //enemy spawner 
    }

    private void Update()
    {
        if (playerTransform == null || navMeshAgent == null || !navMeshAgent.isOnNavMesh)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer(); 
        }
        else
        {
            navMeshAgent.ResetPath(); //wip
        }
    }

    private void ChasePlayer()
    {
        if (navMeshAgent.isActiveAndEnabled && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.SetDestination(playerTransform.position);
        }
    }

    public void DestroyAndSpawnCoin()
    {
        if (coinPrefab != null)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.Euler(0, 0, 90));
        }

        if (enemySpawner != null)
        {
            enemySpawner.EnemyDestroyed();
        }

        Destroy(gameObject);
    }



    public void DestroyAndSpawnCandy()
    {
        if (candyPrefab != null)
        {
            Instantiate(candyPrefab, transform.position, Quaternion.Euler(0, 0, 90));
        }

        if (enemySpawner != null)
        {
            enemySpawner.EnemyDestroyed();
        }

        Destroy(gameObject);
    }

    public void DestroyAndSpawnBattery()
    {
        if (batteryPrefab != null)
        {
            Instantiate(batteryPrefab, transform.position, Quaternion.Euler(0, 0, 90));
        }

        if (enemySpawner != null)
        {
            enemySpawner.EnemyDestroyed();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) //collision events
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.RemovePlayerHealth(healthDamage);
                DestroyAndSpawnCoin();
            }
        }
        if (other.CompareTag("Walls")){
            DestroyImmediate(gameObject);
         
         }
    }
}
