/// <summary>
/// Build terrain. Used to build the terrain in the Unity editor.
/// Author: Jonatan Cöster
/// Created: 2017-10-28
/// Version: 1.0
/// </summary>

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