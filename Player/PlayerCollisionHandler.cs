using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public PlayerController pc = null;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.tag == "Card")
            // pc.GiveNewCard(collision.gameObject.GetComponent<WorldItem>()._item);
    }

    void OnCollisionExit2D(Collision2D collision)
    {

    }
}
