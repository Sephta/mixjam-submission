using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIHandler : MonoBehaviour
{
    [Header("Dependencies")]
    public Rigidbody2D _rb = null;
    public Collider2D _collider = null;
    public SpriteRenderer sr = null;
    public GameObject projectileRefr = null;
    public Transform projHolder = null;
    public Transform projLaunchPoint = null;
    public ProgressBar healthBar = null;

    public EnemyData _dataContainer;
    public List<GameObject> lootList = new List<GameObject>();

    [Header("Enemy Data")]
    public EnemyType _type;
    public Transform target = null;
    public int maxHealth = 0;
    public float moveSpeed = 0f;
    public float minDistance = 0f;

    [Header("Damage Data")]
    public int damageAmount = 0;
    public float damageTickRate = 0f;


    [Header("Debug Data")]
    [ReadOnly] public int currHealth = 0;
    [ReadOnly] public bool _wake = false;
    [ReadOnly] public Vector2 direction = Vector2.zero;
    [ReadOnly] private float timeLastDamageDealt = 0f;
    [ReadOnly] private float timeLastFired = 0f;
    [SerializeField, ReadOnly] private bool colliding = false;
    [ReadOnly] public bool applyKnockback = false;
    [SerializeField, ReadOnly] private float _knockBackForce = -1f;
    // [ReadOnly] public float launchAngle = 0f;
    // [ReadOnly] public Vector3 projDir = Vector3.zero;


    void Awake()
    {
        if (_rb == null && GetComponent<Rigidbody2D>() != null)
            _rb = GetComponent<Rigidbody2D>();

        if (_collider == null && GetComponent<CircleCollider2D>() != null)
            _collider = GetComponent<CircleCollider2D>();

        if (sr == null && GetComponent<SpriteRenderer>() != null)
            sr = GetComponent<SpriteRenderer>();
        
        if (projHolder == null)
            projHolder = transform.GetChild(0);
        
        if (projLaunchPoint == null)
            projLaunchPoint = transform.GetChild(0).GetChild(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        _wake = true;
        currHealth = maxHealth;
        healthBar.max = healthBar.current = currHealth;
        healthBar.current = currHealth;

        if (_type == EnemyType.follow)
            projLaunchPoint.GetComponent<SpriteRenderer>().sprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (currHealth <= 0)
        {
            SpawnLoot();

            if (ArenaManager._inst != null)
                ArenaManager._inst.currEntities -= 1;
            
            target.gameObject.GetComponent<PlayerController>().currPlayerScore += _dataContainer.EnemyScore;

            Destroy(this.gameObject);
        }

        Vector3 launchDir = target.position - transform.position;
        float launchAngle = Mathf.Atan2(launchDir.y, launchDir.x) * Mathf.Rad2Deg;
        
        if (launchAngle > 90 || launchAngle < -90)
        {
            sr.flipX = true;
            launchAngle += 180f;
            projLaunchPoint.localPosition = new Vector3(-1, 0f, 0f);
        }
        else
        {
            sr.flipX = false;
            projLaunchPoint.localPosition = new Vector3(1, 0f, 0f);
        }

        projHolder.rotation = Quaternion.AngleAxis(launchAngle, Vector3.forward);
    }

    void FixedUpdate()
    {
        if (_wake)
        {
            // TODO - do ai stuff
            switch (_type)
            {
                case EnemyType.follow:
                    FollowBehavior();
                    break;
                
                case EnemyType.projectile:
                    ProjectileBehavior();
                    break;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (_type == EnemyType.projectile)
            return;

        if (collision.gameObject.tag == "Player")
        {
            colliding = true;
            if ((Time.time - timeLastDamageDealt) >= damageTickRate)
            {
                target.GetComponent<PlayerController>().TakeDamage(damageAmount);
                timeLastDamageDealt = Time.time;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        colliding = false;
    }


    public void FollowBehavior()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) > minDistance)
            {
                direction = target.position - transform.position;
                _rb.AddForce((direction.normalized * moveSpeed * Time.fixedDeltaTime), ForceMode2D.Impulse);

                if (applyKnockback)
                {
                    _rb.AddForce((-direction.normalized * _knockBackForce), ForceMode2D.Impulse);
                    applyKnockback = false;
                    _knockBackForce = -1f;
                }
            }
        }
    }

    public void ProjectileBehavior()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) > minDistance)
            {
                direction = target.position - transform.position;
                _rb.AddForce((direction.normalized * moveSpeed * Time.fixedDeltaTime), ForceMode2D.Impulse);

                if (applyKnockback)
                {
                    Vector2 dir = transform.position - transform.TransformVector(projLaunchPoint.position);
                    _rb.AddForce((dir.normalized * _knockBackForce), ForceMode2D.Impulse);
                    applyKnockback = false;
                    _knockBackForce = -1f;
                }
            }
            else
            {
                if ((Time.time - timeLastFired) >= damageTickRate)
                {
                    Vector3 projDir = target.position - transform.position;
                    projDir = projDir.normalized;

                    GameObject refr = Instantiate(projectileRefr, transform.TransformVector(projLaunchPoint.position), Quaternion.identity);
                    ProjectileHandler pHandler = refr.GetComponent<ProjectileHandler>();

                    pHandler.damagePlayer = true;
                    pHandler.SetLookDirection(projDir);
                    pHandler.sr.sprite = _dataContainer.projectileSprite;
                    pHandler.projectileVelocity = _dataContainer.projectileVelocity;
                    pHandler.projectileDamage = _dataContainer.Damage;
                    pHandler.projectileLifetime = _dataContainer.projectileLifetime;

                    AudioManager._inst.PlaySFX(7);

                    timeLastFired = Time.time;
                }
            }
        }
    }


    public void TakeDamage(int amount, float knockBack)
    {
        currHealth = (int)Mathf.Clamp((currHealth - amount), 0, maxHealth);
        healthBar.current = currHealth;
        applyKnockback = true;
        _knockBackForce = knockBack;
    }

    private void SpawnLoot()
    {
        GameObject refr = Instantiate(lootList[Random.Range(0, lootList.Count)], transform.position, Quaternion.identity);
    }
}
