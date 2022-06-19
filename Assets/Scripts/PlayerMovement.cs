using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private float _speed = 1;
    private FloatingJoystick _floatingJoystick;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _floatingJoystick = FindObjectOfType<FloatingJoystick>();
        if(_floatingJoystick == null)
        {
            Debug.LogWarning("Can't find FLOATING JOYSTICK (PlayerMovement.cs). Please add FloatingJoystick on scene.");
            enabled = false;
        }
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(-_floatingJoystick.Direction.y, 0, _floatingJoystick.Direction.x);
        _rigidbody.velocity = moveDirection * _speed;
        transform.LookAt(transform.position + moveDirection);
    }
}
