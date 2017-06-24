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
    public  int loadSize = 10;
    public  int deloadSize = 15;
    public int multiplier = 1;

    public int[,] map;

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
        if (loadSize < 100)
        {
            loadSize++;
            deloadSize++;
        }
        

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
        map = new int[1000*multiplier, 400*multiplier];

        float xOffset = Random.Range(0, 100000);
        float yOffset = Random.Range(0, 100000);

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                /*map[i, j] = (int) (Mathf.PerlinNoise(i * .2f + xOffset, j * .2f + yOffset) * 3);
                if (map[i, j] == 2)
                {
                    map[i, j] = 1;
                }*/
                map[i, j] = 0;
                //map[i, j] = (int)(Random.Range(0, 4));
            }
        }

        Vector2 start = new Vector2(50*multiplier, 250 * multiplier);
        Vector2 end = new Vector2(950 * multiplier, Random.Range(150 * multiplier, 300 * multiplier));
        Vector2[] points = new Vector2[20];
        int counter = 1;
        for (int i = 0; i < 1000 * multiplier; i += 334 * multiplier)
        {
            for (int j = 0; j < 400 * multiplier; j += 200 * multiplier)
            {
                for (int k = 0; k < 3; k++)
                {
                    points[counter] = new Vector2(Random.Range(i, i + 333 * multiplier), Random.Range(j, j + 200 * multiplier));
                    counter++;
                }
            }
        }

        points[0] = start;
        points[19] = end;

        for (int i = 0; i < 4; i++)
        {
            int ind = Random.Range(0, 20);
            FillRect((int) points[ind].x, (int) points[ind].y, Random.Range(30 * multiplier, 90 * multiplier), Random.Range(30 * multiplier, 60 * multiplier));
        }

        for (int i = 0; i < 1; i++)
        {
            int ind = Random.Range(0, 20);
            FillCircle(((int)points[ind].x), (int)(points[ind].y), Random.Range(60 * multiplier, 100 * multiplier));
        }

        bool[,] adjacency = new bool[20, 20];
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                adjacency[i, j] = false;
            }
        }
        for (int i = 0; i < 20; i++)
        {
            float mult = Random.Range(2, 4);
            int pathsCreated = 0;
            Vector2 origin = points[i];
            while ((float)pathsCreated < mult)
            {

                for (int j = 0; j < 20; j++)
                {
                    if (i != j)
                    {
                        Vector2 dest = points[j];
                        float dist = SquareDistance(origin.x, origin.y, dest.x, dest.y);
                        if (Random.Range(0, dist * dist / 100000 / (Mathf.Pow(multiplier, 4))  * Mathf.Abs(dest.y - origin.y) /  Mathf.Abs(dest.x - origin.x)) < mult && !adjacency[i, j])
                        {
                            MonoBehaviour.print(origin.x + " " + origin.y + " " + dest.x + " " + dest.y);
                            adjacency[i, j] = true;
                            adjacency[j, i] = true;
                            pathsCreated++;
                            FillPath((int)origin.x, (int)origin.y, (int)dest.x, (int)dest.y, 3 * multiplier);
                        }
                    }
                }
                
            }
            
        }

        //FillPath((int)start.x, (int)start.y, (int)end.x, (int)end.y, 5);
        GameObject.Find("Hero").transform.position = start;
        
        minX = (int)start.x/chunkSize - loadSize;
        maxX = (int)start.x/chunkSize + loadSize;
        minY = (int)start.y/chunkSize - loadSize;
        maxY = (int)start.y/chunkSize + loadSize;
    }

    float SquareDistance(float x1, float y1, float x2, float y2)
    {
        return (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
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
                LoadChunk(i, j);
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

    void FillCircle(int i, int j, int size)
    {
        for (int x = i - size; x < i + size; x++)
        {
            for (int y = j - size; y < j + size; y++)
            {
                if ((x - i) * (x - i) + (y - j) * (y - j) < size * size)
                {
                    SetInMap(x, y, 1);
                }
            }
        }
    }

    void FillRect(int i, int j, int xSize, int ySize)
    {
        for (int x = i - xSize; x < i + xSize; x++)
        {
            for (int y = j - ySize; y < j + ySize; y++)
            {
                SetInMap(x, y, 1);
            }
        }
    }

    void FillPath(int x1, int y1, int x2, int y2, int size)
    {
        int steps = Mathf.Max(Mathf.Abs(x2 - x1), Mathf.Abs(y2 - y1));

        Vector2 offsetOffset = new Vector2(Random.Range(0, 100000), Random.Range(0, 100000));
        Vector2 sizeOffset = new Vector2(Random.Range(0, 100000), Random.Range(0, 100000));

        Vector2 diff = new Vector2(x2 - x1, y2 - y1);
        Vector2 offset = new Vector2((y2 - y1) * .5f, (x2 - x1) * .5f);

        for (float frac = 0f; frac < 1f; frac += 1f / (float)steps)
        {
            float offsetPerlin = (-1 + 2 * (Mathf.PerlinNoise(offsetOffset.x + frac * 2, offsetOffset.y))) * Mathf.Min(frac, 1 - frac);
            int x = (int) (x1 + (diff * frac + offset * offsetPerlin).x);
            int y = (int) (y1 + (diff * frac + offset * offsetPerlin).y);
            FillCircle(x, y, (int) (0.3 * size + 1.5 * size * Mathf.PerlinNoise(sizeOffset.x + frac * 10, sizeOffset.y)));
        }
    }

    void SetInMap(int i, int j, int val)
    {
        if (i < 0 || i >= map.GetLength(0) || j < 0 || j >= map.GetLength(1))
        {
            return;
        }
        map[i, j] = val;
    }
}
