using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public EquipmentCard _item;
    public SpriteRenderer sr = null;

    public float decayTime = 0f;
    [ReadOnly] public float currDecayTime = 0f;

    void Awake()
    {
        if (sr == null && GetComponent<SpriteRenderer>() != null)
            sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        sr.sprite = _item.EquipmentSprite;

        currDecayTime = decayTime;
    }

    void Update()
    {
        currDecayTime -= Time.deltaTime;
        if (currDecayTime <= 0)
            Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            PlayerController pc = collider.gameObject.GetComponent<PlayerController>();

            int count = 0;
            for (int i = 0; i < pc._inventory.Count; i++)
            {
                if (pc._inventory[i].card != null)
                    count++;
            }

            if (count < pc._maxInvSize)
            {
                pc.GiveNewCard(_item);
                Destroy(this.gameObject);
            }
        }
    }
}
