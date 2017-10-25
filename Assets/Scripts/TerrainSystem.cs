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

	public Texture2D grass;

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

		terrain.castShadows = true;

		terrain.treeBillboardDistance = 350.0f;
		terrain.treeCrossFadeLength = 100.0f;
		terrain.treeDistance = 1000.0f;

		terrain.detailObjectDistance = 500.0f;

		AddHeightData (terrainData);

		BuildTextures (terrainData);

		AddTrees (terrain);

		AddGrass (terrain);

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

		// Add island mask.
		for (int z = 0; z < width; z++) {
			for (int x = 0; x < height; x++) {
				float xnorm = (float)x / (float)width;
				float znorm = (float)z / (float)height;
				heightData [x, z] = 1.0f -5*(Mathf.Pow(0.5f - xnorm, 2) + Mathf.Pow(0.5f - znorm, 2));
			}
		}

		// Add mountains.
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

		// Normalise the height data.
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





	/// <summary>
	/// Adds trees to the terrain. The trees are placed based on the terrain height.
	/// </summary>
	void AddTrees(Terrain terrain) {

		TerrainData terrainData = terrain.terrainData;

		// Get the tree prototypes.
		List<TreePrototype> treePrototypes = new List<TreePrototype> (terrainData.treePrototypes);

		// Create a new tree prototype.
		TreePrototype treePrototype = new TreePrototype ();
		treePrototype.prefab = tree;
		treePrototype.bendFactor = 0f;

		// Add the new tree prototype to the terrain.
		treePrototypes.Add (treePrototype);
		terrainData.treePrototypes = treePrototypes.ToArray();

		Debug.Log (terrainData.treePrototypes.Length);

		int scale = 50;

		// Place trees in the terrain.
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
					treeInstance.color = new Color32(255, 255, 255, 5);
					treeInstance.heightScale = Random.Range (0.5f, 1.9f);
					treeInstance.lightmapColor = new Color32(255, 255, 255, 5);
					treeInstance.widthScale = 1.0f;

					terrain.AddTreeInstance (treeInstance);
				}
			}
		}

	}





	/// <summary>
	/// Adds grass to the terrain. The grass is place based on the terrain textures.
	/// </summary>
	/// <param name="terrain">Terrain.</param>
	void AddGrass(Terrain terrain) {

		TerrainData terrainData = terrain.terrainData;

		// Get the detail prototypes.
		List<DetailPrototype> detailPrototypes = new List<DetailPrototype> (terrainData.detailPrototypes);

		// Create a new detail prototype.
		DetailPrototype detailPrototype = new DetailPrototype ();
		detailPrototype.prototypeTexture = grass;
		detailPrototype.bendFactor = 0f;
		detailPrototype.dryColor = new Color (205f, 188f, 26f, 255f);
		detailPrototype.healthyColor = new Color (67f, 249f, 42f, 255f);
		detailPrototype.minHeight = 1f;
		detailPrototype.maxHeight = 5f;
		detailPrototype.minWidth = 1f;
		detailPrototype.maxWidth = 2f;
		detailPrototype.noiseSpread = 0.5f;
		detailPrototype.usePrototypeMesh = false;
		detailPrototype.renderMode = DetailRenderMode.GrassBillboard;

		// Add the new detail prototype to the terrain.
		detailPrototypes.Add(detailPrototype);
		terrainData.detailPrototypes = detailPrototypes.ToArray();

		Debug.Log(terrainData.detailPrototypes.Length);

		// Convert from detail map to alpha map coordinate system.
		float[,,] splatMaps = terrainData.GetAlphamaps (0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
		int alphaWidth = terrainData.alphamapWidth;
		int alphaHeight = terrainData.alphamapHeight;
		int detailWidth = terrainData.detailWidth;
		int detailHeight = terrainData.detailHeight;
		float widthScale = (1.0f / (float)detailWidth) * (float)alphaWidth;
		float heightScale = (1.0f / (float)detailHeight) * (float)alphaHeight;

		// Get the terrain splat textures.
		int[,] map = terrain.terrainData.GetDetailLayer (0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 0);

		// Place grass in the terrain.
		for (int y = 0; y < terrainData.detailHeight; y++) {
			for (int x = 0; x < terrainData.detailWidth; x++) {
				int alphaX = Mathf.FloorToInt ((float)x * widthScale);
				int alphaY = Mathf.FloorToInt ((float)y * heightScale);
				float value = splatMaps [alphaX, alphaY, 0];
				if(value > 0.8f) {
					map [x, y] = 5;
				}
			}
		}

		terrainData.SetDetailLayer (0, 0, 0, map);

	}





}
