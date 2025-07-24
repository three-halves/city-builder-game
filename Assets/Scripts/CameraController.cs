using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector2 _lookSens;
    [SerializeField] private Vector3 _targetPos;

    private float _smoothTime;

    private Vector3 _vel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _smoothTime = 0f;
        _targetPos = Vector3.zero;
        _vel = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, (Vector2)_targetPos, ref _vel, _smoothTime);

        Vector3 p = transform.position;
        p.z = -10;
        transform.position = p;
    }

    public void SetTargetPos(Vector3 targetPos, float smoothTime = 0.25f)
    {
        _targetPos = targetPos;
        _smoothTime = smoothTime;
    }

    public void Navigate(Vector2 delta)
    {
        transform.position += (Vector3)(delta * _lookSens);
        _targetPos = transform.position;
    }
}
