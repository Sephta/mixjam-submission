using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EquipmentItem
{
    public EquipmentCard card;
    public int ammoCount;
}


public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    public Rigidbody2D _rb = null;
    public SpriteRenderer sr = null;
    public Animator _anim = null;
    public ProgressBar healthBar = null;

    public CardHandUI _cardHandUI = null;

    [Header("Cheats")]
    public bool INFAMMO = false;
    public WeaponCard starterWeapon;
    public bool hasControl = true;

    [Header("Player Data")]
    public int _maxHealth = 0;
    public int _maxInvSize = 0;
    public float _movementSpeed = 0f;
    public EquipmentCard _current;
    public List<EquipmentItem> _inventory = new List<EquipmentItem>();
    public bool _firingWeapon = false;
    
    [ReadOnly] public int currPlayerScore = 0;
    [ReadOnly] public int _currHealth = 0;
    [ReadOnly] public int _currInvIndex = 0;
    [ReadOnly] public float timeLastFired = 0f;
    [ReadOnly] public bool _handChanged = true;
    [ReadOnly] public bool applyKnockback = false;
    [ReadOnly] public float _knockBackForce = -1f;

    [SerializeField, ReadOnly] private EquipmentCard _previous;
    [ReadOnly] public int _prevInvIndex = 0;
    [SerializeField, ReadOnly] private Vector2 _dir = Vector2.zero;
    [SerializeField, ReadOnly] private float timeSinceLastMove = 0f;


    void Awake()
    {
        if (_anim == null && GetComponent<Animator>() != null)
            _anim = GetComponent<Animator>();
    }

    void Start()
    {
        healthBar.current = healthBar.max = _currHealth = _maxHealth;
        _current = _inventory[_currInvIndex].card;

        if (_current != null)
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _current.EquipmentSprite;
        
        if (_cardHandUI == null)
            _cardHandUI = GameObject.FindGameObjectWithTag("Card Hand").GetComponent<CardHandUI>();
        
        for (int i = 0; i < _inventory.Count; i++)
        {
            if (_inventory[i].card != null)
            {
                WeaponCard temp = (WeaponCard)_inventory[i].card;
                _inventory[i].ammoCount = temp.AmmoCap;
            }
        }
    }
    
    void Update()
    {
        if (hasControl)
        {
            GetDirectionVector();

            if (_dir != Vector2.zero)
            {
                _anim.SetBool("IsMoving", true);
                if ((Time.time - timeSinceLastMove) >= 0.25f)
                {
                    AudioManager._inst.PlaySFX(4);
                    timeSinceLastMove = Time.time;
                }
            }
            else
                _anim.SetBool("IsMoving", false);


            if (Input.GetKey(InputManager._inst._keyBindings[InputAction.attack]))
            {
                if (_current != null)
                {
                    if (_current.type == EquipmentType.weapon)
                    {
                        WeaponCard weapon = (WeaponCard)_current;
                        weapon.FireWeapon(this);
                    }
                    else
                    {
                        ItemCard item = (ItemCard)_current;
                        item.UseItem(this);
                    }
                    _firingWeapon = true;
                }
            }
            else
                _firingWeapon = false;

            if (Input.GetKeyDown(InputManager._inst._keyBindings[InputAction.nextItem]))
            {
                _prevInvIndex = _currInvIndex;
                _currInvIndex -= 1;
                if (_currInvIndex < 0)
                    _currInvIndex = _inventory.Count - 1;

                _currInvIndex = Mathf.Clamp(_currInvIndex, 0, _inventory.Count);
            }
            else if (Input.GetKeyDown(InputManager._inst._keyBindings[InputAction.prevItem]))
            {
                _prevInvIndex = _currInvIndex;
                _currInvIndex += 1;
                if (_currInvIndex > _inventory.Count - 1)
                    _currInvIndex = 0;

                _currInvIndex = Mathf.Clamp(_currInvIndex, 0, _inventory.Count);
            }

            if (_currInvIndex != _prevInvIndex)
            {
                _previous = _current;
                if (_inventory[_currInvIndex].card != null)
                    _current = _inventory[_currInvIndex].card;
                else
                    _current = null;
            }
            
            if (_current != _previous)
            {
                if (_current != null)
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _current.EquipmentSprite;
                else
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
            }
            else if (_current == null)
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;

            UpdateSpriteLookDirection();

            UpdatePlayerHand();
        }
    }

    void FixedUpdate()
    {
        if (hasControl)
        {
            _rb.AddForce((_dir * _movementSpeed * Time.fixedDeltaTime) , ForceMode2D.Impulse);

            if (applyKnockback)
            {
                Vector2 lookDir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
                lookDir = lookDir.normalized;
                _rb.AddForce((lookDir * _knockBackForce), ForceMode2D.Impulse);
                applyKnockback = false;
                _knockBackForce = -1f;
            }
        }
    }


    public void GiveNewCard(EquipmentCard newCard)
    {
        if (newCard.type == EquipmentType.item)
        {
            ItemCard iCard = (ItemCard)newCard;
            iCard.UseItem(this);
        }
        else if (newCard.type == EquipmentType.weapon)
        {
            EquipmentItem newItem = new EquipmentItem();
            newItem.card = newCard;
            WeaponCard temp = (WeaponCard)newCard;
            newItem.ammoCount = temp.AmmoCap;

            _inventory.Add(newItem);
        }

        _handChanged = true;
    }

    public void RemoveCard(int index)
    {
        _inventory.RemoveAt(index);

        if (_inventory.Count == 0)
            GiveNewCard(starterWeapon);

        _current = _inventory[0].card;
        _currInvIndex = 0;

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _current.EquipmentSprite;

        _handChanged = true;
    }

    public void TakeDamage(int amount)
    {
        _currHealth = (int)Mathf.Clamp((_currHealth - amount), 0, _maxHealth);
        healthBar.current = _currHealth;
    }

    // Gets a normalized direction vector from the movement keys
    private void GetDirectionVector()
    {
        if (InputManager._inst != null)
        {
            if (Input.GetKey(InputManager._inst._keyBindings[InputAction.moveUp]))
            {
                _dir.y = 1.0f;
            }
            else if (Input.GetKey(InputManager._inst._keyBindings[InputAction.moveDown]))
            {
                _dir.y = -1.0f;
            }
            else
                _dir.y = 0f;

            if (Input.GetKey(InputManager._inst._keyBindings[InputAction.moveLeft]))
            {
                _dir.x = -1.0f;
            }
            else if (Input.GetKey(InputManager._inst._keyBindings[InputAction.moveRight]))
            {
                _dir.x = 1.0f;
            }
            else
                _dir.x = 0f;
        }

        _dir = _dir.normalized;
    }

    // Flips the character sprite on the x-axis to face the cursor
    private void UpdateSpriteLookDirection()
    {
        Vector2 lookDir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        lookDir = lookDir.normalized;
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        if (lookAngle > 90 || lookAngle < -90)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    private void UpdatePlayerHand()
    {
        for (int i = 0; i < _inventory.Count; i++)
        {
            if (_inventory[i] == null)
                continue;

            if (_inventory[i].ammoCount <= 0)
            {
                RemoveCard(i);
            }
        }
    }
}
