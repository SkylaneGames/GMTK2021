using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInputController : MonoBehaviour
{
    public Rigidbody mainBody;
    public float movementForce = 1f;
    public Transform forwardTransform;
    

    private IEnumerable<FootController> _feet;


    private Vector3 _currentForce;

    private void Awake()
    {
        _feet = GetComponentsInChildren<FootController>();
    }

    private void FixedUpdate()
    {
        if (_feet.Any(p => !p.IsAttached)) return;

        mainBody.AddForce(_currentForce);
    }

    public void ShiftWeight(InputAction.CallbackContext context)
    {
        // If any feet are detached then don't shift weight, shift body instead.
        if (_feet.Any(p => !p.IsAttached)) return;

        var input = context.ReadValue<Vector2>();

        var eulerAngles = forwardTransform.eulerAngles;
        _currentForce = new Vector3(-input.x, 0 , -input.y) * movementForce;
    }

    public void RotateBody(InputAction.CallbackContext context)
    {

    }
}
