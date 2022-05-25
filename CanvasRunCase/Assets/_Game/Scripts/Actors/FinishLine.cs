using Assets._Game.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour, IInteractable, IInteractableWithPlayer
{
    public bool IsInteracted { get; set; }
    public bool IsEnabled { get; set; } = true;

    public void OnInteracted(Ball ball = null)
    {
        print("int");

        if (ball == null)
            return;

        ball.rigidBody.useGravity = true;
        ball.collider.isTrigger = false;

        GameManager.Instance.BallGroups[ball.groupIndex].Remove(ball);

        //Instantiate(new GameObject("DEBUG"), ball.transform.position - new Vector3(0, 0, .02f), Quaternion.identity);

        ball.rigidBody.AddExplosionForce(Random.Range(100, 300), ball.transform.position + new Vector3(0, -.1f, -.2f), Random.Range(.3f, .5f));
    }

    public void OnInteracted()
    {
        GameManager.Instance.vmCam.Follow = null;
        MovementController.Instance.MovementSpeed /= 5;
    }
}
