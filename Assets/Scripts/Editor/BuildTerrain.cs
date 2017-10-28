using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainSystem))]
public class BuildTerrain : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		TerrainSystem myScript = (TerrainSystem)target;
		if(GUILayout.Button("Build Object"))
		{
			myScript.buildTile (0, 0);
		}
	}
}