// Followed tutorial from https://www.kodeco.com/82-procedural-generation-of-mazes-with-unity?page=2
// to create procedurally generated maze 
// Parts of the code from the tutorial have been modified 
// Code to randomly generate items, npcs and torches was not included in the tutorial 

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
    public GameObject NPC5;
    public GameObject NPC4;
    public GameObject NPC3;
    public GameObject NPC2;
    public GameObject NPC1;
    public GameObject eyeballLetter;
    public GameObject letter1;
    public GameObject letter3;
    public GameObject letter5;
    public GameObject letter6;
    public GameObject letter8;
    public GameObject letter9;
    public GameObject letter10;
    public GameObject torch;
    public BoxCollider boxCol;


    private int candleNum = 10;
    private int dollNum = 6;
    private int saltNum = 8;
    private int eyeballsJarNum = 5;
    private int witheredFlowerNum = 7;
    private int NPCNum = 1;
    private int letterNum = 7;

    private static int candleCount = 0;
    private static int dollCount = 0;
    private static int saltCount = 0;
    private static int eyeballsJarCount = 0;
    private static int witheredFlowerCount = 0;
    private static int NPC5Count = 0;
    private static int NPC4Count = 0;
    private static int NPC3Count = 0;
    private static int NPC2Count = 0;
    private static int NPC1Count = 0;
    private static int letterCount = 0;

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
    DisposeOldMaze();
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

        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.mass = 0;
        rb.drag = 0;
        rb.angularDrag = 0.05f;
        rb.isKinematic = true;
        rb.useGravity = true;
        rb.automaticCenterOfMass = true;
        rb.automaticInertiaTensor = true;
    }

    // Spawns items, torches and NPCs randomly 
    private void SpawnItems()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);


        for (int i = rMax; i >= 0; i--)
        {
            for (int j = 0; j <= cMax; j++)
            {   
                //items
                if (maze[i, j] == 2 && candleCount<candleNum)
                {
                    Instantiate(candle, new Vector3(j*width, 0, i*width), Quaternion.identity);
                    candleCount++;
                    maze[i,j] = -1; 
                } 
                else if (maze[i, j] == 3 && dollCount<dollNum)
                {
                    Instantiate(doll, new Vector3(j*width, 0.25f, i*width), Quaternion.Euler(new Vector3(270, 0, 0)));
                    dollCount++;
                    maze[i,j] = -1;
                }
                else if (maze[i, j] == 4 && saltCount<saltNum)
                {
                    Instantiate(salt, new Vector3(j*width, 0, i*width), Quaternion.identity);
                    saltCount++;
                    maze[i,j] = -1;
                }
                else if (maze[i, j] == 5 && eyeballsJarCount<eyeballsJarNum)
                {
                    Instantiate(eyeballsJar, new Vector3(j*width, -0.19f, i*width), Quaternion.identity);
                    eyeballLetter.transform.position = new Vector3(j*width+0.25f, 0.01f, i*width+0.25f);
                    eyeballsJarCount++;
                    maze[i,j] = -1;
                }
                else if (maze[i, j] == 6 && witheredFlowerCount<witheredFlowerNum)
                {
                    Instantiate(witheredFlower, new Vector3(j*width, -0.05f, i*width), Quaternion.identity);
                    witheredFlowerCount++;
                    maze[i,j] = -1;
                }
                else if (maze[i, j] == 7 && NPC4Count<NPCNum)
                {
                    //Instantiate(NPC3, new Vector3(j*width, 0, i*width), Quaternion.identity);
                    NPC4.transform.position = new Vector3(j*width, 0, i*width);
                    NPC4Count++;
                    maze[i,j] = -1;
                }
                else if (maze[i, j] == 7 && NPC3Count<NPCNum)
                {
                    //Instantiate(NPC3, new Vector3(j*width, 0, i*width), Quaternion.identity);
                    NPC3.transform.position = new Vector3(j*width, 0, i*width);
                    NPC3Count++;
                    maze[i,j] = -1;
                }
                else if (maze[i, j] == 7 && NPC2Count<NPCNum)
                {
                    //Instantiate(NPC2, new Vector3(j*width, 0, i*width), Quaternion.identity);
                    NPC2.transform.position = new Vector3(j*width, 0, i*width);
                    NPC2Count++;
                    maze[i,j] = -1;
                }
                else if (maze[i, j] == 7 && NPC1Count<NPCNum)
                {
                    //Instantiate(NPC2, new Vector3(j*width, 0, i*width), Quaternion.identity);
                    NPC1.transform.position = new Vector3(j*width, 0, i*width);
                    NPC1Count++;
                    maze[i,j] = -1;
                }
                else if (maze[i, j] == 7 && NPC5Count<NPCNum)
                {
                    //Instantiate(NPC3, new Vector3(j*width, 0, i*width), Quaternion.identity);
                    NPC5.transform.position = new Vector3(j*width, 0, i*width);
                    NPC5Count++;
                    maze[i,j] = -1;
                }
                //letters
                else if(maze[i,j] != 1 && Random.value < .02f && letterCount<letterNum){
                    if(letterCount==0){
                        letter1.transform.position = new Vector3(j*width, 0.01f, i*width);
                    } else if(letterCount==1){
                        letter3.transform.position = new Vector3(j*width, 0.01f, i*width);
                    } else if(letterCount==2){
                        letter5.transform.position = new Vector3(j*width, 0.01f, i*width);
                    } else if(letterCount==3){
                        letter6.transform.position = new Vector3(j*width, 0.01f, i*width);
                    } else if(letterCount==4){
                        letter8.transform.position = new Vector3(j*width, 0.01f, i*width);
                    }  else if(letterCount==5){
                        letter9.transform.position = new Vector3(j*width, 0.01f, i*width);
                    }  else if(letterCount==6){
                        letter10.transform.position = new Vector3(j*width, 0.01f, i*width);
                    }
                    letterCount++;
                }
                // Spawns NPCs and eyeball jar at first possible spot if random generation fails
                if(NPC3Count == 0 && i==0){
                    for (int m = rMax-15; m >= 0; m--)
                    {
                        for (int n = 2; n <= cMax; n++)
                        {
                            if(maze[m,n] != 1 && maze[m,n] != -1 && NPC3Count == 0){
                                //Instantiate(NPC3, new Vector3(n*width, 0, m*width), Quaternion.identity);
                                NPC3.transform.position = new Vector3(n*width, 0, m*width);
                                NPC3Count++;
                                maze[m,n] = -1;
                            }
                        }
                    }
                }
                if(NPC4Count == 0 && i==0){
                    for (int m = rMax-3; m >= 0; m--)
                    {
                        for (int n = 7; n <= cMax; n++)
                        {
                            if(maze[m,n] != 1 && maze[m,n] != -1 && NPC4Count == 0){
                                //Instantiate(NPC3, new Vector3(n*width, 0, m*width), Quaternion.identity);
                                NPC4.transform.position = new Vector3(n*width, 0, m*width);
                                NPC4Count++;
                                maze[m,n] = -1;
                            }
                        }
                    }
                }
                if(NPC5Count == 0 && i==0){
                    for (int m = rMax-5; m >= 0; m--)
                    {
                        for (int n = 2; n <= cMax; n++)
                        {
                            if(maze[m,n] != 1 && maze[m,n] != -1 && NPC5Count == 0){
                                //Instantiate(NPC3, new Vector3(n*width, 0, m*width), Quaternion.identity);
                                NPC5.transform.position = new Vector3(n*width, 0, m*width);
                                NPC5Count++;
                                maze[m,n] = -1;
                            }
                        }
                    }
                }
                if((NPC2Count==0||eyeballsJarCount==0) && i==0){              
                for (int m = rMax-5; m >= 0; m--)
                {
                    for (int n = 5; n <= cMax; n++)
                    {
                        if(maze[m,n] != 1 && maze[m,n] != -1  && NPC2Count == 0){
                            //Instantiate(NPC2, new Vector3(n*width, 0, m*width), Quaternion.identity);
                            NPC2.transform.position = new Vector3(n*width, 0, m*width);
                            NPC2Count++;
                            maze[m,n] = -1;
                        }
                        else if(maze[m,n] != 1 && maze[m,n] != -1  && eyeballsJarCount == 0){
                            Instantiate(eyeballsJar, new Vector3(n*width, 0, m*width), Quaternion.identity);
                            eyeballLetter.transform.position = new Vector3(n*width+0.25f, 0.01f, m*width+0.25f);
                            eyeballsJarCount++;
                            maze[m,n] = -1;
                        }
                    }
                }
                }
                //torches
                // if(maze[i,j]==1 && j%2==0 && Random.value < .12f && i!=0 && i!=rMax && j!=0 && j!=cMax){
                //     if(maze[i-1,j]==0){
                //         Instantiate(torch, new Vector3(j*width, 4, (i-0.5f)*width), Quaternion.Euler(new Vector3(-27, 0, 0)));
                //     } else if (maze[i, j+1]==0){
                //         Instantiate(torch, new Vector3((j-0.5f)*width, 4, i*width), Quaternion.Euler(new Vector3(-27, 90, 0)));
                //     }
                // }
                //box colliders for walls
                if(maze[i,j]==1){
                    //Instantiate(boxCol, new Vector3(j*width, 0, i*width), Quaternion.identity);
                }

            }                   
        }
        candleCount=0;
        dollCount=0;
        NPC1Count=0;
        NPC2Count=0;
        NPC3Count=0;
        NPC4Count=0;
        NPC5Count=0;
        eyeballsJarCount=0;
        saltCount=0;
        witheredFlowerCount=0;
        letterCount=0;
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
