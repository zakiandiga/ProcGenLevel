using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomWalkData", menuName = "ProcGen/RandomWalkData")]
public class RandomWalkData : ScriptableObject
{
    [Tooltip("Tile size for step distance consideration")]
    public int tileSize = 1;
    [Tooltip("Number of iteration to run")]
    public int iteration = 10;
    [Tooltip("Number of maximum steps per iteration")]
    public int walkLength = 10;

    [Tooltip("Number of iteration to run")]
    public bool startRandomEachIteration = true;
}
