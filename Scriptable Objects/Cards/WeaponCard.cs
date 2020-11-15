using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponCardData", menuName = "ScriptableObjects/WeaponCard Data", order = 0)]
public class WeaponCard : EquipmentCard
{
    [Header("Weapon Specific data.")]
    public Sprite projectileSprite = null;
    public WeaponType weaponType;
    public WeaponFiringType FireMode;
    public int sfxIndex = 0;

    [Tooltip("Prefab reference of the projectile")]
    public GameObject projectile = null;
    
    [Tooltip("Speed of the projectile.")]
    public float projectileVelocity = 0f;

    [Tooltip("Amount of flat damage projectile will do.")]
    public int projectileDamage = 0;

    [Tooltip("Amount of force applied as knockback from firing projectile.")]
    public float projectileKnockback_player = 0f;

    [Tooltip("Amount of force applied as knockback from being hit by projectile.")]
    public float projectileKnockback_enemy = 0f;
    
    [Tooltip("Life time of the projectile till deletion (Garbage collectio).")]
    public float projectileLifetime = 0f;
    
    [Tooltip("Radius of the bullet / projectiles fired from this weapon.")]
    public float projectileHitBoxRadius = 0f;

    [Tooltip("In bullets per second.")]
    [Range(0f, 1f)] public float FireRate = 0f;

    public int AmmoCap = 0;


    public void FireWeapon(PlayerController pc)
    {
        if (FireMode == WeaponFiringType.full)
        {
            if (((Time.time - pc.timeLastFired) >= FireRate) && pc._firingWeapon && pc._inventory[pc._currInvIndex].ammoCount > 0)
            {
                if (projectile != null)
                {
                    GameObject refr = Instantiate(projectile, pc.transform.GetChild(0).GetChild(0).position, Quaternion.identity);
                    pc.timeLastFired = Time.time;
                    pc._knockBackForce *= projectileKnockback_player;
                    pc.applyKnockback = true;
                    ProjectileHandler pHandler = refr.GetComponent<ProjectileHandler>();

                    AudioManager._inst.PlaySFX(sfxIndex);

                    pHandler.damagePlayer = false;
                    Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(pc.transform.position);
                    pHandler._collider.radius = projectileHitBoxRadius;
                    pHandler.sr.sprite = projectileSprite;
                    pHandler.SetLookDirection(dir.normalized);
                    pHandler.projectileVelocity = projectileVelocity;
                    pHandler.projectileDamage = projectileDamage;
                    pHandler.projectileLifetime = projectileLifetime;
                    pHandler.projectileKnockback = projectileKnockback_enemy;

                    if (!pc.INFAMMO)
                    {
                        pc._inventory[pc._currInvIndex].ammoCount -= 1;
                        pc._inventory[pc._currInvIndex].ammoCount = Mathf.Clamp(pc._inventory[pc._currInvIndex].ammoCount, 0, AmmoCap);
                        pc._handChanged = true;
                    }
                }
            }
        }
        else if (FireMode == WeaponFiringType.semi)
        {
            if (!pc._firingWeapon)
            {
                if (((Time.time - pc.timeLastFired) >= FireRate) && projectile != null && pc._inventory[pc._currInvIndex].ammoCount > 0)
                {
                    GameObject refr = Instantiate(projectile, pc.transform.GetChild(0).GetChild(0).position, Quaternion.identity);
                    pc.timeLastFired = Time.time;
                    pc._knockBackForce *= projectileKnockback_player;
                    pc.applyKnockback = true;
                    ProjectileHandler pHandler = refr.GetComponent<ProjectileHandler>();

                    AudioManager._inst.PlaySFX(sfxIndex);

                    Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(pc.transform.position);
                    pHandler._collider.radius = projectileHitBoxRadius;
                    pHandler.sr.sprite = projectileSprite;
                    pHandler.SetLookDirection(dir.normalized);
                    pHandler.projectileVelocity = projectileVelocity;
                    pHandler.projectileDamage = projectileDamage;
                    pHandler.projectileLifetime = projectileLifetime;
                    pHandler.projectileKnockback = projectileKnockback_enemy;
                    if (weaponType == WeaponType.rocket)
                        pHandler.explode = true;


                    if (!pc.INFAMMO)
                    {
                        pc._inventory[pc._currInvIndex].ammoCount -= 1;
                        pc._inventory[pc._currInvIndex].ammoCount = Mathf.Clamp(pc._inventory[pc._currInvIndex].ammoCount, 0, AmmoCap);
                        pc._handChanged = true;
                    }
                }
            }
        }
    }
}
