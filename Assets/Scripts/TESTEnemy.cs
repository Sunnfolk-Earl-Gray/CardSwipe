using System;
using UnityEngine;
using UnityEngine.Serialization;

public class TESTEnemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackDistance;
    [SerializeField] private float knockbackForce = 2;
    [SerializeField] private float knockbackTime;
    [SerializeField] private float stunTime;
    public GameObject scorePopupPrefab; 
    public GameObject hitParticles;
    [SerializeField] private Rigidbody2D rb;

    public int score = 50;
    public float weaponRange;
    public float attackCooldown = 2;
    public Transform attackPoint;
    public LayerMask playerLayer;
    public LayerMask dashLayer;
    public AudioClip hitSound;
    private AudioSource audioSource;
    private PlayerMovement _playerMovement;
    private int _facingDirection = -1;
    private float _attackCooldownTimer;
    public Transform player;
    private Transform _enemy;
    private EnemyState _enemyState;
    private Animator _anim;
    private ScoreAppear _scorePopup;
    
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ChangeState(_enemyState = EnemyState.Chasing);
        _playerMovement = player.GetComponent<PlayerMovement>();
        _enemy = gameObject.GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_enemyState == EnemyState.Chasing)
        {
            if (player.position.x > transform.position.x && _facingDirection == -1 ||
                player.position.x < transform.position.x && _facingDirection == 1)
            {
                Flip();
            }
        }
        
        if (_attackCooldownTimer > 0)
        {
            _attackCooldownTimer -= Time.deltaTime;
        }
    }
    
    private void FixedUpdate()
    {
        if (_enemyState == EnemyState.Chasing)
        {
           Chase();
        }

        if (_enemyState == EnemyState.Attack)
        {
            Attack();
        }
    }

    void Flip()
    {
        _facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    
    private void Chase()
    {
        var heading = target.position - transform.position;
        var distance = heading.magnitude;
        heading = heading / distance;
        if (distance >= attackDistance) rb.linearVelocity = heading * speed;
        else if (_attackCooldownTimer <= 0)
        {
            ChangeState(EnemyState.Attack);
        }
    }

    private void Attack()
    {
        rb.linearVelocity = new Vector3(0, 0, 0);
        _attackCooldownTimer = attackCooldown;
        Debug.Log("Attacking player now");
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);
        if (hits.Length > 0)
        { 
            PlayHitSound();
            if (PlayerMovement.IsInvul == false)
            {
                hits[0].GetComponent<PlayerManager>().ChangeHealth(-damage);
                hits[0].GetComponent<PlayerMovement>().Knockback(_enemy, knockbackForce, knockbackTime, stunTime);
                CameraShake.Instance.StartCoroutine(CameraShake.Instance.Shake(0.1f, 0.2f));
                HitStop.Instance?.Stop(0.05f);
            }
        }
        
        Collider2D[] parry = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, dashLayer);
        if (parry.Length > 0)
        {
            //send pts to level manager?
            Debug.Log("Parried now");
            Destroy(this.gameObject);
            _playerMovement.dashCooldownTimer = 0;
            GameObject popup = Instantiate(scorePopupPrefab, transform.position, Quaternion.identity);
            ScoreAppear scoreAppear = popup.GetComponent<ScoreAppear>();
            CameraShake.Instance.StartCoroutine(CameraShake.Instance.Shake(0.1f, 0.2f));
            HitStop.Instance?.Stop(0.03f);
            if (scoreAppear != null)
            {
                scoreAppear.Setup(score);
                LeaderboardScript.instance.currentScore++;
            }
            else
            {
                Debug.Log("Score Popup Null");
            }
        }
        ChangeState(EnemyState.Chasing);
    }

    void ChangeState(EnemyState  newState)
    {
        //exit current animation
        /*if (_enemyState == EnemyState.Chasing)
            _anim.SetBool("isChasing", false);
        else if (_enemyState == EnemyState.Attack)
            _anim.SetBool("isAttack", false);
        */
        
        //update state
        _enemyState = newState;
        
        /*
        //update to current animation
        if (_enemyState == EnemyState.Chasing)
            _anim.SetBool("isChasing", true);
        else if (_enemyState == EnemyState.Attack)
            _anim.SetBool("isAttack", true);
            */
    }
    
    void PlayHitSound()
    {
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }
}

public enum EnemyState
{
    Chasing,
    Attack,
}