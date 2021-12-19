using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3WaveField : MonoBehaviour
{
    float scale = 10f;
    float speed = 0.65f;
    float noiseStrength = 1f;
    float noiseWalk = 1f;

    private Vector3[] baseHeight;

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        Mesh waveMesh = GetComponent<MeshFilter>().mesh;

        if (baseHeight == null)
            baseHeight = waveMesh.vertices;

        Vector3[] vertices = new Vector3[baseHeight.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];
            vertex.y += Mathf.Sin(Time.time * speed + baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale;
            vertex.y += Mathf.PerlinNoise(baseHeight[i].x + noiseWalk, baseHeight[i].y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;
            vertices[i] = vertex;
        }
        waveMesh.vertices = vertices;
        waveMesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = waveMesh;
    }
}
