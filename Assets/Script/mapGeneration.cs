using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGeneration : MonoBehaviour {
	
	public int mapHeight;
	public int mapWidth;
	public float noiseScale;

    public bool autoUpdate;

	public void generateMap(){
		
		float[,] noiseMap = noise.generateNoiseMap (mapWidth, mapHeight, noiseScale);

		mapDisplay display = FindObjectOfType<mapDisplay> ();
		display.DrawNoiseMap (noiseMap);

	}
}
