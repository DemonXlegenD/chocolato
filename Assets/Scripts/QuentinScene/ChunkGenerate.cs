using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerate : MonoBehaviour
{
    [SerializeField, Range(1,20)] private int maxObstacles;

    static List<Chunk> terrainChunksVisibleLastUpdate = new List<Chunk>();
    public Transform viewers;

    public static Vector2 viewersPosition;
    public static Vector2 viewersPositionOld;

    int chunksVisibleInViewDst;
    Collection<Vector2, Chunk> terrainChunkCollection = new Collection<Vector2, Chunk>();
    const float viewerMoveThresholdForChunkUpdate = 12f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;
    public float maxViewDst;

    [SerializeField] int chunkSize;
    [SerializeField] List<GameObject> prefabsChunkList = new List<GameObject>();

    static ObstacleSpawner ObstacleSpawner;

    void Start()
    {
        ObstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
        viewersPosition = new Vector2();
        viewersPositionOld = new Vector2();
        UpdateVisibleChunks();
    }

    // Update is called once per frame
    void Update()
    {
        viewersPosition = new Vector2(viewers.position.x, viewers.position.z);

        if ((viewersPositionOld - viewersPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
        {
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
                    int randomNb = Random.Range(0, prefabsChunkList.Count);
                    Chunk newT = new Chunk(viewedChunkCoord, chunkSize, prefabsChunkList[randomNb], maxViewDst, transform, maxObstacles);
                    terrainChunkCollection.AddItem(viewedChunkCoord, newT);
                }
            }
        }

    }

    public class Chunk
    {
        List<GameObject> obstacles = new List<GameObject>();

        GameObject meshObject;
        readonly int size;
        Vector2 coord;
        Vector2 positionV2;
        Vector3 positionV3;
        public Bounds bounds;
        private float maxViewDst;
        private int maxObstaclesNumber;

        public Chunk(Vector2 coord, int size, GameObject chunkGameobject_prefab, float _maxViewDst, Transform parent, int _maxObstaclesNumber)
        {
            meshObject = Instantiate(chunkGameobject_prefab);
            this.size = size;
            this.coord = coord;
            this.positionV2 = coord * size;
            this.positionV3 = new Vector3(positionV2.x, 0, positionV2.y);
            bounds = new Bounds(positionV2, Vector2.one * size);
            this.maxViewDst = _maxViewDst;
            this.maxObstaclesNumber = _maxObstaclesNumber;
            meshObject.name = meshObject.name + coord.ToString();
            meshObject.transform.parent = parent;
            meshObject.transform.position = this.positionV3;    
            meshObject.transform.localScale = new Vector3(size / 10f, 1, size / 10f);
            SetVisible(false);
            this.UpdateTerrainChunk();
            this.GenerateObstacles();
        }

        public void UpdateTerrainChunk()
        {

            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewersPosition));
     
            bool visible = viewerDstFromNearestEdge <= maxViewDst
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
                int numberObstacles = Random.Range(Mathf.RoundToInt(maxObstaclesNumber / 2) + 1, maxObstaclesNumber);
                Vector2 minChunk = (coord - (Vector2.one / 2)) * this.size;
                Vector2 maxChunk = (coord + (Vector2.one / 2)) * this.size;
                for (int i = 0; i < numberObstacles; i++)
                {
                    GameObject newObstacle = ObstacleSpawner.SpawnObstacles(minChunk, maxChunk, meshObject);
                    if (newObstacle != null) obstacles.Add(newObstacle);
                }
            }
        }
    }

}
