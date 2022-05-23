using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : Singleton<GameInitializer>
{
    public int Width = 7;
    public int Length = 40;

    public Ball ballPrefab;
    public Transform referenceBallsTransform;
    public Transform ballsTransform;

    private float ballSize;

    void Start()
    {
        ballSize = ballPrefab.mesh.transform.localScale.x;

        float xPos = Width / 2 * -(ballSize / 1.5f);

        for (int x = 0; x < Width; x++)
        {
            Ball referenceBall = Instantiate(ballPrefab, referenceBallsTransform, false);
            referenceBall.transform.localPosition = Vector3.left * xPos;
            referenceBall.transform.rotation = Quaternion.identity;

            xPos += (ballSize / 1.5f);

            List<Ball> balls = new();

            for (int y = 0; y < Length; y++)
            {
                Ball ball = Instantiate(ballPrefab, -referenceBall.transform.forward * ballSize, Quaternion.identity, ballsTransform);

                balls.Add(ball);
            }

            GameManager.Instance.Balls.Add(referenceBall, balls);
        }
    }
}
