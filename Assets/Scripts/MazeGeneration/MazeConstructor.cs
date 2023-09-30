// Followed tutorial from https://www.kodeco.com/82-procedural-generation-of-mazes-with-unity?page=2
// Parts of the code from the tutorial have been modified 

using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    //1
    public bool showDebug;
    
    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;

    public GameObject candle;
    public GameObject doll;
    public GameObject salt;
    public GameObject eyeballsJar;
    public GameObject witheredFlower;

    private int candleNum = 3;
    private int dollNum = 3;
    private int saltNum = 5;
    private int eyeballsJarNum = 2;
    private int witheredFlowerNum = 3;

    private static int candleCount = 0;
    private static int dollCount = 0;
    private static int saltCount = 0;
    private static int eyeballsJarCount = 0;
    private static int witheredFlowerCount = 0;

    private MazeDataGenerator dataGenerator;
    private MazeMeshGenerator meshGenerator;

    private float width = 9f;
    
    //2
    public int[,] data
    {
        get; private set;
    }

    //3
    void Awake()
    {
        meshGenerator = new MazeMeshGenerator();
        dataGenerator = new MazeDataGenerator();
        // default to walls surrounding a single empty cell
        data = new int[,]
        {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
        };
    }
    
    public void GenerateNewMaze(int sizeRows, int sizeCols)
    {
           if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
    {
        Debug.LogError("Odd numbers work better for dungeon size.");
    }
    data = dataGenerator.FromDimensions(sizeRows, sizeCols);
    DisplayMaze();
    SpawnItems();
    }

    public void DisposeOldMaze()
    {
    GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
    foreach (GameObject go in objects) {
        Destroy(go);
    }
    }

    private void DisplayMaze()
    {
        GameObject go = new GameObject();
        go.transform.position = Vector3.zero;
        go.name = "Procedural Maze";
        go.tag = "Generated";

        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = meshGenerator.FromData(data);
    
        MeshCollider mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mf.mesh;

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.materials = new Material[2] {mazeMat1, mazeMat2};
    }

    private void SpawnItems()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);


        for (int i = rMax; i >= 0; i--)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 2 && candleCount<candleNum)
                {
                    Instantiate(candle, new Vector3(j*width, 0, i*width), Quaternion.identity);
                    candleCount++;
                } 
                else if (maze[i, j] == 3 && dollCount<dollNum)
                {
                    Instantiate(doll, new Vector3(j*width, 0.2f, i*width), Quaternion.Euler(new Vector3(270, 0, 0)));
                    dollCount++;
                }
                else if (maze[i, j] == 4 && saltCount<saltNum)
                {
                    Instantiate(salt, new Vector3(j*width, 0, i*width), Quaternion.identity);
                    saltCount++;
                }
                else if (maze[i, j] == 5 && eyeballsJarCount<eyeballsJarNum)
                {
                    eyeballsJarCount++;
                }
                else if (maze[i, j] == 6 && witheredFlowerCount<witheredFlowerNum)
                {
                    Instantiate(witheredFlower, new Vector3(j*width, 0, i*width), Quaternion.identity);
                    witheredFlowerCount++;
                }                
            }
        }
    }

    void OnGUI()
    {
        //1
        if (!showDebug)
        {
        return;
        }

    //2
    int[,] maze = data;
    int rMax = maze.GetUpperBound(0);
    int cMax = maze.GetUpperBound(1);

    string msg = "";

    //3
    for (int i = rMax; i >= 0; i--)
    {
        for (int j = 0; j <= cMax; j++)
        {
            if (maze[i, j] != 1)
            {
                msg += "....";
            } 
            else
            {
                msg += "==";
            }
        }
        msg += "\n";
    }

    //4
    GUI.Label(new Rect(20, 20, 500, 500), msg);
}

}
