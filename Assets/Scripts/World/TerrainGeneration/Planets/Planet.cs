﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Planets
{
    public class Planet : MonoBehaviour
    {
        [Range(2,256)]
        public int resolution = 10;
        public bool autoUpdate = true;
        public enum FaceRenderMask {
            All,
            Top,
            Bottom,
            Left,
            Right,
            Front,
            Back
        }
        public FaceRenderMask faceRenderMask;

        public ShapeSettings shapeSettings;
        public ColorSettings colorSettings;

        [HideInInspector]
        public bool shapeSettingsFoldout;
        [HideInInspector]
        public bool colorSettingsFoldout;

        ShapeGenerator shapeGenerator = new ShapeGenerator();
        ColorGenerator colorGenerator = new ColorGenerator();

        [SerializeField, HideInInspector]
        MeshFilter[] meshFilters;
        TerrainFace[] terrainFaces;

        private void Update()
        {
            transform.Rotate(Vector3.up * 15f * Time.deltaTime);
        }

        void Initialize()
        {
            shapeGenerator.UpdateSettings(shapeSettings);
            colorGenerator.UpdateSettings(colorSettings);

            if (meshFilters == null || meshFilters.Length == 0)
            {
                meshFilters = new MeshFilter[6];
            }
            
            terrainFaces = new TerrainFace[6];

            Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

            for (int i = 0; i < 6; i++)
            {
                if (meshFilters[i] == null)
                {
                    GameObject meshObj = new GameObject("mesh");
                    meshObj.transform.parent = transform;

                    meshObj.AddComponent<MeshRenderer>();
                    meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[i].sharedMesh = new Mesh();
                }
                // Use our special shader
                meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;

                terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);

                // Only render the faces selected in the inspector GUI
                bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
                meshFilters[i].gameObject.SetActive(renderFace);
            }
        }

        public void GeneratePlanet()
        {
            Initialize();
            GenerateMesh();
            GenerateColors();
        }

        public void OnShapeSettingsUpdated()
        {
            if (autoUpdate)
            {
                Initialize();
                GenerateMesh();
            }
        }

        public void OnColorSettingsUpdated()
        {
            if (autoUpdate)
            {
                Initialize();
                GenerateColors();
            }
        }

        void GenerateMesh()
        {
            for (int i = 0; i < 6; i++)
            {
                if (meshFilters[i].gameObject.activeSelf) {
                    terrainFaces[i].ConstructMesh();
                }
            }

            colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
        }

        void GenerateColors()
        {
            colorGenerator.UpdateColors();

            for (int i = 0; i < 6; i++)
            {
                if (meshFilters[i].gameObject.activeSelf)
                {
                    terrainFaces[i].UpdateUVs(colorGenerator);
                }
            }
        }
    }
}
