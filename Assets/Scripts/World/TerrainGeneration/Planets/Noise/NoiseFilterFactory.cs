﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Planets
{
    public static class NoiseFilterFactory
    {
        public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
        {
            switch (settings.filterType)
            {
                case NoiseSettings.FilterType.Simple:
                    return new SimpleNoiseFilter(settings.simpleNoiseSettings);
                case NoiseSettings.FilterType.Ridged:
                    return new RidgedNoiseFilter(settings.ridgedNoiseSettings);
            }

            return null;
        }
    }
}
