using System;
using UnityEngine;

[Serializable]
public struct WavePart
{
    /// <summary>
    /// Name of the enemy type
    /// </summary>
    public string EnemyName;
    /// <summary>
    /// Amount of enemies for this significant wave
    /// </summary>
    public int EnemyAmount;
}