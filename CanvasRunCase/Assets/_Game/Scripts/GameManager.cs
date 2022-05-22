using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<Ball> FirstLineBalls;
    public Dictionary<Ball, List<Ball>> Balls = new();


}
