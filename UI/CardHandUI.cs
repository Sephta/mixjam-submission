using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandUI : MonoBehaviour
{
    public PlayerController pc = null;
    public GameObject _cardRefr = null;

    public List<GameObject> _hand = new List<GameObject>();
    public List<EquipmentItem> _copyInv = new List<EquipmentItem>();

    // Start is called before the first frame update
    void Start()
    {
        if (pc == null)
            pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
        if (pc != null)
        {
            UpdateCardHand();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pc != null && pc._handChanged)
        {
            UpdateCardHand();
        }


        if (pc._currInvIndex >= 0 && pc._currInvIndex < _hand.Count)
        {
            _hand[pc._currInvIndex].transform.GetChild(0).GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
            float oldY = _hand[pc._currInvIndex].GetComponent<RectTransform>().position.y;
            LeanTween.moveLocalY(_hand[pc._currInvIndex], oldY + 50f, 0.125f);

            for (int i = 0; i < _hand.Count; i++)
                if (i != pc._currInvIndex)
                {
                    _hand[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                    LeanTween.moveLocalY(_hand[i], 0f, 0.125f);
                }
        }
    }


    public void UpdateCardHand()
    {
        if (_hand.Count > 0)
            _hand.Clear();

        if (_copyInv.Count > 0)
            _copyInv.Clear();

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        for (int i = 0; i < pc._inventory.Count; i++)
        {
            if (pc._inventory[i] != null)
            {
                GameObject refr = Instantiate(_cardRefr, this.transform);

                if (pc._inventory[i].card != null)
                    refr.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = pc._inventory[i].card.EquipmentSprite;
                
                CardUI cardInfo = refr.GetComponent<CardUI>();

                WeaponCard wTemp = (WeaponCard)pc._inventory[i].card;

                cardInfo.itemName.text = pc._inventory[i].card.name;
                cardInfo.ammoCount.text = pc._inventory[i].ammoCount.ToString() + "/" + wTemp.AmmoCap;
                cardInfo.firingType.text = wTemp.FireMode.ToString();

                _hand.Add(refr);
            }
        }

        foreach (EquipmentItem card in pc._inventory)
                _copyInv.Add(card);

        pc._handChanged = false;
    }
}
