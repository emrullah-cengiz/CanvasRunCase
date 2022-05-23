using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public Dictionary<Ball, List<Ball>> Balls = new();

    public float ballsMinDistanceForReposition = .25f;
    public float repositionSpeed;
    public float maxTimeThresholdForReposition;

    private void Update()
    {
        foreach (var group in Balls)
        {
            var frontBall = group.Key;

            for (int i = 0; i < group.Value.Count; i++)
            {
                var currentBall = group.Value[i];

                float currentDistance = Vector3.Distance(frontBall.transform.position, currentBall.transform.position);

                //print(i + " index " +currentDistance);

                Vector3 newPos = frontBall.transform.position;// + -frontBall.transform.forward * ballSize;

                var followTime = Time.deltaTime * currentDistance / ballsMinDistanceForReposition * repositionSpeed;

                if (followTime > maxTimeThresholdForReposition)
                    followTime = maxTimeThresholdForReposition;

                currentBall.transform.rotation = Quaternion.Lerp(currentBall.transform.rotation, frontBall.transform.rotation, followTime);
                currentBall.transform.position = Vector3.Lerp(currentBall.transform.position, newPos, followTime);

                frontBall = group.Value[i];
            }
        }
    }
}
