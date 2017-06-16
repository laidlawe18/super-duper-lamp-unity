using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapChunk : MonoBehaviour {

    private int startX;
    private int startY;
    private int[,] map;

	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {

	}

    public void SetXandY(int x, int y)
    {
        startX = x;
        startY = y;
        map = GetComponentInParent<TileMap>().map;
        BuildCollider();
        BuildMesh();
    }

    void BuildCollider()
    {
        

        PolygonCollider2D pc = GetComponent<PolygonCollider2D>();
        int numPaths = 0;
        for (int i = 0; i < TileMap.chunkSize; i++)
        {
            for (int j = 0; j < TileMap.chunkSize; j++)
            {
                if (getInMap(i, j) == 0 && (getInMap(i + 1, j) == 1 || getInMap(i - 1, j) == 1 || getInMap(i, j + 1) == 1 || getInMap(i, j - 1) == 1))
                {
                    numPaths++;
                }
            }
        }
        pc.pathCount = numPaths;
        int pathCntr = 0;
        for (int i = 0; i < TileMap.chunkSize; i++)
        {
            for (int j = 0; j < TileMap.chunkSize; j++)
            {
                if (getInMap(i, j) == 0 && (getInMap(i + 1, j) == 1 || getInMap(i - 1, j) == 1 || getInMap(i, j + 1) == 1 || getInMap(i, j - 1) == 1))
                {
                    Vector2[] points = { new Vector2(i, j), new Vector2(i + 1, j), new Vector2(i + 1, j + 1), new Vector2(i, j + 1) };
                    pc.SetPath(pathCntr, points);
                    pathCntr++;
                }
            }
        }
    }
    void BuildMesh()
    {
        Mesh mesh = new Mesh();
        

        Vector3[] vertices = new Vector3[4 * TileMap.chunkSize * TileMap.chunkSize];
        int[] triangles = new int[6 * TileMap.chunkSize * TileMap.chunkSize];
        Vector2[] uv = new Vector2[4 * TileMap.chunkSize * TileMap.chunkSize];
        Vector3[] normals = new Vector3[4 * TileMap.chunkSize * TileMap.chunkSize];

        for (int i = 0; i < TileMap.chunkSize; i++)
        {
            for (int j = 0; j < TileMap.chunkSize; j++)
            {
                int baseInd = i * TileMap.chunkSize + j;
                vertices[baseInd * 4] = new Vector3(i, j);
                vertices[baseInd * 4 + 1] = new Vector3(i + 1, j);
                vertices[baseInd * 4 + 2] = new Vector3(i, j + 1);
                vertices[baseInd * 4 + 3] = new Vector3(i + 1, j + 1);

                normals[baseInd * 4] = Vector3.back;
                normals[baseInd * 4 + 1] = Vector3.back;
                normals[baseInd * 4 + 2] = Vector3.back;
                normals[baseInd * 4 + 3] = Vector3.back;

                triangles[baseInd * 6] = baseInd * 4;
                triangles[baseInd * 6 + 1] = baseInd * 4 + 2;
                triangles[baseInd * 6 + 2] = baseInd * 4 + 1;
                triangles[baseInd * 6 + 3] = baseInd * 4 + 1;
                triangles[baseInd * 6 + 4] = baseInd * 4 + 2;
                triangles[baseInd * 6 + 5] = baseInd * 4 + 3;

                int textureCols = TileMap.textureCols;
                int textureRows = TileMap.textureRows;
                float texturePad = TileMap.texturePad;

                int textureRow = getInMap(i, j) / textureCols;
                int textureCol = getInMap(i, j) % textureCols;
                uv[baseInd * 4] = new Vector2((float)textureCol / (float)textureCols + texturePad / textureCols, (float)textureRow / (float)textureRows + texturePad / textureRows);
                uv[baseInd * 4 + 1] = new Vector2((float)(textureCol + 1) / (float)textureCols - texturePad / textureCols, (float)textureRow / (float)textureRows + texturePad / textureRows);
                uv[baseInd * 4 + 2] = new Vector2((float)textureCol / (float)textureCols + texturePad / textureCols, (float)(textureRow + 1) / (float)textureRows - texturePad / textureRows);
                uv[baseInd * 4 + 3] = new Vector2((float)(textureCol + 1) / (float)textureCols - texturePad / textureCols, (float)(textureRow + 1) / (float)textureRows - texturePad / textureRows);
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.normals = normals;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    int getInMap(int i, int j)
    {
        i += startX;
        j += startY;
        if (i < 0 || i >= map.GetLength(0) || j < 0 || j >= map.GetLength(1))
        {
            return 1;
        }
        return map[i, j];
    }
}
