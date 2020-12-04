using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Planets
{
    public interface INoiseFilter
    {
        float Evaluate(Vector3 point);
    }
}
