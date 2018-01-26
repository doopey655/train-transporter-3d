using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endlessTerrain : MonoBehaviour {

    public const float maxViewDst = 300;
    public Transform viewer;

    public static Vector2 viewerPosistion;
    int chunkSize;
    int chunksVisibleInViewDst;


	void Start () {
        chunkSize = mapGeneration.mapChunkSize - 1;
        chunksVisibleInViewDst =  Mathf.RoundToInt(maxViewDst / chunkSize);

	}

    void UpdateVisableCunks()
    {
        int currentChunkCoordX = Mathf.RoundToInt(viewerPosistion.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosistion.y / chunkSize);

        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        {

        }

    }
}
