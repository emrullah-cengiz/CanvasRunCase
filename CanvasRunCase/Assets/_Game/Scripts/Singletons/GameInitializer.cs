using Assets.Scripts;
using System;
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

    private float ballSize, firstBallXPos;

    private void Awake()
    {
        ballSize = ballPrefab.mesh.transform.localScale.x;

        firstBallXPos = Width / 2 * -(ballSize / 1.5f);
    }

    void Start()
    {
        var xPos = firstBallXPos;

        for (int x = 0; x < Width; x++)
        {
            Transform referenceTransform = Instantiate(new GameObject(), referenceBallsTransform, false).transform;
            referenceTransform.localPosition = Vector3.left * xPos;
            referenceTransform.rotation = Quaternion.identity;

            GameManager.Instance.referenceTransforms.Add(new Tuple<Transform, bool>(referenceTransform, false));

            xPos += (ballSize / 1.5f);

            List<Ball> balls = new();

            for (int y = 0; y < Length; y++)
            {
                Ball ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity, ballsTransform);
                ball.groupIndex = x;

                balls.Add(ball);
            }

            GameManager.Instance.BallGroups.Add(balls);
        }
    }
    private void Update()
    {
        var xPos = firstBallXPos;

        for (int i = 0; i < Width; i++)
        {
            if (GameManager.Instance.BallGroups[i].Count == 0)
                continue;

            GameManager.Instance.referenceTransforms[i].Item1.localPosition = Vector3.left * xPos;
            xPos += (ballSize / 1.5f);
        }
    }
}
