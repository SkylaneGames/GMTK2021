using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FootController : MonoBehaviour
{
    public Transform forwardTransform;
    public float movementForce = 0.01f;

    private Rigidbody _rigidbody;
    private Renderer _render;

    private Vector3 _currentForce;

    public bool IsAttached { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _render = GetComponent<Renderer>();
    }

    private void Start()
    {
        Grab();
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(_currentForce);
    }

    private void Grab()
    {
        IsAttached = true;
        _rigidbody.isKinematic = true;
        _render.enabled = false;
    }

    private void Release()
    {
        IsAttached = false;
        _rigidbody.isKinematic = false;
        _render.enabled = true;
    }

    public void GrabOrRelease(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log($"[{name}] : Releasing Grip");
            Release();
        }
        else if (context.canceled)
        {
            Debug.Log($"[{name}] : Grabbing on");
            Grab();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        //Debug.Log($"[{name}] : Move, input={input}");
        if (IsAttached) return;
        
        var eulerAngles = forwardTransform.eulerAngles;
        _currentForce = new Vector3(input.x, 0 , input.y) * movementForce;
    }
}
