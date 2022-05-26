using Assets._Game.Scripts.Interfaces;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int groupIndex;
    public GameObject mesh;
    public Rigidbody rigidBody;
    public Collider collider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
            interactable.OnInteracted(this);
    }

}
