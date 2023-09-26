// Followed tutorial from https://www.kodeco.com/82-procedural-generation-of-mazes-with-unity?page=2

using System;
using UnityEngine;

[RequireComponent(typeof(MazeConstructor))]               // 1

public class GameController : MonoBehaviour
{
    private MazeConstructor generator;

    void Start()
    {
        generator = GetComponent<MazeConstructor>();      // 2
        generator.GenerateNewMaze(31, 31);
    }
}

