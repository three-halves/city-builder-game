using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector2 lookSens;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnNavigate(InputValue value)
    {
        var v = value.Get<Vector2>();
        transform.position += new Vector3(v.x * lookSens.x, v.y * lookSens.y, 0);
    }
}
