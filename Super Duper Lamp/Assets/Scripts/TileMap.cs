using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour {


	int[,] map = {{1, 2, 1, 1, 1}, {1, 3, 0, 0, 1}, {1, 0, 2, 0, 1}, {1, 2, 0, 1, 1}, {1, 1, 3, 1, 3}};

	const int textureRows = 2;
	const int textureCols = 2;
	const float texturePad = .1f;

	// Use this for initialization
	void Start () {

		map = new int[50, 50];

        float xOffset = Random.Range(0, 10000000);
        float yOffset = Random.Range(0, 10000000);

		for (int i = 0; i < map.GetLength(0); i++) {
			for (int j = 0; j < map.GetLength(1); j++) {
                //MonoBehaviour.print (Mathf.PerlinNoise (i / 100f, j / 100f));
                map[i, j] = (int) (Mathf.PerlinNoise(i * .1f, j * .1f) * 3);
                if (map[i, j] == 2)
                {
                    map[i, j] = 1;
                }
                //map[i, j] = (int)(Random.Range(0, 4));
			}
		}
		BuildMesh ();
        BuildCollider();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void BuildCollider()
    {
        PolygonCollider2D pc = GetComponent<PolygonCollider2D>();
        int numPaths = 0;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == 0)
                {
                    numPaths++;
                }
            }
        }
        pc.pathCount = numPaths;
        int pathCntr = 0;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == 0)
                {
                    Vector2[] points = { new Vector2(i, j), new Vector2(i + 1, j), new Vector2(i, j + 1), new Vector2(i + 1, j + 1) };
                    pc.SetPath(pathCntr, points);
                    pathCntr++;
                }
            }
        }
    }
	void BuildMesh() {
		Mesh mesh = new Mesh ();

		Vector3[] vertices = new Vector3[4 * map.Length];
		int[] triangles = new int[6 * map.Length];
		Vector2[] uv = new Vector2[vertices.Length];
		Vector3[] normals = new Vector3[vertices.Length];

		for (int i = 0; i < map.GetLength(0); i++) {
			for (int j = 0; j < map.GetLength(1); j++) {
				int baseInd = i * map.GetLength(1) + j;
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

				int textureRow = map[i, j] / textureCols;
				int textureCol = map[i, j] % textureCols;
				MonoBehaviour.print((textureCol / textureCols + texturePad / textureCols) + ", " + (textureRow / textureRows + texturePad / textureRows) + " | " + ((textureCol + 1) / (float) textureCols - texturePad / textureCols) + ", " + (textureRow / textureRows + texturePad / textureRows));
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
}
