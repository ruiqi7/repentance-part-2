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

    private MazeDataGenerator dataGenerator;
    private MazeMeshGenerator meshGenerator;


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
    DisposeOldMaze();
    data = dataGenerator.FromDimensions(sizeRows, sizeCols);
    DisplayMaze();
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
            if (maze[i, j] == 0)
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
