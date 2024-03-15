using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class ChunkGenerate : MonoBehaviour
{
    /*[SerializeField] private collectionChunk;*/
    const float scale = 2f;
    static List<Chunk> terrainChunksVisibleLastUpdate = new List<Chunk>();
    public Transform viewers;
    public static Vector2 viewersPosition;
    public static Vector2 viewersPositionOld;
    int chunksVisibleInViewDst;
    Collection<Vector2, Chunk> terrainChunkCollection = new Collection<Vector2, Chunk>();
    const float viewerMoveThresholdForChunkUpdate = 20f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;
    public static float maxViewDst;

    [SerializeField] int chunkSize;
    [SerializeField] GameObject chunkGameobject_prefab;
    [SerializeField] Object obstacle1_prefab;
    [SerializeField] Object obstacle2_prefab;
    void Start()
    {
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
        viewersPosition = new Vector2();
        viewersPositionOld = new Vector2();
        Chunk first = new Chunk(new Vector2(1, 0), chunkSize, chunkGameobject_prefab, obstacle1_prefab, obstacle2_prefab);
    }

    // Update is called once per frame
    void Update()
    {
        viewersPosition = new Vector2(viewers.position.x, viewers.position.z) / 2f;

        if ((viewersPositionOld - viewersPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
        {
            Debug.Log("oui");
            viewersPositionOld = viewersPosition;
            UpdateVisibleChunks();
        }
    }

    void UpdateVisibleChunks()
    {

        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
        {
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewersPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewersPosition.y / chunkSize);

        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);


                if (terrainChunkCollection.HasItem(viewedChunkCoord))
                {
                    terrainChunkCollection.GetItemBykey(viewedChunkCoord).UpdateTerrainChunk();
                }
                else
                {
                    Chunk newT = new Chunk(viewedChunkCoord, chunkSize, chunkGameobject_prefab, obstacle1_prefab, obstacle2_prefab);
                    terrainChunkCollection.AddItem(viewedChunkCoord, newT);
                }
            }
        }

    }

    public class Chunk
    {
        GameObject meshObject;
        Object obstacle1;
        Object obstacle2;
        readonly int size;
        Vector2 coord;
        Vector2 positionV2;
        Vector3 positionV3;
        public Bounds bounds;

        public Chunk(Vector2 coord, int size, GameObject chunkGameobject_prefab, Object obstacle1_prefab, Object obstacle2_prefab)
        {
            meshObject = chunkGameobject_prefab;
            obstacle1 = obstacle1_prefab;
            obstacle2 = obstacle2_prefab;
            this.size = size;
            this.coord = coord;
            this.positionV2 = coord * size;
            this.positionV3 = new Vector3(positionV2.x, 0, positionV2.y);
            bounds = new Bounds(positionV2, Vector2.one * size);




            Instantiate(meshObject);
            this.UpdateTerrainChunk();
            //this.GenerateObstacles();
        }

        public void UpdateTerrainChunk()
        {

            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewersPosition));
            float viewerDstFromNearestEdge2 = 0;
            bool visible = false;
            if (viewersPosition != null) visible = viewerDstFromNearestEdge <= maxViewDst || viewerDstFromNearestEdge2 <= maxViewDst;
            else visible = viewerDstFromNearestEdge <= maxViewDst
;
            if (visible)
            {
                terrainChunksVisibleLastUpdate.Add(this);
            }
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }

        public void GenerateObstacles()
        {
            if (coord.x != 0 || coord.y != 0)
            {
                int numberObstacles = UnityEngine.Random.Range(5, 10);
                Vector2 minChunk = (coord - (Vector2.one / 2)) * this.size;
                Vector2 maxChunk = (coord + (Vector2.one / 2)) * this.size;
                for (int i = 0; i < numberObstacles; i++)
                {
                    obstacle1.Generate(minChunk, maxChunk);
                    obstacle2.Generate(minChunk, maxChunk);
                }

            }
        }
    }

}
