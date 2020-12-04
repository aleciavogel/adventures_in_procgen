using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Planets
{
    public class RidgedNoiseFilter : INoiseFilter
    {
        NoiseSettings.RidgedNoiseSettings settings;
        FastNoiseLite noise = new FastNoiseLite();

        public RidgedNoiseFilter(NoiseSettings.RidgedNoiseSettings settings)
        {
            this.settings = settings;
        }

        public float Evaluate(Vector3 point)
        {
            Vector3 p = point + settings.centre;

            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
            noise.SetFractalType(FastNoiseLite.FractalType.Ridged);
            noise.SetFrequency(settings.baseRoughness);
            noise.SetFractalOctaves(settings.numLayers);
            noise.SetFractalGain(settings.persistence); // change in amplitude
            noise.SetFractalLacunarity(settings.roughness); // change in frequency
            noise.SetFractalWeightedStrength(settings.weightMultiplier);

            float noiseValue = noise.GetNoise(p.x, p.y, p.z);
            noiseValue = (noiseValue + 1) * .5f; // Keeps value between 0 and 1
            noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
            noiseValue *= settings.strength;

            return noiseValue;
        }
    }
}