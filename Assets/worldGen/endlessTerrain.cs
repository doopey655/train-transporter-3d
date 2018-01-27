using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endlessTerrain : MonoBehaviour {

    public const float maxViewDst = 450;
    public Transform viewer;
	public Material mapMaterial;

    public static Vector2 viewerPosistion;
	static mapGeneration mapGeneration;
    int chunkSize;
    int chunksVisibleInViewDst;

	Dictionary<Vector2, TerrainChunk> terrainChunkDictonDictionary = new Dictionary<Vector2, TerrainChunk>();
	List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

	void Start () {
		mapGeneration = FindObjectOfType<mapGeneration> ();
        chunkSize = mapGeneration.mapChunkSize - 1;
        chunksVisibleInViewDst =  Mathf.RoundToInt(maxViewDst / chunkSize);

	}

	void Update(){
		viewerPosistion = new Vector2 (viewer.position.x, viewer.position.z);
		UpdateVisableCunks ();
	}


    void UpdateVisableCunks()
    {

		for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++) 
		{
			terrainChunksVisibleLastUpdate [i].SetVisible (false);
		}

		terrainChunksVisibleLastUpdate.Clear ();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosistion.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosistion.y / chunkSize);

        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        {
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++) 
			{
				Vector2 viewedChunckCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset); 
				if (terrainChunkDictonDictionary.ContainsKey (viewedChunckCoord)) {
					terrainChunkDictonDictionary [viewedChunckCoord].UpdateTerrainChunk ();
					if (terrainChunkDictonDictionary [viewedChunckCoord].IsVisible()) {
						terrainChunksVisibleLastUpdate.Add (terrainChunkDictonDictionary [viewedChunckCoord]);
					}
				
				} else {
					terrainChunkDictonDictionary.Add (viewedChunckCoord, new TerrainChunk(viewedChunckCoord,chunkSize,transform, mapMaterial));
				
				}

			}
        }

    }


	public class TerrainChunk{

		GameObject meshObject;
		Vector2 posistion;
		Bounds bounds;

		MeshRenderer meshRenderer;
		MeshFilter meshFilter;


		public TerrainChunk(Vector2 coord, int size, Transform parent, Material material)
		{
			posistion = coord * size;
			Vector3 posistionV3 = new Vector3(posistion.x,0,posistion.y);
			bounds = new Bounds(posistion, Vector2.one * size);
			
			meshObject = new GameObject("Terrain Chunk");
			meshRenderer = meshObject.AddComponent<MeshRenderer>();
			meshFilter = meshObject.AddComponent<MeshFilter>();
			meshRenderer.material = material;

			meshObject.transform.position = posistionV3;
			meshObject.transform.parent = parent;

			SetVisible(false);

			mapGeneration.RequestMapData(OnMapDataRecieved);

		}

		void OnMapDataRecieved(MapData mapData)
		{
			mapGeneration.requestMeshData (mapData, OnMeshDataRecieved);
		}


		void OnMeshDataRecieved(MeshData meshData)
		{
			meshFilter.mesh = meshData.CreateMesh ();	
		}

		public void UpdateTerrainChunk() 
		{
			float viewerDstFromNearestEdge = Mathf.Sqrt (bounds.SqrDistance (viewerPosistion));
			bool visible = viewerDstFromNearestEdge <= maxViewDst;
			SetVisible (visible);
		}

		public void SetVisible(bool visible)
		{
			meshObject.SetActive (visible);
		}
		public bool IsVisible()
		{
			return meshObject.activeSelf;
		}
	}
}
