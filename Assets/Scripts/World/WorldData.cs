using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData
{
    public int seed;
    public float[,] noiseMap;

    public WorldData(World world)
    {
        seed = world.seed;
    }
}
