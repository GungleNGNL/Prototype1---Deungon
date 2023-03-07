using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    nothing = 0,
    startZone = 1,
    endZone = 2,
    normalZone = 3,
    //unknown = 8,
}
public class MazeGenerator
{
    private static MazeGenerator currentMaze;
    public static MazeGenerator Instance
    {
        get
        {
            if (currentMaze == null)
                currentMaze = new MazeGenerator();
            return currentMaze;
        }
    }

    public int ZN;
    public struct Position
    {
        public int X;
        public int Y;
    }

    public State[,] GenerateMaze(int width, int height, int zoneNum)
    {
        if (zoneNum > width * height - 1)
            zoneNum = width * height - 1;
        State[,] maze = new State[height, width];
        var rng = new System.Random();
        var startPoint = new Position { X = rng.Next(0, width), Y = rng.Next(0, height) };
        maze[startPoint.X, startPoint.Y] = State.startZone;
        int currentZoneNum = 0;
        Vector2 dir = GetDir(startPoint, width, height);
        var lastPoint = startPoint;
        int randomLength = 0;
        while (zoneNum > currentZoneNum)
        {
            if(randomLength == 0 || lastPoint.X + (int)dir.x >= width || lastPoint.Y + (int)dir.y >= height || lastPoint.X + (int)dir.x < 0 || lastPoint.Y + (int)dir.y < 0)
            {
                dir = GetDir(lastPoint, width, height);
                randomLength = rng.Next(3, (int)width/2);
            }
            else
            {
                if (maze[lastPoint.X + (int)dir.x, lastPoint.Y + (int)dir.y] == State.nothing)
                {
                    maze[lastPoint.X + (int)dir.x, lastPoint.Y + (int)dir.y] = State.normalZone;
                    currentZoneNum++;
                    lastPoint = new Position { X = lastPoint.X + (int)dir.x, Y = lastPoint.Y + (int)dir.y };
                    randomLength--;
                }
                else
                {
                    lastPoint = new Position { X = lastPoint.X + (int)dir.x, Y = lastPoint.Y + (int)dir.y };
                    randomLength--;
                }
            }
        }
        ZN = currentZoneNum;
        maze[lastPoint.X, lastPoint.Y] = State.endZone;
        return maze;
    }

    private int lastDir;

    private Vector2 GetDir(Position pos, int width, int height)
    {
        bool haveResult = false;
        Vector2 dir = new Vector2();
        var rng = new System.Random();
        while (!haveResult)
        {
            int _dir = rng.Next(0, 4);
            switch (_dir)
            {
                case 0: //+X
                    if (pos.X + 3 < width && lastDir != 0)
                    {
                        haveResult = true;
                        dir = new Vector2(1, 0);
                        lastDir = 0;
                    }
                    break;
                case 1: //-X
                    if (pos.X - 3 >= 0 && lastDir != 1)
                    {
                        haveResult = true;
                        dir = new Vector2(-1, 0);
                        lastDir = 1;
                    }
                    break;
                case 2: //+Y
                    if (pos.Y + 3 < height && lastDir != 2)
                    {
                        haveResult = true;
                        dir = new Vector2(0, 1);
                        lastDir = 2;
                    }
                    break;
                case 3: //-Y
                    if (pos.Y - 3 >= 0 && lastDir != 3)
                    {
                        haveResult = true;
                        dir = new Vector2(0, -1);
                        lastDir = 3;
                    }
                    break;
            }
        }
        return dir;
    }
}
