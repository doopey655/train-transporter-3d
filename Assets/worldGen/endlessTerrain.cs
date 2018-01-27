using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endlessTerrain : MonoBehaviour {

    public const float maxViewDst = 450;
    public Transform viewer;

    public static Vector2 viewerPosistion;
    int chunkSize;
    int chunksVisibleInViewDst;

	List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

	Dictionary<Vector2, TerrainChunk> terrainChunkDictonDictionary = new Dictionary<Vector2, TerrainChunk>();


	void Start () {
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
					terrainChunkDictonDictionary.Add (viewedChunckCoord, new TerrainChunk(viewedChunckCoord,chunkSize,transform));
				
				}

			}
        }

    }
	public class TerrainChunk{

		GameObject meshObject;
		Vector2 posistion;
		Bounds bounds;


		public TerrainChunk(Vector2 coord, int size, Transform parent)
		{
			posistion = coord * size;
			Vector3 posistionV3 = new Vector3(posistion.x,0,posistion.y);
			bounds = new Bounds(posistion, Vector2.one * size);
			
			meshObject = GameObject.CreatePrimitive((PrimitiveType.Plane));
			meshObject.transform.position = posistionV3;
			meshObject.transform.localScale = Vector3.one * size /10f;
			meshObject.transform.parent = parent;
			SetVisible(false);

		}

		public void UpdateTerrainChunk() {
			float viewerDstFromNearestEdge = Mathf.Sqrt (bounds.SqrDistance (viewerPosistion));
			bool visible = viewerDstFromNearestEdge <= maxViewDst;
			SetVisible (visible);
		}

		public void SetVisible(bool visible)
		{
			meshObject.SetActive (visible);
		}
		public bool IsVisible(){
			return meshObject.activeSelf;
		}
	}
}
