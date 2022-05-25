using Assets._Game.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MoundsGroup : MonoBehaviour, IInteractableWithPlayer
{
    public bool IsInteracted { get; set; }
    public bool IsEnabled { get; set; } = true;

    public void OnInteracted()
    {
        if (!IsEnabled)
            return;

        if (!IsInteracted)
        {
            IsInteracted = true;
            GameManager.Instance.StartTheInclinationCheck();
        }
    }
}
