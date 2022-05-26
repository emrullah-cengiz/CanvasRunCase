using Assets._Game.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleStick : MonoBehaviour, IInteractable
{
    public bool IsInteracted { get; set; }
    public bool IsEnabled { get; set; } = true;

    public void OnInteracted(Ball ball)
    {
        GameManager.Instance.BallsToScheculedRemove.Add(ball);

        StartCoroutine(GameManager.Instance.RetarderTimerForGroup(ball.groupIndex));

        ball.enabled = false;

        ball.mesh.SetActive(false);
    }
}
