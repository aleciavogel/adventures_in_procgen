using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Planets
{
    public class ColorGenerator
    {
        ColorSettings settings;
        Texture2D texture;
        const int textureResolution = 50;

        public void UpdateSettings(ColorSettings settings)
        {
            this.settings = settings;

            if (texture == null || texture.height != settings.biomeColorSettings.biomes.Length)
            {
                texture = new Texture2D(textureResolution, settings.biomeColorSettings.biomes.Length, TextureFormat.RGBA32, false);
            }
        }

        public void UpdateElevation(MinMax elevationMinMax)
        {
            settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
        }

        public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
        {
            float heightFromPercent = (pointOnUnitSphere.y + 1) / 2f;
            float biomeIndex = 0;
            int numBiomes = settings.biomeColorSettings.biomes.Length;

            for (int i = 0; i < numBiomes; i++)
            {
                if (settings.biomeColorSettings.biomes[i].startHeight < heightFromPercent)
                {
                    biomeIndex = i;
                }
                else
                {
                    break;
                }
            }

            return biomeIndex / Mathf.Max(1, numBiomes - 1);
        }

        public void UpdateColors()
        {
            Color[] colors = new Color[texture.width * texture.height];

            int colorIndex = 0;

            foreach(var biome in settings.biomeColorSettings.biomes)
            {
                for (int i = 0; i < textureResolution; i++)
                {
                    Color gradientCol = biome.gradient.Evaluate(i / (textureResolution - 1f));
                    Color tintColor = biome.tint;
                    colors[colorIndex] = gradientCol * (1 - biome.tintPercent) + tintColor * biome.tintPercent;
                    colorIndex++;
                }
            }

            texture.SetPixels(colors);
            texture.Apply();
            settings.planetMaterial.SetTexture("_texture", texture);
        }
    }
}

