using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Wave;

public class WaveField : MonoBehaviour
{
	private List<Wave> waves;
	private float lastTime = 0.0f;
	
	private Mesh target;
	
	public float waveVelocity = 0.01f;
	public float waveScale    = 0.5f;
	public float energyScale  = 0.75f;
	
	public int count;
	public int size;
	
	public Color32 oceanLow  = new Color32(0, 72, 150, 255);
	public Color32 oceanBase = new Color32(0, 120, 240, 255);
	public Color32 oceanHigh = new Color32(75, 150, 255, 255);
	
    public float baseWaveScale = 10f;
    public float baseWaveSpeed = 0.65f;
    public float waveNoiseStrength = 1f;
    public float waveNoiseWalk = 1f;
	
	public static WaveField water;
	
	// Use this for initialization
	void Start()
	{
		waves = new List<Wave>();

		generateMesh();
	}
	
	void Awake()
	{
		water = this;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Time.time - lastTime > 0.02f)
		{
			updateWaves();
		
			lastTime = Time.time;
		}
		
		renderWaves();
	}
	
	public void addWave(Vector2 position, Vector2 direction, float energy)
	{
		Wave wave = new Wave(position, direction.normalized * waveVelocity, energyScale * energy);
		
		waves.Add(wave);
	}
	
	private void updateWaves()
	{
		float dt = Time.deltaTime;
		
		for (int i = waves.Count - 1; i >= 0; i--)
		{
			waves[i].step(dt);
			
			if (waveInBounds(waves[i], new Vector2(-size / 2, -size / 2), new Vector2(size / 2, size / 2)))
			{
				waves.RemoveAt(i);
			}
		}
	}
	
	private void renderWaves()
	{
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Color32[] colors = new Color32[vertices.Length];
		
		for (int i = 0; i < count; i++)
		{
			for (int j = 0; j < count; j++)
			{
				int index = i * count + j;
				
				vertices[index].z =		Mathf.Sin(Time.time * baseWaveSpeed + (vertices[index].x + vertices[index].y + vertices[index].z) * 7f / size) * baseWaveScale +
										Mathf.PerlinNoise(vertices[index].x * 5.0f / size + waveNoiseWalk, vertices[index].y * 5.0f / size + Mathf.Sin(Time.time * 0.1f)) * waveNoiseStrength;

				foreach (Wave wave in waves)
				{
					vertices[index].z += wave.influence(new Vector2(vertices[index].x, vertices[index].y)) * waveScale;
				}
				
				if (vertices[index].z >= 0)
				{
					colors[index] = Color32.Lerp(oceanBase, oceanHigh, vertices[index].z / 10.0f);
				}
				else if (vertices[index].z < 0)
				{
					colors[index] = Color32.Lerp(oceanLow, oceanBase, (Mathf.Max(vertices[index].z, -1f) + 1f) / 10.0f);
				}
			}
		}
		
		mesh.vertices = vertices;
		mesh.colors32 = colors;
		
		mesh.RecalculateNormals();
		
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}
	
	private static bool waveInBounds(Wave wave, Vector3 lower, Vector3 upper)
	{
		return (wave.current.x < lower.x || wave.current.x > upper.x) || (wave.current.y < lower.y || wave.current.y > upper.y);
	}
	
	private void generateMesh()
	{
		Vector3[] vertices = new Vector3[count * count];
		int[] triangles = new int[6 * (count - 1) * (count - 1)];
		
		float sizePerStep = ((float) size) / ((float)(count - 1));
		
		for (int y = 0; y < count; y++)
		{
			for (int x = 0; x < count; x++)
			{
				int index = y * count + x;
				
				vertices[index] = new Vector3((x - (1 + count / 2)) * sizePerStep, (y - (1 + count / 2)) * sizePerStep, 0.0f);
			}
		}
		
		int i = 0;
		
		for (int y = 0; y < count - 1; y++)
		{
			for (int x = 0; x < count - 1; x++)
			{
				// First triangle
				triangles[i++] = y * count + x;				// TL
				triangles[i++] = y * count + x + 1;			// TR
				triangles[i++] = (y + 1) * count + x;		// BL
				
				// Second triangle
				triangles[i++] = y * count + x + 1;			// TR
				triangles[i++] = (y + 1) * count + x + 1;	// BR
				triangles[i++] = (y + 1) * count + x;		// BL
			}
		}
		
		target = new Mesh();
		
		target.vertices = vertices;
		target.triangles = triangles;
		
		GetComponent<MeshFilter>().mesh = target;
		GetComponent<MeshCollider>().sharedMesh = target;
	}
}











