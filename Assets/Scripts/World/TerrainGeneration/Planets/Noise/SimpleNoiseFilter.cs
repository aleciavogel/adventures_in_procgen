using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Planets
{
    public class SimpleNoiseFilter : INoiseFilter
    {
        NoiseSettings.SimpleNoiseSettings settings;
        FastNoiseLite noise = new FastNoiseLite();

        public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
        {
            this.settings = settings;
        }

        public float Evaluate(Vector3 point)
        {
            Vector3 p = point + settings.centre;

            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
            noise.SetFractalType(FastNoiseLite.FractalType.FBm);
            noise.SetFrequency(settings.baseRoughness);
            noise.SetFractalOctaves(settings.numLayers);
            noise.SetFractalGain(settings.persistence); // change in amplitude
            noise.SetFractalLacunarity(settings.roughness); // change in frequency

            float noiseValue = noise.GetNoise(p.x, p.y, p.z);
            noiseValue = (noiseValue + 1) * .5f; // Keeps value between 0 and 1
            noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
            noiseValue *= settings.strength;

            return noiseValue;
        }
    }
}
