using Assets._Game.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FlyRamp : MonoBehaviour, IInteractable
{
    public float FlyAngle = 45f;

    public Transform rotatable;
    public bool IsInteracted { get; set; }
    public bool IsEnabled { get; set; } = true;

    public void OnInteracted()
    {
        if (!IsEnabled)
            return;

        if (!IsInteracted)
        {
            IsInteracted = true;
            MovementController.Instance.Fly(FlyAngle);
        }
        else
        {
            IsEnabled = false;
            MovementController.Instance.StopFlying();
        }
    }

    private void OnValidate()
    {
        rotatable.eulerAngles = Vector3.right * (FlyAngle + 13f);
    }
}
