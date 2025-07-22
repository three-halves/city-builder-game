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

    void OnNavigate(InputValue value)
    {
        var v = value.Get<Vector2>();
        if (v != Vector2.zero)
        {
            _targetPos += new Vector3(v.x * _lookSens.x, v.y * _lookSens.y, 0);
            _smoothTime = 0.001f;
        }
    }

    public void  SetTargetPos(Vector3 targetPos, float smoothTime = 0.25f)
    {
        _targetPos = targetPos;
        _smoothTime = smoothTime;
    }
}
