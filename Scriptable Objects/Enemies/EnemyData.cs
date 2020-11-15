using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType
{
    follow = 0,
    projectile = 1
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/Enemy Data", order = 2)]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Data")]
    public Sprite enemySprite = null;
    public EnemyType enemyType;
    public int MaxHealth = 0;
    public float MovementSpeed = 0;
    public float MinFollowDistance = 0;
    public int EnemyScore = 0;

    [Header("Damage Data")]
    public int Damage = 0;
    public float DamageRate = 0f;

    [Header("Projectile Data")]
    public Sprite projectileSprite = null;
    public float projectileVelocity = 0f;
    public float projectileKnockback = 0f;
    public float projectileLifetime = 0f;
    public float projectileHitBoxRadius = 0f;
    [Range(0f, 1f)] public float FireRate = 0f;
}
