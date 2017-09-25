using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSystem : MonoBehaviour {

	public Camera theCamera;

	// The resolution of each tile. This should be a power of 2, plus 1.
	private int tileResolution = 1025;

	// The size of each tile in world space.
	private float tileSize = 10000;

	// The number of tiles in each direction.
	private int nTiles = 3;

	// The heightmap of the world.
	private float[,] map;

	// The terrain tiles which constitutes the terrain.
	private GameObject[,] terrainTiles;

	public Texture2D[] textures;

	public float[] textureTileSize;

	public Texture2D[] normalMaps;

	// Use this for initialization
	void Start () {
		buildTiles ();
		BuildMap ();
		BuildTerrain ();
		BuildTextures ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}





	/// <summary>
	/// Creates the terrain tiles. Note that this does not set the terrain height data.
	/// </summary>
	void buildTiles() {
		
		terrainTiles = new GameObject[nTiles, nTiles];

		for (int z = 0; z < nTiles; z++) {
			for (int x = 0; x < nTiles; x++) {
				
				GameObject tile = new GameObject ();

				// Give the tile a name based on its position.
				tile.name = x + " " + z;

				// All tiles should be children of the Terrain System object.
				tile.transform.SetParent (gameObject.transform);

				// Add the terrain components to the tile.
				Terrain terrain = tile.AddComponent<Terrain> ();
				TerrainCollider terrainCollider = tile.AddComponent<TerrainCollider> ();
				TerrainData terrainData = new TerrainData ();
				terrain.terrainData = terrainData;
				terrainCollider.terrainData = terrainData;

				// Apply terrain tile settings.
				terrainData.heightmapResolution = tileResolution;
				terrainData.baseMapResolution = 513;
				terrainData.SetDetailResolution (1024, 32);
				terrainData.size = new Vector3 (tileSize, 4096, tileSize);

				// Position the tile in the world.
				tile.transform.localPosition = new Vector3 (x * tileSize, 0, z * tileSize);

				terrainTiles [x, z] = tile;

				terrain.basemapDistance = 2000.0f;
			}
		}

		// Set each tiles neighbors.
		for (int z = 0; z < nTiles; z++) {
			for (int x = 0; x < nTiles; x++) {

				Terrain topNeighbor = null;
				Terrain leftNeighbor = null;
				Terrain bottomNeighbor = null;
				Terrain rightNeighbor = null;

				if(nTiles == 1) {
					// The tile has no neighbors.
					terrainTiles [x, z].GetComponent<Terrain> ().SetNeighbors (leftNeighbor, topNeighbor, rightNeighbor, bottomNeighbor);
					return;
				}

				if(z == 0) {
					// The tile has a top neighbor.
					topNeighbor = terrainTiles [x, z+1].GetComponent<Terrain>();
				} else if (z == nTiles-1) {
					// The tile has a bottom neighbor.
					bottomNeighbor = terrainTiles[x, z-1].GetComponent<Terrain>();
				} else {
					// The tile has top and bottom neighbors.
					topNeighbor = terrainTiles [x, z+1].GetComponent<Terrain>();
					bottomNeighbor = terrainTiles[x, z-1].GetComponent<Terrain>();
				}

				if(x == 0) {
					// The tile has a right neighbor.
					rightNeighbor = terrainTiles[x+1, z].GetComponent<Terrain>();
				} else if(x == nTiles-1) {
					// The tile has a left neighbor
					leftNeighbor = terrainTiles[x-1, z].GetComponent<Terrain>();
				} else {
					// The tile has right and left neighbors.
					rightNeighbor = terrainTiles[x+1, z].GetComponent<Terrain>();
					leftNeighbor = terrainTiles[x-1, z].GetComponent<Terrain>();
				}

				terrainTiles [x, z].GetComponent<Terrain> ().SetNeighbors (leftNeighbor, topNeighbor, rightNeighbor, bottomNeighbor);

			}
		}
	}





	/// <summary>
	/// Legacy map building function. Do not use this.
	/// </summary>
	void BuildMapOld() {
		map = new float[nTiles * tileResolution, nTiles * tileResolution];
		float octaves = 8;
		float gain = 0.5f;
		float lacunarity = 2;
		float frequency = 18f;
		float amplitude = 0.5f;

		for (int i = 0; i < octaves; i++) {
			for (int z = 0; z < nTiles * tileResolution; z++) {
				for (int x = 0; x < nTiles * tileResolution; x++) {
					float xx = (float)x / (float)map.GetLength (0);
					float zz = (float)z / (float)map.GetLength (1);
					map [x, z] += Mathf.PerlinNoise (xx * frequency, zz * frequency) * amplitude;
				}
			}
			frequency *= lacunarity;
			amplitude *= gain;
		}

	}





	/// <summary>
	/// Builds the height map.
	/// </summary>
	void BuildMap() {
		
		map = new float[nTiles * tileResolution, nTiles * tileResolution];

		// Number of "passes".
		float octaves = 9;

		// How much the amplitude decreases with each new pass.
		float gain = 0.38f;

		// The change in frequency with each new pass.
		float lacunarity = 2;

		// Initial settings.
		float frequency = 0.002f;
		float amplitude = 0.65f;

		// Build the initial height map.
		for (int i = 0; i < octaves; i++) {
			for (int z = 0; z < nTiles * tileResolution; z++) {
				for (int x = 0; x < nTiles * tileResolution; x++) {
					map [x, z] += Mathf.PerlinNoise ((float)x * frequency, (float)z * frequency) * amplitude;
				}
			}
			frequency *= lacunarity;
			amplitude *= gain;
		}

		// Make the valleys more smooth.
		for (int z = 0; z < nTiles * tileResolution; z++) {
			for (int x = 0; x < nTiles * tileResolution; x++) {
				map [x, z] = Mathf.Pow (map [x, z], 2.8f);
			}
		}

		Debug.Log ("Map resolution: " + nTiles * tileResolution);
	}





	/// <summary>
	/// Adds height data to all tiles.
	/// </summary>
	void BuildTerrain() {
		for (int z = 0; z < nTiles; z++) {
			for (int x = 0; x < nTiles; x++) {
				AddTileHeight ((TerrainData)terrainTiles [x, z].GetComponent<Terrain>().terrainData, x, z);
			}
		}
	}





	/// <summary>
	/// Adds height data from the world map to a tile.
	/// </summary>
	/// <param name="terrainData">The object which the height data is added to.</param>
	/// <param name="xTileNumber">Tile number in the x direction.</param>
	/// <param name="zTileNumber">Tile number in the z direction.</param>
	void AddTileHeight(TerrainData terrainData, int xTileNumber, int zTileNumber) {

		// Get the old height data from the tile.
		float[,] heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);

		//TODO: Fix the terrain seams.

		// Add height data from the world map to the tile.
		for (int z = 0; z < terrainData.heightmapHeight; z++) {
			for (int x = 0; x < terrainData.heightmapWidth; x++) {
				// Note. X and z are supposed to be switched, in order to map the tiles correctly.
				heights [x, z] = map [(xTileNumber * tileResolution) + z, (zTileNumber * tileResolution) + x];
			}
		}
		terrainData.SetHeights (0, 0, heights);
	}





	/// <summary>
	/// Builds the terrain texture structures.
	/// </summary>
	void BuildTextures() {

		SplatPrototype[] splatPrototypes = new SplatPrototype[textures.Length];

		// Get the textures and normal maps from the inspector.
		for (int i = 0; i < splatPrototypes.Length; i++) {
			splatPrototypes [i] = new SplatPrototype ();
			splatPrototypes [i].texture = textures [i];
			splatPrototypes [i].tileSize = new Vector2 (textureTileSize [i], textureTileSize [i]);
			splatPrototypes [i].normalMap = normalMaps [i];
		}

		// Add the textures and normal maps to each tile.
		for (int z = 0; z < nTiles; z++) {
			for (int x = 0; x < nTiles; x++) {
				terrainTiles [x, z].GetComponent<Terrain> ().terrainData.splatPrototypes = splatPrototypes;
				ApplyTextures (terrainTiles [x, z].GetComponent<Terrain> ().terrainData);
			}
		}

	}





	/// <summary>
	/// Applies the textures to the terrain. The textures are apllied according to how steep the terrain is.
	/// </summary>
	/// <param name="terrainData">Terrain data.</param>
	void ApplyTextures(TerrainData terrainData) {

		float[,,] splatMaps = terrainData.GetAlphamaps (0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

		for (int alphaZ = 0; alphaZ < terrainData.alphamapWidth; alphaZ++) {
			for (int alphaX = 0; alphaX < terrainData.alphamapHeight; alphaX++) {
				float x = (float)alphaX / (float)terrainData.alphamapWidth;
				float z = (float)alphaZ / (float)terrainData.alphamapHeight;

				// Get the gradient.
				float angle = terrainData.GetSteepness (z, x);

				// How steep the terrain is.
				float steepness = angle / 90.0f;

				// Apply less snow and more mountain texture to steep areas.
				splatMaps [alphaX, alphaZ, 0] = 1 - steepness;
				splatMaps [alphaX, alphaZ, 1] = steepness;
			}
		}

		terrainData.SetAlphamaps (0, 0, splatMaps);
	}





}
