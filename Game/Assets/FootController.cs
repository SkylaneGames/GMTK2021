using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FootController : MonoBehaviour
{
    public Transform forwardTransform;
    public float movementForce = 0.01f;
    public float grabDistance = 1f;

    private Rigidbody _rigidbody;

    private Vector3 _currentForce;

    public bool IsAttached { get; private set; }
    private bool _shouldAttach;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Grab();
    }

    private void FixedUpdate()
    {
        if (_shouldAttach && !IsAttached)
        {
            TryAttach();
        }


        if (IsAttached) return;
        _rigidbody.AddForce(_currentForce);
    }

    private void TryAttach()
    {
        var closestPoint = GetClosestPointOnGround();

        if (!closestPoint.HasValue) return;

        Attach(closestPoint.Value);
    }

    private void Grab()
    {
        _shouldAttach = true;
    }

    private void Attach(RaycastHit hit)
    {
        IsAttached = true;
        _rigidbody.isKinematic = true;
        //Debug.Log($"Setting point {hit.point}, for transform at {transform.position}");
        transform.position = hit.point;
        //transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
    }

    private RaycastHit? GetClosestPointOnGround()
    {
        var direction = GetLizardDown();
        RaycastHit hit;

        if (Physics.Raycast(new Ray(transform.position, direction), out hit, grabDistance))
        {
            return hit;
        }

        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, GetLizardDown());
    }

    private Vector3 GetLizardDown()
    {
        // Armature is rotated -90 degree around x and is back to front, so the backward direction is now down.
        return forwardTransform.forward * -1;
    }

    private void Release()
    {
        _shouldAttach = false;
        IsAttached = false;
        _rigidbody.isKinematic = false;
    }

    public void GrabOrRelease(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Release();
        }
        else if (context.canceled)
        {
            Grab();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (IsAttached) return;
        
        var input = context.ReadValue<Vector2>();
        _currentForce = new Vector3(input.x, -input.y, 0) * movementForce;
        _currentForce = forwardTransform.localRotation * _currentForce;
    }
}
