using Assets._Game.Scripts;
using Assets._Game.Scripts.Interfaces;
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : Singleton<MovementController>
{
    private const float ROTATION_SPEED_MULTIPLIER = 1000;

    public bool CanMove = true;
    public float SwipeSpeed;
    public float SwipeMaxValue = 3;

    float firstPoint, SwipeDeltaValue;

    public float RotationSpeed = 10;
    public float MaxRotationDegree = 80f;
    public float MovementSpeed = 5f;
    public float XAngleResetDuration = 3f;

    private float flyRampAngle;
    private bool isFlying;
    private bool isOnGround;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
            interactable.OnInteracted();
    }

    void Update()
    {
        CalculateHorizontalDelta();

        if (SwipeDeltaValue != 0)
            RotateY();
        else
            ResetYAngleSmoothly();

        if (CanMove)
            Movement();

        if (isFlying)
            FlyAndResetXAngleSmoothly();

    }

    private void CalculateHorizontalDelta()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SwipeDeltaValue = 0;

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

                SwipeDeltaValue = (delta * SwipeSpeed) * Time.deltaTime;
                firstPoint = Input.mousePosition.x;
            }
            else
            {
                float delta = Input.GetTouch(0).position.x + Screen.width - firstPoint;

                SwipeDeltaValue = delta * SwipeSpeed * Time.deltaTime;
                firstPoint = Input.GetTouch(0).position.x + Screen.width;
            }
        }
        else
            SwipeDeltaValue = 0;
    }

    private void RotateY()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            float yAngle = SwipeDeltaValue * RotationSpeed * ROTATION_SPEED_MULTIPLIER * Time.deltaTime;

            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Vector3.up * xAngle), 100 *Time.deltaTime);

            transform.Rotate(0, yAngle, 0);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, UtilityHelper.ClampAngle(transform.eulerAngles.y, -MaxRotationDegree, MaxRotationDegree), 0);
        }
    }

    private void Movement()
    {
        var pos = Vector3.Lerp(transform.position, transform.position + transform.forward, MovementSpeed * Time.deltaTime);

        pos.x = Mathf.Clamp(pos.x, -SwipeMaxValue, SwipeMaxValue);

        transform.position = pos;
    }

    private void ResetYAngleSmoothly()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, 0, 0), RotationSpeed * Time.deltaTime);
    }

    public void Fly(float xAngle)
    {
        flyRampAngle = -xAngle;
        isFlying = true;

        transform.eulerAngles = new Vector3(-xAngle, transform.eulerAngles.y, 0);
    }

    private void FlyAndResetXAngleSmoothly()
    {
        //Quaternion startRotation = Quaternion.Euler(flyRampAngle, transform.eulerAngles.y, 0);
        Quaternion targetRotation = Quaternion.Euler(flyRampAngle + 90, transform.eulerAngles.y, 0);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 20 * Time.deltaTime);
    }

    public void StopFlying()
    {
        flyRampAngle = 0;
        isFlying = false;

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
