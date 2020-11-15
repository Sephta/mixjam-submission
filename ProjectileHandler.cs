using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    public Rigidbody2D _rb = null;
    public CircleCollider2D _collider = null;
    public SpriteRenderer sr = null;
    public GameObject explosionRefr = null;
    [ReadOnly] public bool explode = false;
    [ReadOnly] public bool damagePlayer = false;
    [ReadOnly] public Vector2 _direction = Vector2.zero;
    public float projectileVelocity = 0f;
    public float projectileLifetime = 0f;
    public float projectileKnockback = 0f;
    public int projectileDamage= 0;
    [SerializeField, ReadOnly] private float lifetime = 0f;
    [SerializeField, ReadOnly] private float angle = 0f;
    [SerializeField, ReadOnly] private bool enemyHit = false;


    void Awake()
    {
        if (sr == null && GetComponent<SpriteRenderer>() != null)
            sr = GetComponent<SpriteRenderer>();

        if (_collider == null && GetComponent<CircleCollider2D>())
            _collider = GetComponent<CircleCollider2D>();

        SetLookDirection(_direction);
    }

    void Start()
    {
        lifetime = projectileLifetime;
    }

    void Update()
    {
        if (lifetime == 0f || enemyHit)
            Destroy(this.gameObject);
        else
        {
            lifetime -= Time.deltaTime;
            lifetime = Mathf.Clamp(lifetime, 0, projectileLifetime);
        }
    }

    void FixedUpdate()
    {
        // _rb.AddForce((_direction * projectileVelocity * Time.fixedDeltaTime + _rb.velocity) - _rb.velocity, ForceMode2D.Impulse);
        _rb.velocity = _direction * projectileVelocity * Time.fixedDeltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy" && !damagePlayer)
        {
            if (explode)
            {
                GameObject refr = Instantiate(explosionRefr, transform.position, Quaternion.identity);   
                ExplosionHandler eHandler = refr.GetComponent<ExplosionHandler>();
                eHandler.knockBack = projectileKnockback;
                eHandler.maxDamage = projectileDamage;
                eHandler.explode = true;
            }
            collider.gameObject.GetComponent<AIHandler>().TakeDamage(projectileDamage, projectileKnockback);
            enemyHit = true;
            AudioManager._inst.PlaySFX(8);
        }
        else if (collider.gameObject.tag == "Player" && damagePlayer)
        {
            collider.gameObject.GetComponent<PlayerController>().TakeDamage(projectileDamage);
            enemyHit = true;
        }
    }


    public void SetLookDirection(Vector2 dir)
    {
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _direction = dir;
    }
}
