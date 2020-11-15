using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemCardData", menuName = "ScriptableObjects/ItemCard Data", order = 1)]
public class ItemCard : EquipmentCard
{
    [Header("Item Specific data.")]
    public ItemType itemType;
    [Range(0, 5)] public float effectRadius = 0;
    public List<WeaponCard> weapons = new List<WeaponCard>();

    public void UseItem(PlayerController pc)
    {
        // Debug.Log("Using item");
        if (itemType == ItemType.consumable)
        {
            Debug.Log("Using Consumable...");
        }
        else if (itemType == ItemType.equipment)
        {
            // Debug.Log("Giving Equipment...");
            
            WeaponCard newWeapon = weapons[Random.Range(0, weapons.Count)];

            EquipmentItem newItem = new EquipmentItem();
            newItem.card = newWeapon;
            newItem.ammoCount = newWeapon.AmmoCap;

            pc._inventory.Add(newItem);
        }
    }
}
