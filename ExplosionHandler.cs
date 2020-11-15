using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    [SerializeField] LayerMask targetLayers;
    public float explosionRadius = 0f;
    public float knockBack = 0f;
    public int maxDamage = 0;
    public int damage = 0;
    public int damageFallOffRate = 0;
    public bool explode = false;

    private bool _hit = false;

    // Start is called before the first frame update
    void Start()
    {
        damage = maxDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (explode)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero, 0.001f, targetLayers);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    damage = Mathf.Clamp(damage - damageFallOffRate, 0, maxDamage);
                    hit.collider.GetComponent<AIHandler>().TakeDamage(damage, knockBack);
                    _hit = true;
                }
            }

            if (_hit)
            {
                AudioManager._inst.PlaySFX(2);
                _hit = false;
            }

            Destroy(this.gameObject);
        }
    }
}
