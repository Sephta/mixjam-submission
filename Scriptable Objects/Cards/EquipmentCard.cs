using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EquipmentType
{
    weapon = 0,
    item = 1
}

public enum WeaponType
{
    pistol = 0,
    rifle = 1,
    shotgun = 2,
    smg = 3,
    rocket = 4,
}

public enum WeaponFiringType
{
    semi = 0,
    full = 1
}

public enum ItemType
{
    consumable = 0,
    equipment = 1
}



public class EquipmentCard : ScriptableObject
{
    [Header("Entity/Card Data")]
    public Sprite EquipmentSprite = null;
    public EquipmentType type;
}
