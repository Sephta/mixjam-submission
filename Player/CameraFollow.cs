using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform _objToFollow = null;
    [Range(0, 1)] public float dampTime = 0f;
    public Vector3 cameraOffset = Vector3.zero;

    [SerializeField, ReadOnly] private Vector3 _cameraVelocity = Vector3.zero;

    void Update()
    {
        if (_objToFollow)
         {
             Vector3 point = Camera.main.WorldToViewportPoint(_objToFollow.position);
             Vector3 delta = _objToFollow.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
             Vector3 destination = transform.position + delta;
             transform.position = Vector3.SmoothDamp(transform.position, destination, ref _cameraVelocity, dampTime);
         }
    }
}
