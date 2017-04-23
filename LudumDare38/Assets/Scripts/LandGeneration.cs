using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGeneration : MonoBehaviour {

    public MeshFilter meshFilter;
    public bool collided = false;

    private float maxSize = 5;
    private float minSize = 2f;
    private float maxNoise = 0.5f;
    private float maxIntervalLength = 5;
    private float minIntervalLength = 2;

    private float depth = -2f;


    private void Start()
    {
        GenerateMesh();
        meshFilter.transform.localPosition = new Vector3(-meshFilter.mesh.bounds.extents.x, - meshFilter.mesh.bounds.extents.y, 0);
    }

    /// <summary>
    /// Generate a mesh for this land.
    /// </summary>
	private void GenerateMesh()
    {
        Vector3[] vertices = GenerateVertices();
        int[] triangles = GenerateTriangles(vertices);

        vertices = GenerateDepth(vertices);
        triangles = GenerateDepthTriangles(vertices, triangles);

        Vector3[] normals = new Vector3[vertices.Length];
        for(int n = 0; n < normals.Length; n++)
        {
            normals[n] = -Vector3.forward;
        }

        Mesh mesh = meshFilter.mesh;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.RecalculateNormals();
        mesh.normals = normals;

        meshFilter.GetComponent<MeshCollider>().sharedMesh = mesh;

    }

    private Vector3[] GenerateVertices()
    {
        float maxWidth = Random.Range(minSize, maxSize);
        float maxHeight = Random.Range(maxSize, maxSize);


        List<Vector3> vertices = new List<Vector3>();

        //Bottom left corner:
        Vector3 currentVertex = new Vector3(0, 0, 0);

        //Left edge:
        while (currentVertex.y < maxHeight)
        {
            float intervalLength = Random.Range(minIntervalLength, maxIntervalLength);
            currentVertex = GenerateVertex(0, Mathf.Min(currentVertex.y + intervalLength, maxHeight));
            vertices.Add(currentVertex);

        }
        
        //Top edge:
        while(currentVertex.x < maxWidth)
        {
            float intervalLength = Random.Range(minIntervalLength, maxIntervalLength);
            currentVertex = GenerateVertex(Mathf.Min(currentVertex.x + intervalLength, maxWidth), maxHeight);
            vertices.Add(currentVertex);
        }

        //Right edge:
        while (currentVertex.y > 0)
        {
            float intervalLength = Random.Range(minIntervalLength, maxIntervalLength);
            currentVertex = GenerateVertex(maxWidth, Mathf.Max(currentVertex.y - intervalLength));
            vertices.Add(currentVertex);

        }

        //Bottom edge:
        while (currentVertex.x > 0)
        {
            float intervalLength = Random.Range(minIntervalLength, maxIntervalLength);
            currentVertex = GenerateVertex(Mathf.Min(currentVertex.x - intervalLength, 0), 0);
            vertices.Add(currentVertex);
        }
        return vertices.ToArray();
    }

    private Vector3 GenerateVertex(float x, float y)
    {
        float noise = Random.Range(-maxNoise, maxNoise);

        return new Vector3(x + noise, y + noise, 0);
    }

    private int[] GenerateTriangles(Vector3[] vertices)
    {
        int[] triangles = new int[(vertices.Length - 2) * 3];

        int lastTriNum = 1;
        for (int t = 0; t < triangles.Length; t++)
        {
            if (t % 3 == 0)
            {
                triangles[t] = 0;
            }
            else if (t % 3 == 1)
            {
                triangles[t] = lastTriNum;
            }
            else
            {
                triangles[t] = lastTriNum + 1;
                lastTriNum++;
            }
        }
        return triangles;
    }

    private Vector3[] GenerateDepth(Vector3[] vertices)
    {
        Vector3[] depthVertices = new Vector3[vertices.Length];
        for(int v = 0; v < depthVertices.Length; v++)
        {
            depthVertices[v] = new Vector3(vertices[v].x, vertices[v].y, vertices[v].z - depth);
        }

        Vector3[] allVertices = new Vector3[vertices.Length + depthVertices.Length];
        vertices.CopyTo(allVertices, 0);
        depthVertices.CopyTo(allVertices, vertices.Length);

        return allVertices;
    }

    private int[] GenerateDepthTriangles(Vector3[] vertices, int[] triangles)
    {
        int[] depthTriangles = new int[vertices.Length*2 * 3];

        for (int v = 0; v < vertices.Length/2; v++)
        {
            depthTriangles[v * 6] = v;
            depthTriangles[(v * 6) + 1] = v + vertices.Length / 2;
            depthTriangles[(v * 6) + 2] = v + 1;

            depthTriangles[(v * 6) + 3] = v;
            depthTriangles[(v * 6) + 4] = v + (vertices.Length / 2) - 1;

            depthTriangles[(v * 6) + 5] = v + vertices.Length / 2;
        }


        int[] allTriangles = new int[triangles.Length + depthTriangles.Length];
        triangles.CopyTo(allTriangles, 0);
        depthTriangles.CopyTo(allTriangles, triangles.Length);
        return allTriangles;
    }


}
