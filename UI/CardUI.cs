using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    public Image _image = null;
    public TextMeshProUGUI itemName = null;
    public TextMeshProUGUI ammoCount = null;
    public TextMeshProUGUI weaponType = null;
    public TextMeshProUGUI firingType = null;

    void Awake()
    {
        if (_image == null && transform.GetChild(0).GetComponent<Image>() != null)
            _image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }
}
