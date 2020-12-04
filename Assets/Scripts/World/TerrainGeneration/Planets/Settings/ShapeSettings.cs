using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Planets
{
    [CreateAssetMenu()]
    public class ShapeSettings : ScriptableObject
    {
        public float planetRadius = 1;
        public NoiseLayer[] noiseLayers;
        
        [System.Serializable]
        public class NoiseLayer
        {
            public NoiseSettings noiseSettings;
            public bool useFirstLayerAsMask;
            public bool enabled = true;
        }
    }
}
