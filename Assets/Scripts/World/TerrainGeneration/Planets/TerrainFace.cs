using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Planets
{
    public class TerrainFace
    {
        ShapeGenerator shapeGenerator;
        Mesh mesh;

        int resolution; // How detailed it needs to be
        Vector3 localUp; // Which way it's facing
        Vector3 axisA;
        Vector3 axisB;

        public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
        {
            this.shapeGenerator = shapeGenerator;
            this.mesh = mesh;
            this.resolution = resolution;
            this.localUp = localUp;

            axisA = new Vector3(localUp.y, localUp.z, localUp.x); // swap around the coordinates of the localUp we've been given
            axisB = Vector3.Cross(localUp, axisA); // find axis perpendicular to axisA and localUp
        }

        public void ConstructMesh()
        {
            Vector3[] vertices = new Vector3[resolution * resolution];
            int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
            int triIndex = 0;
            Vector2[] uv = mesh.uv; 

            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    int i = x + y * resolution;
                    Vector2 percent = new Vector2(x, y) / (resolution - 1); // How close to complete these loops
                    Vector3 pointOnUnitCube = localUp + ((percent.x - .5f) * 2 * axisA) + ((percent.y - .5f) * 2 * axisB);
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized; // Every vertice should have same distance from center of sphere

                    vertices[i] = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);

                    // Create triangles as long as they're not on the right or bottom edge
                    bool notRightEdge = x != resolution - 1;
                    bool notBottomEdge = y != resolution - 1;

                    if (notRightEdge && notBottomEdge)
                    {
                        // First triangle
                        triangles[triIndex] = i;
                        triangles[triIndex + 1] = i + resolution + 1;
                        triangles[triIndex + 2] = i + resolution;

                        // Second triangle
                        triangles[triIndex + 3] = i;
                        triangles[triIndex + 4] = i + 1;
                        triangles[triIndex + 5] = i + resolution + 1;

                        triIndex += 6;
                    }
                }
            }
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.uv = uv;
        }

        public void UpdateUVs(ColorGenerator colorGenerator) {
            Vector2[] uv = new Vector2[resolution * resolution];

            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    int i = x + y * resolution;
                    Vector2 percent = new Vector2(x, y) / (resolution - 1); // How close to complete these loops
                    Vector3 pointOnUnitCube = localUp + ((percent.x - .5f) * 2 * axisA) + ((percent.y - .5f) * 2 * axisB);
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized; // Every vertice should have same distance from center of sphere

                    uv[i] = new Vector2(colorGenerator.BiomePercentFromPoint(pointOnUnitSphere), 0);
                }
            }

            mesh.uv = uv;
        }
    }
}
