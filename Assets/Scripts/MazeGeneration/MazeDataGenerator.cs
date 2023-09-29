// Followed tutorial from https://www.kodeco.com/82-procedural-generation-of-mazes-with-unity?page=2
// Parts of the code from the tutorial have been modified 

using System.Collections.Generic;
using UnityEngine;

public class MazeDataGenerator
{
    public float placementThreshold;    // chance of empty space
    public float itemSpawnChance;

    public MazeDataGenerator()
    {
        placementThreshold = .15f;
        itemSpawnChance = .02f;                               // 1
    }

    public int[,] FromDimensions(int sizeRows, int sizeCols)    // 2
    {
        int[,] maze = new int[sizeRows, sizeCols];
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

    for (int i = 0; i <= rMax; i++)
    {
        for (int j = 0; j <= cMax; j++)
        {
            //1
            if (i == 0 || j == 0 || i == rMax || j == cMax)
            {   
                maze[i, j] = 1;
            }
            
            //2 (Adds Empty Space in Middle of Maze as Spawn Area)
            else if (i > rMax/2 - 3 && i < rMax/2 + 3 && j > cMax/2 - 3 && j < cMax/2 + 3){

            } 
            
            //3 (Adds Empty Spaces for Enemy Spawn Points)
            else if ((i == 6 && j == 6) || (i == 27 && j == 6) || (i == 15 && j == 27)  ){
                maze[i,j] = -1;
            }
            
            //4
            else if (i % 2 == 0 && j % 2 == 0)
            {
                if (Random.value > placementThreshold)
                {
                    //3
                    maze[i, j] = 1;

                    int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                    int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                    if(maze[i+a,j+b]!=2 && maze[i+a,j+b]!=3 && maze[i+a,j+b]!=4 && maze[i+a,j+b]!=5 && maze[i+a,j+b]!=6){
                        maze[i+a, j+b] = 1;
                    }
                } 
            }
            
            //5 (Item Spawn Chance)
            if(maze[i,j]!=1){
            if (Random.value < itemSpawnChance) {
                    maze[i, j] = 2; // candle
            }
            else if (Random.value < itemSpawnChance) {
                    maze[i, j] = 3; // doll
            }
            else if (Random.value < itemSpawnChance) {
                    maze[i, j] = 4; // salt
            }
            else if (Random.value < itemSpawnChance) {
                    maze[i, j] = 5; // jar of eyeballs
            }
            else if (Random.value < itemSpawnChance) {
                    maze[i, j] = 6; // withered flower
            }
            }

        }
    }

        return maze;
    }
}
