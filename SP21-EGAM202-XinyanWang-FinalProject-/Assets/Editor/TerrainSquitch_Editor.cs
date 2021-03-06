using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainSquitch))]
[CanEditMultipleObjects]
public class TerrainSquitch_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TerrainSquitch squitch = (TerrainSquitch)target;

        if (GUILayout.Button("RandomWalkProfile"))
            squitch.RandomWalkProfile();

        if (GUILayout.Button("Extrude Box"))
            squitch.ExtrudeBox();

        if (GUILayout.Button("Set Elevation"))
            squitch.SetElevation();

        if (GUILayout.Button("Single Room"))
            squitch.SingleRoom();

        if (GUILayout.Button("Many Rooms"))
            squitch.ManyRooms();

        if (GUILayout.Button("Fill Niche"))
            squitch.FillNiche();

        if (GUILayout.Button("Many Rooms2"))
            squitch.ManyRooms2();
    }
}
