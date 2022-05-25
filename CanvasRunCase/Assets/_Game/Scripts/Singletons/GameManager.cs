using Assets.Scripts;
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    const string MOUND_TAG = "Mound";
    const string MOUND_GROUP_GROUND_TAG = "MoundGroupGround";

    /// <summary>
    /// Tuple.Item1 = transform, Tuple.Item2 = is group slow now?
    /// </summary>
    [HideInInspector] public List<Tuple<Transform, bool>> referenceTransforms = new();
    [HideInInspector] public List<List<Ball>> BallGroups = new();
    [HideInInspector] public List<Ball> BallsToScheculedRemove = new();

    public LayerMask moundLayer;
    public CinemachineVirtualCamera vmCam;
    public float ballsMinDistanceForReposition = .25f;
    public float repositionSpeed;
    public float maxTimeThresholdForReposition;
    public bool checkForInclination;

    private void Update()
    {
        TailFollowing();

        if (checkForInclination)
            InclinationCheck();
    }

    private void InclinationCheck()
    {
        for (int x = 0; x < BallGroups.Count; x++)
        {
            if (BallGroups[x].Count == 0)
                continue;

            for (int i = 0; i < BallGroups[x].Count; i++)
            {
                var ball = BallGroups[x][i];

                if (Physics.Raycast(new Vector3(ball.transform.position.x, 1, ball.transform.position.z), Vector3.down, out RaycastHit hit, 2f, moundLayer))
                    ball.transform.position = hit.point;
            }
        }
    }

    private void TailFollowing()
    {
        for (int x = 0; x < BallGroups.Count; x++)
        {
            var frontBallTransform = referenceTransforms[x].Item1;

            for (int i = 0; i < BallGroups[x].Count; i++)
            {
                var currentBall = BallGroups[x][i];

                float currentDistance = Vector3.Distance(frontBallTransform.position, currentBall.transform.position);

                Vector3 newPos = frontBallTransform.position;

                var followTime = Time.deltaTime * currentDistance / ballsMinDistanceForReposition * repositionSpeed;// * (referenceTransforms[x].Item2 ? .05f : 1);

                if (followTime > maxTimeThresholdForReposition)
                    followTime = maxTimeThresholdForReposition;

                currentBall.transform.SetPositionAndRotation(
                                Vector3.Lerp(currentBall.transform.position, newPos, followTime),
                                Quaternion.Lerp(currentBall.transform.rotation, frontBallTransform.rotation, followTime));

                frontBallTransform = currentBall.transform;
            }
        }
    }

    public IEnumerator RetarderTimerForGroup(int groupIndex)
    {
        var refTransform = referenceTransforms[groupIndex];

        if (refTransform.Item2)
            yield break;

        referenceTransforms[groupIndex] = new Tuple<Transform, bool>(refTransform.Item1, true);

        yield return new WaitForSeconds(.3f);

        foreach (var item in BallsToScheculedRemove)
            BallGroups[item.groupIndex].Remove(item);

        referenceTransforms[groupIndex] = new Tuple<Transform, bool>(refTransform.Item1, false);

        yield break;
    }

    public void StartTheInclinationCheck() => checkForInclination = true;
}
