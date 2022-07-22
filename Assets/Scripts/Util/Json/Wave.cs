using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Wave
{
    /// <summary>
    /// "[Location].[Level].[WaveNumber]"
    /// </summary>
    public string Id;
    /// <summary>
    /// Enemies amount per second
    /// </summary>
    public float Density;
    /// <summary>
    /// Info about enemy type and amount
    /// </summary>
    public WavePart[] WaveParts;
}