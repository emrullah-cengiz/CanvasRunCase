using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public int width = 7;
    public int length = 40;

    public Ball ballPrefab;
    public Transform referenceBallsTransform;
    public Transform ballsTransform;

    private float ballSize;
    public float minDistance = .25f;
    public float speed;
    public float threshold;

    void Start()
    {
        ballSize = ballPrefab.mesh.transform.localScale.x;

        //foreach (var referenceBall in GameManager.Instance.FirstLineBalls)
        float xPos = width / 2 * -(ballSize / 1.5f);
        for (int x = 0; x < width; x++)
        {
            Ball referenceBall = Instantiate(ballPrefab, referenceBallsTransform, false);
            referenceBall.transform.localPosition = Vector3.left * xPos;
            referenceBall.transform.rotation = Quaternion.identity;

            xPos += (ballSize / 1.5f);

            List<Ball> balls = new();

            for (int y = 0; y < length; y++)
            {
                Ball ball = Instantiate(ballPrefab, -referenceBall.transform.forward * ballSize, Quaternion.identity, ballsTransform);

                balls.Add(ball);
            }

            GameManager.Instance.Balls.Add(referenceBall, balls);
        }
    }

    private void Update()
    {
        foreach (var group in GameManager.Instance.Balls)
        {
            var frontBall = group.Key;

            for (int i = 0; i < group.Value.Count; i++)
            {
                var currentBall = group.Value[i];

                float currentDistance = Vector3.Distance(frontBall.transform.position, currentBall.transform.position);

                //print(i + " index " +currentDistance);

                Vector3 newPos = frontBall.transform.position;// + -frontBall.transform.forward * ballSize;

                var followTime = Time.deltaTime * currentDistance / minDistance * speed;

                if (followTime > threshold)
                    followTime = threshold;

                currentBall.transform.rotation = Quaternion.Slerp(currentBall.transform.rotation, frontBall.transform.rotation, followTime);
                currentBall.transform.position = Vector3.Slerp(currentBall.transform.position, newPos, followTime);

                frontBall = group.Value[i];
            }
        }
    }
}
