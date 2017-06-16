using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour {


    public const int chunkSize = 10;
    public const int textureRows = 2;
    public const int textureCols = 2;
    public const float texturePad = .1f;

    public GameObject chunk;

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
        int xChunks = 80;
        int yChunks = 30;

        chunks = new GameObject[xChunks, yChunks];
        for (int i = 0; i < xChunks; i++)
        {
            for (int j = 0; j < yChunks; j++)
            {
                chunks[i, j] = Instantiate(chunk, new Vector3(i * chunkSize, j * chunkSize), Quaternion.identity) as GameObject;
                chunks[i, j].transform.SetParent(this.gameObject.transform);
                chunks[i, j].GetComponent<TileMapChunk>().SetXandY(i * chunkSize, j * chunkSize);
            }
        }
    }
    
}
