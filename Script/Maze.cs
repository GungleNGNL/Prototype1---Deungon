using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    private static Maze _currentMaze;
    public static Maze Instance
    {
        get
        {
            if (_currentMaze == null)
                _currentMaze = new Maze();
            return _currentMaze;
        }
    }
    int height;// maze
    int width;// maze
    private State[,] maze;
    private MazeItem[,] itemMap;
    private Char[,] charMap;
    private Node[,] grid; // astar
    private Player player;
    private List<Enemy> enemyList;
    private Enemy[] enemyType;
    private int mazeLv = 1;

    public void setMaze(State[,] maze)
    {
        this.maze = new State[maze.GetLength(0), maze.GetLength(1)];
        this.maze = maze;
        height = maze.GetLength(0);
        width = maze.GetLength(1);
        GenerateMazeMap();
    }

    public void GenerateMazeMap()
    {
        if(player == null)
        {
            player = new Player();
            player.c.mass = 10;
            player.c.scaler = 1;
        }

        enemyList = new List<Enemy>();
        itemMap = new MazeItem[height, width];
        charMap = new Char[height, width];
        ImportMazeData(height, width);
        GenerateMazeEnemy();
        GeneratePoint();
        GenerateAStarGrid();
    }

    public void ImportMazeData(int height, int width)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var zone = maze[i, j];
                switch (zone)
                {
                    case State.nothing:
                        break;
                    case State.normalZone:
                        itemMap[i, j] = MazeItem.canUse;
                        charMap[i, j] = Char.canUse;
                        break;
                    case State.startZone:
                        itemMap[i, j] = MazeItem.canUse;
                        charMap[i, j] = Char.player;
                        player.c.pos = new Vector2(j, i);
                        break;
                    case State.endZone:
                        itemMap[i, j] = MazeItem.canUse;
                        charMap[i, j] = Char.canUse;
                        break;
                }
            }
        }
    }

    public void GenerateMazeEnemy()
    {
        var rng = new System.Random();
        int enemyNum = rng.Next(4 + (mazeLv / 2), 7 + mazeLv);        
        for (int i = 0; i < enemyNum; i++)
        {
            GenerateEnemy();
        }
        Debug.Log("Make " + enemyList.Count + " Enemy!");
    }

    public void GenerateEnemy()
    {
        var rng2 = new System.Random();
        int type = rng2.Next(0, 11);
        Debug.Log("Generate type " + 0 + " Enemy");
        var nEnemy = new Enemy(enemyList.Count);
        if(type < 8) // 3
        {
            nEnemy.type = Char.monster;
            nEnemy.c.pos = GenerateEnemyPos();
            nEnemy.c.massLv = rng2.Next(mazeLv, mazeLv + 2);
            nEnemy.c.mass = 5 * nEnemy.c.massLv;
            nEnemy.c.scaler = 1;
            nEnemy.c.speed = 1;
        }
        else    //4
        {
            nEnemy.type = Char.monsterB;
            nEnemy.c.pos = GenerateEnemyPos();
            nEnemy.c.massLv = rng2.Next(mazeLv, mazeLv + 1);
            nEnemy.c.mass = 3 * nEnemy.c.massLv;
            nEnemy.c.scaler = 1;
            nEnemy.c.speed = 2;
        }
        enemyList.Add(nEnemy);
        charMap[(int)nEnemy.c.pos.y, (int)nEnemy.c.pos.x] = nEnemy.type;
        MazeRender.Instance.spawnMonster(nEnemy.c.pos, nEnemy.c.speed);
        Debug.Log("Make 1 Enemy in " + nEnemy.c.pos.y + " " + nEnemy.c.pos.x);
    }

    public void GeneratePoint()
    {
        var rng = new System.Random();
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if (itemMap[y, x] == MazeItem.canUse)
                {
                    if(rng.Next(0, 11) > 1)
                    {
                        itemMap[y, x] = MazeItem.point;
                    }
                }
            }
        }
        MazeRender.Instance.generatePointMap();
    }

    Vector2 GenerateEnemyPos()
    {
        Vector2 pos = new Vector2();
        bool havePos = false;
        var rng = new System.Random();
        while (!havePos)
        {
            var x = rng.Next(0, width);
            var y = rng.Next(0, height);
            if (charMap[y, x] == Char.canUse && GetPlayerDistance(x, y) > 6)
            {
                havePos = true;
                pos = new Vector2(x, y);
            }
        }
        return pos;
    }
    //-----------------------------A Star-----------------------------------------
    public class Node{
        public Vector2 pos;
        public Node parent;
        public float g, h;
        public Node(Vector2 pos)
        {
            this.pos = pos;
        }
        public float f
        {
            get
            {
                return g + h;
            }
        }
    }
    void GenerateAStarGrid()
    {
        grid = new Node[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[y, x] = new Node(new Vector2(x, y));
            }
        }
    }
    
    Stack<Vector2> AStar(Vector2 startPos, Vector2 desPos)
    {
        int looplimit = 2000;
        int loopTime = 0;
        Node start = grid[(int)startPos.y, (int)startPos.x];
        Node des = grid[(int)desPos.y, (int)desPos.x];
        Stack<Vector2> path = new Stack<Vector2>();

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        Node current;

        start.g = 0;
        start.h = Vector2.Distance(startPos, desPos);
        open.Add(start);

        while (open.Count > 0)
        {
            if (loopTime > looplimit)
            {
                Debug.LogError("Loop more than loop limit");
                break;
            }
            current = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].f < current.f || open[i].f == current.f)
                {
                    if (open[i].h < current.h)
                        current = open[i];
                }
            }
            if (current.pos == des.pos)
            {
                Stack<Vector2> desPath = reconstructPath(start, des);
                //Debug.Log("find des with " + desPath.Count + " nodes !");
                return desPath;
            }
            open.Remove(current);
            closed.Add(current);
            List<Node> neighbourList = new List<Node>();
            neighbourList = GetNeighbour(current.pos);
            foreach (Node neighbour in neighbourList)
            {
                if (closed.Contains(neighbour))
                {
                    continue;
                }
                var cost = current.g + Vector2.Distance(current.pos, neighbour.pos);
                //Debug.Log("have " + neighbourList.Count + " neighbour");
                //Debug.Log("debug index " + open.Contains(neighbour));
                //Debug.Log("current " + current.pos + " des " + des.pos);
                if (cost < neighbour.g || !open.Contains(neighbour))
                {
                    neighbour.g = cost;
                    neighbour.h = Vector2.Distance(neighbour.pos, desPos);
                    neighbour.parent = current;

                    if(!open.Contains(neighbour))
                    {
                        //Debug.Log("add a open instance");
                        //Debug.Log("neighbour add with pos " + neighbour.pos);
                        open.Add(neighbour);
                    }
                }
            }
            loopTime++;
        }
        //Debug.Log("loop " + loopTime);
        //Debug.Log("OpenCount " + open.Count);
       // Debug.Log("ClosedCount " + closed.Count);
        return path;
    }

    Stack<Vector2> reconstructPath(Node start, Node des)
    {
        Stack<Vector2> path = new Stack<Vector2>();
        Node currentNode = grid[(int)des.pos.y, (int)des.pos.x];
        path.Push(des.pos);
        while (currentNode.pos != start.pos)
        {
            path.Push(currentNode.pos);
            currentNode = currentNode.parent;
        }
        return path;
    }

    List<Node> GetNeighbour(Vector2 current)
    {
        int curX = (int)current.x;
        int curY = (int)current.y;
        //Debug.Log("get " + curY + " " + curX + "neighbour");
        var neighbourList = new List<Node>();

        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                var posX = curX + x;
                var posY = curY + y;
                
                if (posY < height && posX < width && posY >= 0 && posX >= 0)
                {
                    //Debug.Log("Check Nei " + posY + " " + posX);
                    if (charMap[posY, posX] != Char.nothing)
                    {
                        neighbourList.Add(grid[posY, posX]);
                    }
                }              
            }
        }
        return neighbourList;
    }
    //-----------------------------A Star-----------------------------------------
    int GetPlayerDistance(int x, int y)
    {
        int dis = (int)Vector2.Distance(new Vector2(x, y), player.c.pos);
        return dis;
    }

    public void UpdateEnemyPos()
    {
        foreach (Enemy enemy in enemyList)
        {
            for(int i = 0; i < enemy.c.speed; i++)
            {
                int dis = GetPlayerDistance((int)enemy.c.pos.x, (int)enemy.c.pos.y);
                if (dis < 5 || (enemy.path.Count == 0 && dis < 10))
                {
                    enemy.path.Clear();
                    enemy.path = AStar(enemy.c.pos, player.c.pos);
                    Debug.Log("path have " + enemy.path.Count + " nodes");
                }
                else if (enemy.path.Count == 0)
                {
                    continue;
                }
                var des = enemy.path.Peek();
                if ((int)charMap[(int)des.y, (int)des.x] > 2)
                {
                    continue;
                }
                if (des == player.c.pos)
                {
                    player.c.mass -= enemy.c.massLv;
                    MazeRender.Instance.DisplayDmg(enemy.c.massLv);
                    if (player.c.mass > 0)
                    {
                        continue;
                    }
                    //player dead
                    MazeRender.Instance.destroyPlayer();
                }
                charMap[(int)enemy.c.pos.y, (int)enemy.c.pos.x] = Char.canUse;
                enemy.c.pos = enemy.path.Pop();
                charMap[(int)enemy.c.pos.y, (int)enemy.c.pos.x] = Char.monster;
            }            
        }
        for(int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].isDead)
            {
                enemyList.RemoveAt(i);
                MazeRender.Instance.destroyMonster(i);
            }
        }
        MazeRender.Instance.UpdateMap();
        Debug.Log("MapUpdated");
    }

    public void UpdatePlayerPos(int x, int y)
    {
        Vector2 des = player.c.pos + new Vector2(x, y);
        if (CanMove((int)des.x, (int)des.y) == false)
        {
            Debug.Log("can't move");
            return;
        }
        Debug.Log(itemMap[(int)des.y, (int)des.x].ToString() + " can move");
        MapEvent(des);        
        Debug.Log("player now pos now in" + des.y + " " + des.x);
        UpdateEnemyPos();
    }

    void DestroyEnemy(int i)
    {
        enemyList[i].isDead = true;
        enemyList.RemoveAt(i);
        if (enemyList.Count < 4 + mazeLv / 2)
        {
            GenerateEnemy();
        }
    }

    public bool CanMove(int x, int y)
    {
        if (x < charMap.GetLength(1) && y < charMap.GetLength(0) && x >= 0 && y >= 0)
            if(charMap[y, x] != Char.nothing)
            {
                return true;
            }
            else
            {
                Debug.Log(y + " " + x +" ara " + charMap[y, x].ToString());
                return false;
            }
        else return false;
    }

    public void MapEvent(Vector2 des)
    {
        int x = (int)des.x;
        int y = (int)des.y;
        var zone = itemMap[y, x];
        Debug.Log("PlayerEvent in Zone-" + zone.ToString());
        var map = maze[y, x];
        var character = charMap[y, x];
        switch (character)
        {
            case Char.canUse:
                character = Char.player;
                charMap[(int)player.c.pos.y, (int)player.c.pos.x] = Char.canUse;
                player.c.pos = des;
                break;
            case Char.monster:
            case Char.monsterB:
                for (int i = 0; i < enemyList.Count; i++)
                {
                    if (enemyList[i].c.pos == des)
                    {
                        var enemy = enemyList[i];
                        if (player.power > enemy.power)
                        {
                            player.c.mass -= enemy.c.mass;
                            charMap[y, x] = Char.canUse;
                            player.AddExp(enemy.c.massLv * enemy.c.massLv);
                            DestroyEnemy(i);
                            MazeRender.Instance.destroyMonster(i);
                            charMap[y, x] = Char.player;
                            charMap[(int)player.c.pos.y, (int)player.c.pos.x] = Char.canUse;
                            player.c.pos = des;
                            break;
                        }
                        else
                        {
                            player.isDead = true;
                            charMap[(int)player.c.pos.y, (int)player.c.pos.x] = Char.canUse;
                            MazeRender.Instance.destroyPlayer();
                            return;
                        }
                    }
                }
                break;
        }

        switch (zone)
        {
            case MazeItem.canUse:
                break;
            case MazeItem.point:
                if (player.isFull()) break;
                player.c.mass += 1;
                MazeRender.Instance.destroyPoint(x, y);
                itemMap[y, x] = MazeItem.canUse;
                break;
            default:
                break;
        }
        switch (map)
        {
            case State.endZone:
                MazeRender.Instance.NewMaze();
                mazeLv++;
                break;
            default:
                break;
        }

    }

    public MazeItem[,] GetMazeItemsMap()
    {
        return itemMap;
    }

    public Char[,] GetChatMap()
    {
        return charMap;
    }


    public Player GetPlayer()
    {
        return player;
    }

    public List<Enemy> GetEnemy()
    {
        return enemyList;
    }

    public void Restart()
    {
        player = new Player();
        player.c.mass = 10;
        player.c.scaler = 1;
        player.c.massLv = 1;
        player.c.speed = 1;
       mazeLv = 1;

        GameManager.Instance.setLv(player.c.massLv);
        player.AddExp(0);        
    }
}

public enum MazeItem
{
    nothing = 0,
    canUse = 1,
    point = 2,
}

public enum Char
{
    nothing = 0,
    canUse = 1,
    player = 2,
    monster = 3,
    monsterB = 4,
}


public struct Character
{
    public int massLv;
    public int mass;
    public int scaler;
    public Vector2 pos;
    public int speed;
}

