using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSystem : MonoBehaviour {

	public Camera theCamera;

	// The resolution of each tile. This should be a power of 2, plus 1.
	private int tileResolution = 4097;

	// The size of each tile in world space.
	private float tileSize = 1000;

	// The height of each tile in world space.
	float tileHeight = 250;

	// The number of tiles in each direction.
	private int nTiles = 1;

	public Texture2D[] textures;

	public float[] textureTileSize;

	public Texture2D[] normalMaps;

	public GameObject tree;

	// Use this for initialization
	void Start () {
		buildTile (0, 0);
	}
	




	/// <summary>
	/// Creates a terrain tile.
	/// </summary>
	void buildTile(int xPosition, int zPosition) {
		
		GameObject tile = new GameObject ();

		// A tile should be a child of the Terrain System object.
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
		terrainData.size = new Vector3 (tileSize, tileHeight, tileSize);

		// Position the tile in the world.
		tile.transform.localPosition = new Vector3 (xPosition, 0, zPosition);

		terrain.basemapDistance = 2000.0f;

		AddHeightData (terrainData);

		BuildTextures (terrainData);

		addTrees (terrain);

	}





	/// <summary>
	/// Adds height data to a tile.
	/// </summary>
	/// <param name="terrainData">The object which the height data is added to.</param>
	void AddHeightData(TerrainData terrainData) {

		// Get the old height data from the tile.
		float[,] heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);

		float[,] heightData = BuildMap(terrainData.heightmapWidth, terrainData.heightmapHeight);

		// Add height data from the world map to the tile.
		for (int z = 0; z < terrainData.heightmapHeight; z++) {
			for (int x = 0; x < terrainData.heightmapWidth; x++) {
				// Note. X and z are supposed to be switched, in order to map the tiles correctly.
				heights [x, z] = heightData[z, x];
			}
		}
		terrainData.SetHeights (0, 0, heights);
	}





	/// <summary>
	/// Builds the height map.
	/// </summary>
	float[,] BuildMap(int width, int height) {
		
		float[,] heightData = new float[width, height];

		// Number of "passes".
		float octaves = 9;

		// How much the amplitude decreases with each new pass.
		float gain = 0.38f;

		// The change in frequency with each new pass.
		float lacunarity = 2;

		// Initial settings.
		float frequency = 0.002f;
		float amplitude = 0.6f;

		// Raise the terrain at the rim.
		/*
		for (int z = 0; z < nTiles * tileResolution; z++) {
			for (int x = 0; x < nTiles * tileResolution; x++) {

				float xResolution = nTiles * tileResolution;
				float zResolution = nTiles * tileResolution;
				float centreSize = 1000;

				if( (x < xResolution / 2) && (z < xResolution - x) && (z > x) ) {
					heightData [x, z] = ((xResolution / 2) - centreSize - x) / tileHeight;
				} else if ( (x > xResolution / 2) && (z > xResolution - x) && (z < x)) {
					heightData [x, z] = ((xResolution / 2) - xResolution - centreSize + x) / tileHeight;
				}  

				if( (z < zResolution / 2) && (x <= xResolution - z) && (x >= z) ) {
					heightData [x, z] = ((zResolution / 2) - centreSize - z) / tileHeight;
				} else if((z > zResolution / 2) && (x >= xResolution - z) && (x <= z) ) {
					heightData [x, z] = ((zResolution / 2) - xResolution - centreSize + z) / tileHeight;
				}

			}
		}
		*/

		float xcenter = (float)width / 2f;
		float zcenter = (float)height / 2f;

		for (int z = 0; z < width; z++) {
			for (int x = 0; x < height; x++) {
				float xnorm = (float)x / (float)width;
				float znorm = (float)z / (float)height;
				heightData [x, z] = 1.0f -5*(Mathf.Pow(0.5f - xnorm, 2) + Mathf.Pow(0.5f - znorm, 2));
			}
		}


		// Build the initial height map.
		for (int i = 0; i < octaves; i++) {
			for (int z = 0; z < nTiles * tileResolution; z++) {
				for (int x = 0; x < nTiles * tileResolution; x++) {
					heightData [x, z] += Mathf.PerlinNoise ((float)x * frequency, (float)z * frequency) * amplitude;
				}
			}
			frequency *= lacunarity;
			amplitude *= gain;
		}

		// Make the valleys more smooth.
		for (int z = 0; z < nTiles * tileResolution; z++) {
			for (int x = 0; x < nTiles * tileResolution; x++) {
				heightData [x, z] = Mathf.Pow (heightData [x, z], 2.8f);
			}
		}

		float value = 0f;

		for (int z = 0; z < width; z++) {
			for (int x = 0; x < height; x++) {
				//value = Mathf.Max (value, heightData [x, z]);
				if(heightData[x, z] > value) {
					value = heightData[x, z];
				}
			}
		}

		for (int z = 0; z < width; z++) {
			for (int x = 0; x < height; x++) {
				heightData [x, z] /= value;
				if(heightData[x, z] > 1) {
					Debug.LogError("Height data value error");
				}
			}
		}

		Debug.Log ("Map resolution: " + nTiles * tileResolution);

		return heightData;
	}





	/// <summary>
	/// Builds the terrain texture structures.
	/// </summary>
	void BuildTextures(TerrainData terrainData) {

		SplatPrototype[] splatPrototypes = new SplatPrototype[textures.Length];

		// Get the textures and normal maps from the inspector.
		for (int i = 0; i < splatPrototypes.Length; i++) {
			splatPrototypes [i] = new SplatPrototype ();
			splatPrototypes [i].texture = textures [i];
			splatPrototypes [i].tileSize = new Vector2 (textureTileSize [i], textureTileSize [i]);
			splatPrototypes [i].normalMap = normalMaps [i];
		}

		// Add the textures and normal maps to each tile.

		terrainData.splatPrototypes = splatPrototypes;
		ApplyTextures (terrainData);

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

				// Make the border between snow and mountain more clear.
				if(steepness > 0.35) {
					steepness = 1;
				}

				// Apply less snow and more mountain texture to steep areas.
				splatMaps [alphaX, alphaZ, 0] = 1 - steepness;
				splatMaps [alphaX, alphaZ, 1] = steepness;

				if(terrainData.GetInterpolatedHeight(x, z) < 5.0f) {
					splatMaps [alphaX, alphaZ, 1] = 0f;
					splatMaps [alphaX, alphaZ, 0] = terrainData.GetInterpolatedHeight (x, z) / 5.0f;
					splatMaps [alphaX, alphaZ, 2] = 1.0f - (terrainData.GetInterpolatedHeight (x, z) / 5.0f);
				}
			}
		}

		terrainData.SetAlphamaps (0, 0, splatMaps);
	}





	void addTrees(Terrain terrain) {

		TerrainData terrainData = terrain.terrainData;

		List<TreePrototype> treePrototypes = new List<TreePrototype> (terrainData.treePrototypes);

		TreePrototype treePrototype = new TreePrototype ();
		treePrototype.prefab = tree;
		treePrototype.bendFactor = 0f;

		treePrototypes.Add (treePrototype);

		terrainData.treePrototypes = treePrototypes.ToArray();

		Debug.Log (terrainData.treePrototypes.Length);

		int scale = 50;

		for (int y = 0; y < scale; y++) {
			for (int x = 0; x < scale; x++) {

				float ymap = (float)y / (float) scale;
				float xmap = (float)x / (float) scale;

				float mapHeight = terrainData.GetInterpolatedHeight (xmap, ymap);
				float angle = terrainData.GetSteepness (xmap, ymap);
				float steepness = angle / 90.0f;

				if(mapHeight > 5.0f && mapHeight < 50.0f &&steepness < 0.5f) {

					TreeInstance treeInstance = new TreeInstance ();
					treeInstance.prototypeIndex = 0;
					Vector3 position = new Vector3 (xmap, 0f, ymap);
					position.y = mapHeight;
					treeInstance.position = position;
					treeInstance.color = Color.green;
					treeInstance.heightScale = Random.Range (0.5f, 1.5f);
					treeInstance.lightmapColor = Color.white;
					treeInstance.widthScale = 1.0f;

					terrain.AddTreeInstance (treeInstance);
				}
			}
		}

	}





}
