using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour {


    public const int chunkSize = 10;
    public const int textureRows = 2;
    public const int textureCols = 2;
    public const float texturePad = .1f;

    public GameObject chunk;
    private int minX;
    private int maxX;
    private int minY;
    private int maxY;
    public const int loadSize = 10;
    public const int deloadSize = 15;

    public int[,] map = {{1, 2, 1, 1, 1}, {1, 3, 0, 0, 1}, {1, 0, 2, 0, 1}, {1, 2, 0, 1, 1}, {1, 1, 3, 1, 3}};

    GameObject[,] chunks;


	// Use this for initialization
	void Start () {

        BuildMap();
        BuildChunks();
	}

    // Update is called once per frame
    void Update()
    {
        float heroX = GameObject.Find("Hero").transform.position.x;
        float heroY = GameObject.Find("Hero").transform.position.y;
        int heroXChunk = (int)(heroX / chunkSize);
        int heroYChunk = (int)(heroY / chunkSize);
        if(heroXChunk - minX < loadSize)
        {
            for(int i = heroXChunk-loadSize; i < minX; i++)
            {
                for(int j = minY; j < maxY; j ++)
                {
                    LoadChunk(i, j);
                }
            }
            minX = heroXChunk - loadSize;
        }
        if (heroYChunk - minY < loadSize)
        {
            for (int i = heroYChunk - loadSize; i < minY; i++)
            {
                for (int j = minX; j < maxX; j++)
                {
                    LoadChunk(i, j);
                }
            }
            minY = heroYChunk - loadSize;
        }
        if (maxY - heroYChunk < loadSize)
        {
            for (int i = maxY; i < heroYChunk + loadSize; i++)
            {
                for (int j = minX; j < maxX; j++)
                {
                    LoadChunk(i, j);
                }
            }
            maxY = heroYChunk + loadSize;
        }
        if (maxX - heroXChunk < loadSize)
        {
            for (int i = maxX; i < heroXChunk + loadSize; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    LoadChunk(i, j);
                }
            }
            maxX = heroXChunk + loadSize;
        }
    }

    void BuildMap()
    {
        map = new int[2000, 1500];

        float xOffset = Random.Range(0, 100000);
        float yOffset = Random.Range(0, 100000);

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                //MonoBehaviour.print (Mathf.PerlinNoise (i / 100f, j / 100f));
                map[i, j] = (int) (Mathf.PerlinNoise(i * .2f + xOffset, j * .2f + yOffset) * 3);
                if (map[i, j] == 2)
                {
                    map[i, j] = 1;
                }
                //map[i, j] = 0;
                //map[i, j] = (int)(Random.Range(0, 4));
            }
        }


    }
	
	void BuildChunks()
    {
        int xChunks = map.GetLength(0)/chunkSize;
        int yChunks = map.GetLength(1)/chunkSize;

        chunks = new GameObject[xChunks, yChunks];
        /*for (int i = 0; i < xChunks; i++)
        {
            for (int j = 0; j < yChunks; j++)
            {
                chunks[i, j] = Instantiate(chunk, new Vector3(i * chunkSize, j * chunkSize), Quaternion.identity) as GameObject;
                chunks[i, j].transform.SetParent(this.gameObject.transform);
                chunks[i, j].GetComponent<TileMapChunk>().SetXandY(i * chunkSize, j * chunkSize);
            }
        }*/
    }
    void LoadChunk (int i, int j)
    {
        if (!(i < 0 || i >= chunks.GetLength(0) || j < 0 || j >= chunks.GetLength(1)))
        {
            chunks[i, j] = Instantiate(chunk, new Vector3(i * chunkSize, j * chunkSize), Quaternion.identity) as GameObject;
            chunks[i, j].transform.SetParent(this.gameObject.transform);
            chunks[i, j].GetComponent<TileMapChunk>().SetXandY(i * chunkSize, j * chunkSize);
        }
    }
}
