using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

    public float NumberOfRooms;

    [Header("Fill Niche Settings")]
    public Niche FillNiche_Niche;
    public Niche FillNiche_Niche1;
    public Niche FillNiche_Niche2;
    public Transform FillNiche_ParentsTransform;
    public Transform FillNiche_ParentsTransform1;
    public Transform FillNiche_ParentsTransform2;

    [Header("Install Water Setting")]
    public float InstallWater_WaterLevel;
    public GameObject WaterPrefab;
    public float WaterPrefabSize;
    public Transform WaterParent;

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
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestLevel();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Exit();
        }
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
        Debug.Log("This Terrain has a heightMap with width=" + heightMapWidth + "and height=" + heightMapHeight);

        float[,] heights;
        heights = thisTerrain.terrainData.GetHeights(0, 0, heightMapWidth, heightMapHeight);

        // change the mappos into worldpos
        NewRoom_Height = 0.1f;
        float Room_zMin_World, Room_zMax_World;
        float Room_xMin_World, Room_xMax_World;
        Room_zMin_World =Room_zMin * thisTerrain.terrainData.heightmapScale.z;
        Room_zMax_World = Room_zMax * thisTerrain.terrainData.heightmapScale.z;
        Room_xMin_World = Room_xMin * thisTerrain.terrainData.heightmapScale.x;
        Room_xMax_World = Room_xMax * thisTerrain.terrainData.heightmapScale.x;

        Vector3 centerOfBox = new Vector3((Room_xMin_World + Room_xMax_World) / 2, Room_Height, (Room_zMin_World + Room_zMax_World) / 2);
        Vector3 halfSizeOfBox = new Vector3((Room_xMax_World - Room_xMin_World) / 2, 500 / 2, (Room_zMax_World - Room_zMin_World) / 2);

        if (Physics.CheckBox(centerOfBox, halfSizeOfBox, Quaternion.identity, LayerMask.GetMask("Rooms"), QueryTriggerInteraction.UseGlobal))
            return;

        // add the colision
        /*GameObject RoomColliderObject = new GameObject();
        RoomColliderObject.AddComponent<BoxCollider>();
        RoomColliderObject.transform.position = new Vector3((Room_xMin_World + Room_xMax_World) / 2, Room_Height, (Room_zMin_World + Room_zMax_World) / 2);
        RoomColliderObject.transform.localScale = new Vector3((Room_xMax_World - Room_xMin_World), 500, (Room_zMax_World - Room_zMin_World));

        RoomColliderObject.GetComponent<BoxCollider>().isTrigger = true;
        RoomColliderObject.layer = 7;*/



        // if the room is horizontal
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

        //Pull middle down
        for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
        {
            for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
            {
                if (mapPos.z > Room_zMin + 5 && mapPos.z < Room_zMax - 5 && mapPos.x > Room_xMin + 5 && mapPos.x < Room_xMax - 5)
                {
                    heights[(int)mapPos.z, (int)mapPos.x] = NewRoom_Height;
                }
            }
        }

        //Push down a piece of wall, to make a door
        for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
        {
            for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
            {
                if (mapPos.z > Room_zMin + 50 && mapPos.z < Room_zMin + 60 && mapPos.x > Room_xMin && mapPos.x < Room_xMin + 50)
                {
                    heights[(int)mapPos.z, (int)mapPos.x] = NewRoom_Height;
                }
            }
        }
        thisTerrain.terrainData.SetHeights(0, 0, heights);
    }

    public void ManyRooms()
    {
        Terrain thisTerrain = GetComponent<Terrain>();
        if (thisTerrain == null)
            throw new System.Exception("TerrainSquitch requires a Terrain. Please add a Terrain to " + gameObject.name);

        int heightMapWidth, heightMapHeight,i;
        heightMapWidth = thisTerrain.terrainData.heightmapResolution;
        heightMapHeight = thisTerrain.terrainData.heightmapResolution;
        //Debug.Log("This Terrain has a heightMap with width=" + heightMapWidth + "and height=" + heightMapHeight);

        float[,] heights;
        heights = thisTerrain.terrainData.GetHeights(0, 0, heightMapWidth, heightMapHeight);

        // change the mappos into worldpos
        
        NewRoom_Height = 0;
        float Room_zMin_World2, Room_zMax_World2;
        float Room_xMin_World2, Room_xMax_World2;
        
        i = 0;
        while (i < NumberOfRooms)
        {
            Debug.Log("create");
            NewRoom_xMax = Random.Range(150f, 500f);
            NewRoom_xMin = NewRoom_xMax - Random.Range(100f, 200f);
            NewRoom_zMax = Random.Range(150f, 500f);
            NewRoom_zMin = NewRoom_zMax - Random.Range(100f, 200f);

            Room_zMin_World2 = NewRoom_zMin * thisTerrain.terrainData.heightmapScale.z;
            Room_zMax_World2 = NewRoom_zMax * thisTerrain.terrainData.heightmapScale.z;
            Room_xMin_World2 = NewRoom_xMin * thisTerrain.terrainData.heightmapScale.x;
            Room_xMax_World2 = NewRoom_xMax * thisTerrain.terrainData.heightmapScale.x;


            Vector3 centerOfBox = new Vector3((Room_xMin_World2 + Room_xMax_World2) / 2, Room_Height, (Room_zMin_World2 + Room_zMax_World2) / 2);
            Vector3 halfSizeOfBox = new Vector3((Room_xMax_World2 - Room_xMin_World2) / 2, 500 / 2, (Room_zMax_World2 - Room_zMin_World2) / 2);

            // add the colision
           /* GameObject RoomColliderObject = new GameObject();
            RoomColliderObject.AddComponent<BoxCollider>();
            RoomColliderObject.transform.position = new Vector3((Room_xMin_World2 + Room_xMax_World2) / 2, Room_Height, (Room_zMin_World2 + Room_zMax_World2) / 2);
            RoomColliderObject.transform.localScale = new Vector3((Room_xMax_World2 - Room_xMin_World2), 500, (Room_zMax_World2 - Room_zMin_World2));

            RoomColliderObject.GetComponent<BoxCollider>().isTrigger = true;
            RoomColliderObject.layer = 7;*/
            i++;

            Vector3 mapPos;
            if (Physics.CheckBox(centerOfBox, halfSizeOfBox, Quaternion.identity, LayerMask.GetMask("Rooms"), QueryTriggerInteraction.UseGlobal))
                return;
            Debug.Log("1");
            for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
            {
                for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
                {
                    if (mapPos.z > NewRoom_zMin && mapPos.z < NewRoom_zMax && mapPos.x > NewRoom_xMin && mapPos.x < NewRoom_xMax)
                    {
                        heights[(int)mapPos.z, (int)mapPos.x] = Room_Height;
                    }
                }
            }

            //Pull middle down
            for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
            {
                for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
                {
                    if (mapPos.z > NewRoom_zMin + 5 && mapPos.z < NewRoom_zMax - 5 && mapPos.x > NewRoom_xMin + 5 && mapPos.x < NewRoom_xMax - 5)
                    {
                        heights[(int)mapPos.z, (int)mapPos.x] = NewRoom_Height;
                    }
                }
            }

            //Push down a piece of wall, to make a door
            for (mapPos.z = 0; mapPos.z < heightMapHeight; mapPos.z++)
            {
                for (mapPos.x = 0; mapPos.x < heightMapWidth; mapPos.x++)
                {
                    if (mapPos.z > NewRoom_zMin + 10 && mapPos.z < NewRoom_zMin + 30 && mapPos.x > NewRoom_xMin && mapPos.x < NewRoom_xMin + 10)
                    {
                        heights[(int)mapPos.z, (int)mapPos.x] = NewRoom_Height;
                    }
                }
            }
            thisTerrain.terrainData.SetHeights(0, 0, heights);
            GetComponent<UnityEngine.AI.NavMeshSurface>().BuildNavMesh();
        }
    }

    public void ManyRooms2()
    {
        float i;
        i = 0;
        while (i < NumberOfRooms)
        {
            Room_xMax = Random.Range(150f, 500f);
            Room_xMin = Room_xMax - Random.Range(100f, 200f);
            Room_zMax = Random.Range(150f, 500f);
            Room_zMin = Room_zMax - Random.Range(100f, 200f);

            SingleRoom();
            i++;
        }
        GetComponent<UnityEngine.AI.NavMeshSurface>().BuildNavMesh();
    }

    public void FillNiche()
    {
        Terrain thisTerrain = GetComponent<Terrain>();
        if (thisTerrain == null)
            throw new System.Exception("TerrainSquitch requires a Terrain. Please add a Terrain to " + gameObject.name);

        int heightMapWidth, heightMapLength;
        heightMapWidth = thisTerrain.terrainData.heightmapResolution;
        heightMapLength = thisTerrain.terrainData.heightmapResolution;
        Debug.Log("This Terrain has a heightMap with width=" + heightMapWidth + "and height=" + heightMapLength);

        //knowing the size of the Terrain, in worlds units
        float heightMapWidthInWorld, heightMapLengthInWorld;
        heightMapWidthInWorld = heightMapWidth * thisTerrain.terrainData.heightmapScale.x;
        heightMapLengthInWorld = heightMapLength * thisTerrain.terrainData.heightmapScale.z;

        //using worldPos
        Vector3 worldPos;
        worldPos = new Vector3(0, InstallWater_WaterLevel, 0);
        for (worldPos.z = 0; worldPos.z < heightMapLengthInWorld; worldPos.z += 1)
        {
            for (worldPos.x = 0; worldPos.x < heightMapWidthInWorld; worldPos.x += 1)
            {
                worldPos.y = thisTerrain.SampleHeight(worldPos);
                //check x, z, elevation
                if (worldPos.x > FillNiche_Niche.MinX && worldPos.x < FillNiche_Niche.MaxX &&
                    worldPos.z > FillNiche_Niche.MinZ && worldPos.z < FillNiche_Niche.MaxZ &&
                    worldPos.y > FillNiche_Niche.MinElve && worldPos.y < FillNiche_Niche.MaxElev)
                {
                    //Draw a random number, and instantiate
                    if (Random.value < FillNiche_Niche.ProbabilityPerHeter)
                    {
                        Instantiate(FillNiche_Niche.NicheOccupant, worldPos, Quaternion.identity, FillNiche_ParentsTransform);
                    }
                }
                if (worldPos.x > FillNiche_Niche1.MinX && worldPos.x < FillNiche_Niche1.MaxX &&
                   worldPos.z > FillNiche_Niche1.MinZ && worldPos.z < FillNiche_Niche1.MaxZ &&
                   worldPos.y > FillNiche_Niche1.MinElve && worldPos.y < FillNiche_Niche1.MaxElev)
                {
                    //Draw a random number, and instantiate
                    if (Random.value < FillNiche_Niche1.ProbabilityPerHeter)
                    {
                        Instantiate(FillNiche_Niche1.NicheOccupant, worldPos, Quaternion.identity, FillNiche_ParentsTransform1);
                    }
                }
                if (worldPos.x > FillNiche_Niche2.MinX && worldPos.x < FillNiche_Niche2.MaxX &&
                   worldPos.z > FillNiche_Niche2.MinZ && worldPos.z < FillNiche_Niche2.MaxZ &&
                   worldPos.y > FillNiche_Niche2.MinElve && worldPos.y < FillNiche_Niche2.MaxElev)
                {
                    //Draw a random number, and instantiate
                    if (Random.value < FillNiche_Niche2.ProbabilityPerHeter)
                    {
                        Instantiate(FillNiche_Niche2.NicheOccupant, worldPos, Quaternion.identity, FillNiche_ParentsTransform2);
                    }
                }
            }
        }
    }

    public void RestLevel()
    {
        SceneManager.LoadScene("Main");

        Transform ParentForDeletion;
        ParentForDeletion = GameObject.Find("HolderOfAll").transform;
        for(int childi = 0; childi < ParentForDeletion.childCount; childi++)
        {
            Destroy(ParentForDeletion.GetChild(childi).gameObject, 0.1f);
        }

        SetElevation_Elevation = 0;
        SetElevation();
        SetElevation_Elevation = .1f;
        SetElevation();
        ManyRooms2();
        FillNiche();

        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
