using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour {


    public const int chunkSize = 10;
    public const int textureRows = 2;
    public const int textureCols = 2;
    public const float texturePad = .1f;

    public GameObject chunk;
    private int minX = 10;
    private int maxX = 30;
    private int minY = 10;
    private int maxY = 30;
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

        //Loads Chunks
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
                    LoadChunk(j, i);
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
                    LoadChunk(j, i);
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

        //Deloads Chunks
        if (heroXChunk - minX > deloadSize)
        {
            for (int i = minX; i < heroXChunk - deloadSize; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    DeloadChunk(i, j);
                }
            }
            minX = heroXChunk - deloadSize;
        }
        if (heroYChunk - minY > deloadSize)
        {
            for (int i = minY; i < heroYChunk - deloadSize; i++)
            {
                for (int j = minX; j < maxX; j++)
                {
                    DeloadChunk(j, i);
                }
            }
            minY = heroYChunk - deloadSize;
        }
        if (maxY - heroYChunk > deloadSize)
        {
            for (int i = heroYChunk + deloadSize; i < maxY; i++)
            {
                for (int j = minX; j < maxX; j++)
                {
                    DeloadChunk(j, i);
                }
            }
            maxY = heroYChunk + deloadSize;
        }
        if (maxX - heroXChunk > deloadSize)
        {
            for (int i = heroXChunk + deloadSize; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    DeloadChunk(i, j);
                }
            }
            maxX = heroXChunk + deloadSize;
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
        for (int i = minX; i < maxX; i++)
        {
            for (int j = minY; j < maxY; j++)
            {
                chunks[i, j] = Instantiate(chunk, new Vector3(i * chunkSize, j * chunkSize), Quaternion.identity) as GameObject;
                chunks[i, j].transform.SetParent(this.gameObject.transform);
                chunks[i, j].GetComponent<TileMapChunk>().SetXandY(i * chunkSize, j * chunkSize);
            }
        }
    }
    void DeloadChunk (int i, int j)
    {
        if (!(i < 0 || i >= chunks.GetLength(0) || j < 0 || j >= chunks.GetLength(1)))
        {
            Destroy(chunks[i, j]);
            //MonoBehaviour.print(i + " " + j);
            //chunks[i, j].transform.position = new Vector3(0, 0, 0);
            chunks[i, j] = null;
        }
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
