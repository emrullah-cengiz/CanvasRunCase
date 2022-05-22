using Assets._Game.Scripts;
using Assets._Game.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovementController : MonoBehaviour
{
    private const float ROTATION_SPEED_MULTIPLIER = 1000;

    public bool CanMove = true;
    public float Speed;

    //public bool CanRotate = true;
    public float MaxRotationDegree = 80f;
    public float RotationSpeed = 10;

    public float _HorizontalMaxMinValue = 3;

    float firstPoint, _HorizontalDeltaValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
            interactable.OnInteracted();
    }

    void Update()
    {
        CalculateHorizontalDelta();

        if (_HorizontalDeltaValue != 0)
            Rotate();
        else
            ResetRotation();

        if (CanMove)
            Movement();
    }

    private void ResetRotation()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 5 * Time.deltaTime);
    }

    private void Movement()
    {
        var pos = Vector3.Lerp(transform.position, transform.position + transform.forward * 1, 5 * Time.deltaTime);

        pos.x = Mathf.Clamp(pos.x, -_HorizontalMaxMinValue, _HorizontalMaxMinValue);

        transform.position = pos;
    }

    private void Rotate()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            float xAngle = _HorizontalDeltaValue * RotationSpeed * ROTATION_SPEED_MULTIPLIER * Time.deltaTime;

            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Vector3.up * xAngle), 100 *Time.deltaTime);

            transform.Rotate(0, xAngle, 0);
            transform.eulerAngles = new Vector3(0, UtilityHelper.ClampAngle(transform.eulerAngles.y, -MaxRotationDegree, MaxRotationDegree), 0);
        }
    }

    private void CalculateHorizontalDelta()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _HorizontalDeltaValue = 0;

            if (UtilityHelper.IsEditorPlatform())
                firstPoint = Input.mousePosition.x;
            else
                firstPoint = (Input.GetTouch(0).position.x + Screen.width);
        }
        else if (Input.GetMouseButton(0))
        {
            if (UtilityHelper.IsEditorPlatform())
            {
                float delta = Input.mousePosition.x - firstPoint;

                _HorizontalDeltaValue = (delta * Speed) * Time.deltaTime;
                firstPoint = Input.mousePosition.x;
            }
            else
            {
                float delta = Input.GetTouch(0).position.x + Screen.width - firstPoint;

                _HorizontalDeltaValue = delta * Speed * Time.deltaTime;
                firstPoint = Input.GetTouch(0).position.x + Screen.width;
            }
        }
        else
            _HorizontalDeltaValue = 0;

        //Vector3 velocity = Vector3.zero;
        //velocity.x = _DeltaHorizontalValue;

        //transform.localPosition += velocity;
        //transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -_HorizontalMaxMinValue, _HorizontalMaxMinValue), 0, 0);
    }
}
