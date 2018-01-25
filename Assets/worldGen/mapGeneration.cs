﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGeneration : MonoBehaviour {

    public enum Drawmode {NoiseMap, ColourMap, Mesh};
    public Drawmode drawMode;
	
	public int mapHeight;
	public int mapWidth;
	public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;

    public bool autoUpdate;

    public TerrainType[] regions;

	public void generateMap(){
		
		float[,] noiseMap = noise.generateNoiseMap (mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset, regions);

        Color[] colourMap = new Color[mapWidth * mapHeight];
        for(int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];   
                for (int i = 0; i < regions.Length; i++)
                {

                if (currentHeight <= regions[i].height)
                {
                colourMap [y * mapWidth + x] = regions [i].colour;
                break;
                }

                }
            }
        }

       
        
		mapDisplay display = FindObjectOfType<mapDisplay> ();
        if(drawMode == Drawmode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
           
        }else if (drawMode == Drawmode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
        }else if (drawMode == Drawmode.Mesh)
        {
            display.drawMesh(meshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier), TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
        }
		

	}

    private void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (mapHeight < 1)
        {
            mapHeight = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
        if (noiseScale < 0)
        {
            noiseScale = 0;
        }
    }
}
[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}
