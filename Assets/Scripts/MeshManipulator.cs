using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManipulator : MonoBehaviour
{
	public float scale = 10.0f;
	public Color32 oceanHighColour = new Color32(0, 120, 250, 255);
	public Color32 oceanBaseColour = new Color32(0, 120, 250, 255);
	public Color32 oceanLowColour  = new Color32(0, 120, 250, 255);
	private float time = 0.0f;

	// Update is called once per frame
	void Update()
	{
		time += Time.deltaTime;
		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Color32[] colours = new Color32[vertices.Length];
		
		for (int i = 0; i < vertices.Length; i++)
		{
			float interp = Mathf.Cos(time - vertices[i].x) * Mathf.Sin(time + vertices[i].y) * (2.0f * Mathf.PerlinNoise(0.33f * time + vertices[i].x, 0.5f * time + vertices[i].y) - 1.0f);
			
			vertices[i].z = interp / scale;
			
			if (vertices[i].z > 0)
			{
				colours[i] = Color32.Lerp(oceanBaseColour, oceanHighColour, interp * interp);
			}
			else
			{
				colours[i] = Color32.Lerp(oceanLowColour, oceanBaseColour, 1.0f + interp);
			}
		}
		
		mesh.vertices = vertices;
		mesh.colors32 = colours;
	}
}
