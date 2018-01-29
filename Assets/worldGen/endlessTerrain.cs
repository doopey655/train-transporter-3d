using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endlessTerrain : MonoBehaviour
{

    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

    public Transform viewer;
    public Material mapMaterial;
    public LODInfo[] detailLevels;
    public static float maxViewDst;

    public static Vector2 viewerPosistion;
    Vector2 viewerPosistionOld;
    static mapGeneration mapGeneration;
    int chunkSize;
    int chunksVisibleInViewDst;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictonDictionary = new Dictionary<Vector2, TerrainChunk>();
    static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    void Start()
    {

        maxViewDst = detailLevels[detailLevels.Length - 1].visibleDistanceThreshold;
        mapGeneration = FindObjectOfType<mapGeneration>();
        chunkSize = mapGeneration.mapChunkSize - 1;
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
        UpdateVisableCunks();

    }

    void Update()
    {
        viewerPosistion = new Vector2(viewer.position.x, viewer.position.z);

        if ((viewerPosistionOld - viewerPosistion).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
        {
            viewerPosistionOld = viewerPosistion;
            UpdateVisableCunks();
        }

    }


    void UpdateVisableCunks()
    {

        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
        {
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }

        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosistion.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosistion.y / chunkSize);

        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                Vector2 viewedChunckCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
                if (terrainChunkDictonDictionary.ContainsKey(viewedChunckCoord))
                {
                    terrainChunkDictonDictionary[viewedChunckCoord].UpdateTerrainChunk();
                }
                else
                {
                    terrainChunkDictonDictionary.Add(viewedChunckCoord, new TerrainChunk(viewedChunckCoord, chunkSize, detailLevels, transform, mapMaterial));

                }

            }
        }

    }


    public class TerrainChunk
    {

        GameObject meshObject;
        Vector2 posistion;
        Bounds bounds;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        LODInfo[] detailLevels;
        LODMesh[] lodMeshes;

        MapData mapData;
        bool mapDataRecieved;
        int previousLODIndex = -1;


        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material)
        {
            this.detailLevels = detailLevels;
            posistion = coord * size;
            Vector3 posistionV3 = new Vector3(posistion.x, 0, posistion.y);
            bounds = new Bounds(posistion, Vector2.one * size);

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer.material = material;

            meshObject.transform.position = posistionV3;
            meshObject.transform.parent = parent;

            SetVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];

            for (int i = 0; i < detailLevels.Length; i++)
            {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
            }

            mapGeneration.RequestMapData(posistion, OnMapDataRecieved);

        }

        void OnMapDataRecieved(MapData mapData)
        {
            this.mapData = mapData;
            mapDataRecieved = true;

            Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colourMap, mapGeneration.mapChunkSize, mapGeneration.mapChunkSize);
            meshRenderer.material.mainTexture = texture;

            UpdateTerrainChunk();
        }


        public void UpdateTerrainChunk()
        {
            if (mapDataRecieved)
            {
                float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosistion));
                bool visible = viewerDstFromNearestEdge <= maxViewDst;

                if (visible)
                {
                    int lodInex = 0;

                    for (int i = 0; i < detailLevels.Length - 1; i++)
                    {
                        if (viewerDstFromNearestEdge > detailLevels[i].visibleDistanceThreshold)
                        {
                            lodInex = i + 1;
                        }
                        else
                        {
                            break;
                        }

                    }
                    if (lodInex != previousLODIndex)
                    {
                        LODMesh lodMesh = lodMeshes[lodInex];
                        if (lodMesh.hasMesh)
                        {
                            meshFilter.mesh = lodMesh.mesh;

                        }
                        else if (!lodMesh.hasRequestedMesh)
                        {

                            lodMesh.RequestMesh(mapData);

                        }
                    }
                    terrainChunksVisibleLastUpdate.Add(this);
                }

                SetVisible(visible);
            }
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }
        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }
    }

    class LODMesh
    {

        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        System.Action updateCallback;

        public LODMesh(int lod, System.Action updateCallback)
        {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }
        void OnMeshDataRecieved(MeshData meshData)
        {
            mesh = meshData.CreateMesh();
            hasMesh = true;

            updateCallback();
        }

        public void RequestMesh(MapData mapData)
        {
            hasRequestedMesh = true;
            mapGeneration.RequestMeshData(mapData, lod, OnMeshDataRecieved);

        }
    }

    [System.Serializable]
    public struct LODInfo
    {
        public int lod;
        public float visibleDistanceThreshold;

    }

}