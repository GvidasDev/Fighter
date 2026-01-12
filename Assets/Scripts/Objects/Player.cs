using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour, IGiftObjectParent {
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedGiftParentChangedEventArgs> OnSelectedGiftParentChanged;

    public class OnSelectedGiftParentChangedEventArgs : EventArgs {
        public BaseGiftParentObject selectedParent;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform giftObjectHoldPoint;
    [SerializeField] private LayerMask ContainerLayerMask;
    [SerializeField] private int health = 100;
    [SerializeField] private GameObject explosionParticle;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip damageSoundClip;
    [SerializeField] private AudioClip attackSoundClip;
    [SerializeField] private AudioClip powerUpSoundClip;

    private bool isWalking;
    private Vector3 lastInteractionDir;
    private BaseGiftParentObject selectedParent;
    private GiftObject giftObject;

    [SerializeField]private int score;
    [SerializeField] private int stamina;
    [SerializeField]private int blood;
    private int highScore;
    [SerializeField] private PlayerAbilitiesSO abilitiesSO;

    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject[] trail;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Multiple Player instances detected!");
            return;
        }
        Instance = this;
    }

    private void Start() {
       // GameManager.Instance.IsPlayerDead = false;

        LoadPlayerData();
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractActivate += GameInput_OnInteractActivate;
    }

    private void OnApplicationQuit() {
        SavePlayerData();
    }

    private void OnDestroy() {
        SavePlayerData();
    }

    private void GameInput_OnInteractActivate(object sender, EventArgs e) {
        ActivateAbilitie();
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (selectedParent != null) {
            selectedParent.Interact(this);
        }
    }

    private void Update() {
        Death();
        HandleMovement();
        HandleInteraction();
        HandleEnemyKill();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleEnemyKill() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            float effectRadius = 5f;
            float effectAngle = 70f;
            int damageAmount = Random.Range(25, 35);
            float pushForce = 4f;
            float pushDuration = 0.2f;

            if (audioSource != null && damageSoundClip != null) {
                audioSource.PlayOneShot(attackSoundClip, 0.3f);
            }

            Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, effectRadius, LayerMask.GetMask("Enemy"));
            foreach (var enemyCollider in enemiesInRange) {
                Vector3 directionToEnemy = (enemyCollider.transform.position - transform.position).normalized;

                float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);
                if (angleToEnemy <= effectAngle) {
                    Enemy enemy = enemyCollider.GetComponent<Enemy>();
                    if (enemy != null) {
                        enemy.Damage(damageAmount);
                        blood += enemy.GetComponent<Enemy>().GetBloodDrops();
                        score += enemy.GetComponent<Enemy>().GetScore();
                        stamina += enemy.GetComponent<Enemy>().GetStamina();
                        enemy.ApplySmoothPush(transform.position, pushForce, pushDuration);
                    }
                }
            }
        }
    }

    private void HandleInteraction() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractionDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit raycastHit, interactDistance, ContainerLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out BaseGiftParentObject baseCounter)) {
                if (baseCounter != selectedParent) {
                    SetSelectedParent(baseCounter);
                }
            } else {
                SetSelectedParent(null);
            }
        } else {
            SetSelectedParent(null);
        }
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * 2f, 0.5f, moveDir, moveDistance);

        if (!canMove) {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * 2f, 0.5f, moveDirX, moveDistance);
            if (canMove) {
                moveDir = moveDirX;
            } else {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * 2f, 0.5f, moveDirZ, moveDistance);
                if (canMove) {
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void ActivateAbilitie() {
        if (abilitiesSO == null) {
            Debug.LogError("abilitiesSO is not assigned!");
            return;
        }
        if (stamina >= 100 && abilitiesSO.Abilitie != Abilities.None) {
            if (audioSource != null && damageSoundClip != null) {
                audioSource.PlayOneShot(damageSoundClip, 0.3f);
            }
            switch (abilitiesSO.Abilitie) {
                case Abilities.Shield:
                    ActivateShieldAbility();
                    break;
                case Abilities.Immune:
                    ActivateImmunityAbility();
                    break;
                case Abilities.Speed:
                    ActivateSpeedAbility();
                    break;
                default:
                    Debug.Log("No ability selected.");
                    break;
            }
            //stamina = 0;
        } else {
            Debug.Log("Not enough stamina!");
        }
    }

    private void ActivateShieldAbility() {
        Debug.Log("Shield activated!");
        shield.SetActive(true);
        StartCoroutine(ShieldEffectCoroutine(7f));
    }

    private IEnumerator ShieldEffectCoroutine(float duration) {
        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            PushEnemiesGently();
            elapsedTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        shield.SetActive(false);
        Debug.Log("Shield deactivated!");
    }

    private void PushEnemiesGently() {
        Collider[] enemies = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("Enemy"));
        foreach (var enemyCollider in enemies) {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null) {
                Vector3 pushDirection = (enemy.transform.position - transform.position).normalized;
                pushDirection.y = 0f;
                enemy.ApplySmoothPush(transform.position, 5f, 0.3f);
            }
        }
    }

    private void ActivateImmunityAbility() {
        Debug.Log("Immunity activated!");
        StartCoroutine(TemporaryImmunity(7f));
    }

    private IEnumerator TemporaryImmunity(float duration) {
        int originalHealth = health;
        float endTime = Time.time + duration;
        while (Time.time < endTime) {
            health = originalHealth;
            yield return null;
        }
        Debug.Log("Immunity deactivated!");
    }

    private void ActivateSpeedAbility() {
        Debug.Log("Speed boost activated!");
        float originalSpeed = moveSpeed;
        moveSpeed *= 1.5f;
        foreach (GameObject a in trail) {
            a.SetActive(true);
        }
        StartCoroutine(ResetSpeedAfterTime(originalSpeed, 10f));
    }

    private IEnumerator ResetSpeedAfterTime(float originalSpeed, float duration) {
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
        Debug.Log("Speed boost deactivated!");
        foreach(GameObject a in trail) {
            a.SetActive(false);
        }
    }

    private void SetSelectedParent(BaseGiftParentObject selectedParent) {
        this.selectedParent = selectedParent;

        OnSelectedGiftParentChanged?.Invoke(this, new OnSelectedGiftParentChangedEventArgs {
            selectedParent = this.selectedParent
        });
    }

    public Transform GetGiftObjectFollowTransform() {
        return giftObjectHoldPoint;
    }

    public void SetGiftObject(GiftObject giftObject) {
        this.giftObject = giftObject;
    }

    public GiftObject GetGiftObject() {
        return giftObject;
    }

    public void ClearGiftObject() {
        giftObject = null;
    }

    public bool HasGiftObject() {
        return giftObject != null;
    }

    public void Damage(int damage) {
        health -= damage;
        if (audioSource != null && damageSoundClip != null) {
            audioSource.PlayOneShot(damageSoundClip, 0.3f);
        }
    }

    public int GetHealth() {
        return health;
    }

    public void Death() {
        if (health <= 0) {
            SavePlayerData();
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            GameManager.Instance.IsPlayerDead = true;
        }
    }

    public int GetScore() {
        return score;
    }

    public void AddScore(int score) {
        this.score += score;
    }

    public int GetStamina() {
        return stamina;
    }

    public int GetBlood() {
        return blood;
    }

    public void ReduceBlood(int amount) {
        blood -= amount;
        Debug.Log($"Blood reduced by {amount}. Remaining blood: {blood}");
    }

    public PlayerAbilitiesSO GetPlayerAbilitiesSO() {
        return abilitiesSO;
    }

    public int GetHighScore() {
        return highScore;
    }

    public void SavePlayerData() {
        if(highScore < score) highScore = score;
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("Blood", blood);
        PlayerPrefs.Save();
        Debug.Log("Player data saved.");
    }

    public void LoadPlayerData() {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        blood = PlayerPrefs.GetInt("Blood", 150);
        Debug.Log($"Player data loaded. High Score: {highScore}, Blood: {blood}");
    }
}
