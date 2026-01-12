using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyLevel {
    DEFAULT,
    FIRST,
    SECOND,
    THIRD
}

public class Enemy : MonoBehaviour {

    [SerializeField] private LayerMask PlayerLayerMask;
    [SerializeField] private EnemyLevel enemyLevel;
    [SerializeField] private float followSpeed = 3.5f;
    [SerializeField] private float stoppingDistance = 2f;

    [SerializeField] private GameObject explosionParticle;

    private Transform player;
    private NavMeshAgent agent;
    [SerializeField] private int health;
    [SerializeField] private int damage;
    private int bloodDrops = 0;
    private int score;
    private int stamina;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = followSpeed;
        agent.stoppingDistance = stoppingDistance;
    }

    void Start() {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) {
            player = playerObject.transform;
        }
    }

    void Update() {
        if (player != null) {
            FollowPlayer();
            RotateTowardsPlayer();
            HandleInteraction();
            Death();
        }
    }

    void FollowPlayer() {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        agent.speed = Mathf.Lerp(0, followSpeed, distanceToPlayer / stoppingDistance);
        agent.SetDestination(player.position);
    }

    void RotateTowardsPlayer() {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void HandleInteraction() {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 moveDir = new Vector3(direction.x, 0, direction.z);

        float interactDistance = 0.9f;
        if (Physics.Raycast(transform.position, moveDir, out RaycastHit raycastHit, interactDistance, PlayerLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out Player player)) {
                if (player.HasGiftObject()) {
                    player.GetGiftObject().DestroySelf();
                    player.ClearGiftObject();
                }
                if (enemyLevel == EnemyLevel.FIRST) {
                    health = 0;
                } else if (enemyLevel == EnemyLevel.SECOND) {
                    player.Damage(damage);
                    health = 0;
                } else if (enemyLevel == EnemyLevel.THIRD) {
                    player.Damage(damage);
                    health = 0;
                } else {
                    Debug.Log("Sito neturetum matyt");
                }
            }
        }
    }

    public void ApplySmoothPush(Vector3 sourcePosition, float pushRadius, float pushDuration) {
        if (agent != null) {
            Vector3 pushDirection = (transform.position - sourcePosition).normalized;
            Vector3 targetPosition = sourcePosition + pushDirection * pushRadius;

            // Maintain original Y position
            targetPosition.y = transform.position.y;

            StartCoroutine(SmoothPushCoroutine(targetPosition, pushDuration));
        }
    }

    private IEnumerator SmoothPushCoroutine(Vector3 targetPosition, float duration) {
        agent.isStopped = true;

        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;

        while (elapsedTime < duration) {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        agent.isStopped = false;
    }


    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0) {
            bloodDrops = Random.Range(1, 4);
            score = Random.Range(10, 20);
            stamina = Random.Range(1, 5);
        }
    }

    private void Death() {
        if (health <= 0) {
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public int GetBloodDrops() {
        return bloodDrops;
    }

    public int GetScore() {
        return score;
    }

    public int GetStamina() {
        return stamina;
    }
}
