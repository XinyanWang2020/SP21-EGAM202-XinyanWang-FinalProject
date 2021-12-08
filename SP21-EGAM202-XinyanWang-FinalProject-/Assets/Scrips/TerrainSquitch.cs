using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSquitch : MonoBehaviour
{
    public float RandomWalk_StartingElevation;
    public float RandomWalk_MaxStepSize;

    public float SetElevation_Elevation;

    public int Box_zMin = 30;
    public int
        Box_zMax = 100,
        Box_xMin = 30,
        Box_xMax = 100;
    public float Box_Height;

    public float Room_zMin,Room_zMax,Room_xMin,Room_xMax,Room_Height;
    private float NewRoom_xMax,NewRoom_xMin,NewRoom_zMax,NewRoom_zMin,NewRoom_Height;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    List<Vector2> GenerateNeighbours(Vector2 pos, int width, int height)
    {
        List<Vector2> neighbours = new List<Vector2>();
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                if (!(x == 0 && y == 0))
                {
                    Vector2 nPos = new Vector2(Mathf.Clamp(pos.x + x, 0, width - 1),
                                                Mathf.Clamp(pos.y + y, 0, height - 1));
                    if (!neighbours.Contains(nPos))
                        neighbours.Add(nPos);
                }
            }
        }
        return neighbours;
    }
    void Update()
    {
        
    }

    public void RandomWalkProfile()
    {
        Terrain thisTerrain = GetComponent<Terrain>();
        if (thisTerrain == null)
            throw new System.Exception("TerrainSquitch requires a Terrain. Please add a Terrain to " + gameObject.name);

        int heightMapWidth, heightMapLength;
        heightMapWidth = thisTerrain.terrainData.heightmapResolution;
        heightMapLength = thisTerrain.terrainData.heightmapResolution;
        Debug.Log("This Terrain has a heightMap with width=" + heightMapWidth + "and height=" + heightMapLength);

        float[,] heights;
        heights = thisTerrain.terrainData.GetHeights(0, 0, heightMapWidth, heightMapLength);

        Vector3 mapPos;
        float heightAtThisZ = RandomWalk_StartingElevation;
        for (mapPos.z = 0; mapPos.z < heightMapLength; mapPos.z++)
        {
            heightAtThisZ += Random.Range(-RandomWalk_MaxStepSize, RandomWalk_MaxStepSize);
            for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
            {
                heights[(int)mapPos.z, (int)mapPos.x] = heightAtThisZ;
            }
        }
        thisTerrain.terrainData.SetHeights(0, 0, heights);
    }

    public void ExtrudeBox()
    {
        Terrain thisTerrain = GetComponent<Terrain>();
        if (thisTerrain == null)
            throw new System.Exception("TerrainSquitch requires a Terrain. Please add a Terrain to " + gameObject.name);

        int heightMapWidth, heightMapHeight;
        heightMapWidth = thisTerrain.terrainData.heightmapResolution;
        heightMapHeight = thisTerrain.terrainData.heightmapResolution;
        Debug.Log("This Terrain has a heightMap with width=" + heightMapWidth + "and height=" + heightMapHeight);

        float[,] heights;
        heights = thisTerrain.terrainData.GetHeights(0, 0, heightMapWidth, heightMapHeight);

        Vector3 mapPos;
        for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
        {
            for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
            {
                if (mapPos.z > Box_zMin && mapPos.z < Box_zMax && mapPos.x > Box_xMin && mapPos.x < Box_xMax)
                {
                    heights[(int)mapPos.z, (int)mapPos.x] = Box_Height;
                }
            }
        }
        thisTerrain.terrainData.SetHeights(0, 0, heights);
    }

    public void SetElevation()
    {
        Terrain thisTerrain = GetComponent<Terrain>();
        if (thisTerrain == null)
            throw new System.Exception("TerrainSquitch requires a Terrain. Please add a Terrain to " + gameObject.name);

        int heightMapWidth, heightMapHeight;
        heightMapWidth = thisTerrain.terrainData.heightmapResolution;
        heightMapHeight = thisTerrain.terrainData.heightmapResolution;
        Debug.Log("This Terrain has a heightMap with width=" + heightMapWidth + "and height=" + heightMapHeight);

        float[,] heights;
        heights = thisTerrain.terrainData.GetHeights(0, 0, heightMapWidth, heightMapHeight);

        Vector3 mapPos;
        mapPos.z = 0;
        for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
        {
            for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
            {
                heights[(int)mapPos.z, (int)mapPos.x] = SetElevation_Elevation;
            }
        }
        thisTerrain.terrainData.SetHeights(0, 0, heights);
    }

    public void SingleRoom()
    {
        Terrain thisTerrain = GetComponent<Terrain>();
        if (thisTerrain == null)
            throw new System.Exception("TerrainSquitch requires a Terrain. Please add a Terrain to " + gameObject.name);

        int heightMapWidth, heightMapHeight;
        heightMapWidth = thisTerrain.terrainData.heightmapResolution;
        heightMapHeight = thisTerrain.terrainData.heightmapResolution;
        //Debug.Log("This Terrain has a heightMap with width=" + heightMapWidth + "and height=" + heightMapHeight);

        float[,] heights;
        heights = thisTerrain.terrainData.GetHeights(0, 0, heightMapWidth, heightMapHeight);

        NewRoom_Height = 0;

        // add the colision
        GameObject RoomColliderObject = new GameObject();
        RoomColliderObject.AddComponent<BoxCollider>();
        RoomColliderObject.transform.position = new Vector3((Room_xMin + Room_xMax) / 2, Room_Height, (Room_zMin + Room_zMax) / 2);
        RoomColliderObject.transform.localScale = new Vector3((Room_xMax - Room_xMin), 500, (Room_zMax - Room_zMin));

        RoomColliderObject.GetComponent<BoxCollider>().isTrigger = true;
        RoomColliderObject.layer = 7;

        // if the room is horizontal
        if (Room_xMax > Room_zMax)
        {
            Debug.Log("1");
            Vector3 mapPos;
            for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
            {
                for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
                {
                    if (mapPos.z > Room_zMin && mapPos.z < Room_zMax && mapPos.x > Room_xMin && mapPos.x < Room_xMax)
                    {
                        heights[(int)mapPos.z, (int)mapPos.x] = Room_Height;
                    }
                }
            }
            //add a door
            for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
            {
                for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
                {
                    if (mapPos.z > Room_zMin && mapPos.z < Room_zMax && mapPos.x > Room_xMin -20 && mapPos.x < Room_xMax - 20)
                    {
                        heights[(int)mapPos.z, (int)mapPos.x] = NewRoom_Height;
                    }
                }
            }
            // add another wall which is the same
            for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
            {
                for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
                {
                    if (mapPos.z > Room_zMin + 50 && mapPos.z < Room_zMax + 50 && mapPos.x > Room_xMin && mapPos.x < Room_xMax)
                    {
                        heights[(int)mapPos.z, (int)mapPos.x] = Room_Height;
                    }
                }
            }
            // Create another wall
            NewRoom_zMax = Room_zMin + 50;
            NewRoom_xMin = Room_xMax - 5;
            for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
            {
                for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
                {
                    if (mapPos.z > Room_zMin && mapPos.z < NewRoom_zMax && mapPos.x > NewRoom_xMin && mapPos.x < Room_xMax)
                    {
                        heights[(int)mapPos.z, (int)mapPos.x] = Room_Height;
                    }
                }
            }
            // add another wall which is the same
            NewRoom_xMax = Room_xMax - (Room_xMax - Room_xMin);
            NewRoom_xMin = NewRoom_xMax - 5;
            for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
            {
                for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
                {
                    if (mapPos.z > Room_zMin && mapPos.z < NewRoom_zMax && mapPos.x > NewRoom_xMin && mapPos.x < NewRoom_xMax)
                    {
                        heights[(int)mapPos.z, (int)mapPos.x] = Room_Height;
                    }
                }
            }


        }

        //if the room is vertical
        else
        {
            Debug.Log("3");
            Vector3 mapPos;
            for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
            {
                for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
                {
                    if (mapPos.z > Room_zMin && mapPos.z < Room_zMax && mapPos.x > Room_xMin && mapPos.x < Room_xMax)
                    {
                        heights[(int)mapPos.z, (int)mapPos.x] = Room_Height;
                    }
                }
            }
            //add a door
            for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
            {
                for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
                {
                    if (mapPos.z > Room_zMin - 20 && mapPos.z < Room_zMax - 20 && mapPos.x > Room_xMin  && mapPos.x < Room_xMax )
                    {
                        heights[(int)mapPos.z, (int)mapPos.x] = NewRoom_Height;
                    }
                }
            }
            // add another wall which is the same
            for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
            {
                for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
                {
                    if (mapPos.z > Room_zMin && mapPos.z < Room_zMax && mapPos.x > Room_xMin + 50 && mapPos.x < Room_xMax + 50)
                    {
                        heights[(int)mapPos.z, (int)mapPos.x] = Room_Height;
                    }
                }
            }
            // add another wall
            NewRoom_zMin = Room_zMax - 5;
            NewRoom_xMax = Room_xMin + 50;
            for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
            {
                for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
                {
                    if (mapPos.z > NewRoom_zMin && mapPos.z < Room_zMax && mapPos.x > Room_xMin && mapPos.x < NewRoom_xMax)
                    {
                        heights[(int)mapPos.z, (int)mapPos.x] = Room_Height;
                    }
                }
            }

            NewRoom_zMax = Room_zMax - (Room_zMax - Room_zMin);
            NewRoom_zMin = NewRoom_zMax - 5;
            for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
            {
                for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
                {
                    if (mapPos.z > NewRoom_zMin && mapPos.z < NewRoom_zMax && mapPos.x > Room_xMin && mapPos.x < NewRoom_xMax)
                    {
                        heights[(int)mapPos.z, (int)mapPos.x] = Room_Height;
                    }
                }
            }

        }
        thisTerrain.terrainData.SetHeights(0, 0, heights);
    }
}
