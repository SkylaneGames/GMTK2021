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
    public float angularForce = 1f;
    public Transform forwardTransform;
    

    private IEnumerable<FootController> _feet;


    private Vector3 _currentForce;
    private float _currentAngularForce;

    private void Awake()
    {
        _feet = GetComponentsInChildren<FootController>();
    }

    private void FixedUpdate()
    {
        if (_feet.All(p => !p.IsAttached)) return;

        mainBody.AddRelativeTorque(0, 0, _currentAngularForce);
        mainBody.AddRelativeForce(_currentForce);
    }

    public void ShiftWeight(InputAction.CallbackContext context)
    {
        // If any feet are detached then don't shift weight, shift body instead.
        //if (_feet.Any(p => !p.IsAttached)) return;

        var input = context.ReadValue<Vector2>();

        _currentForce = new Vector3(0, input.y, 0) * movementForce;
        _currentAngularForce = input.x * angularForce;
    }

    //public void RotateBody(InputAction.CallbackContext context)
    //{
    //    //if (_feet.Any(p => !p.IsAttached)) return;

    //    var input = context.ReadValue<float>();
    //    _currentAngularForce = input * angularForce;
    //}
}
