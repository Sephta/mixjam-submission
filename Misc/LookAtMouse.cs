using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    public SpriteRenderer _sr = null;
    [ReadOnly] public float angle = 0f;

    void Update()
    {
        Transform proj = transform.GetChild(0);

        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (angle > 90 || angle < -90)
        {
            _sr.flipX = true;
            angle += 180f;
            proj.localPosition = new Vector3(-1, 0f, 0f);
        }
        else
        {
            _sr.flipX = false;
            proj.localPosition = new Vector3(1, 0f, 0f);
        }

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
