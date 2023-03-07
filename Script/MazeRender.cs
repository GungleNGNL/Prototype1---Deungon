using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MazeRender : MonoBehaviour
{
    private static MazeRender _currentMazeRender;
    public static MazeRender Instance
    {
        get
        {
            if (_currentMazeRender == null)
                _currentMazeRender = new MazeRender();
            return _currentMazeRender;
        }
        set
        {
            _currentMazeRender = value;
        }
    }

    public GameObject player, monster, monsterB, noUse, point;
    public GameObject damageDisplay;
    public GameObject nothing, plant, start, end;
    //Asset
    [SerializeField]private Transform gPlayer;
    public List<Transform> gMonster;
    public GameObject[,] pointMap;
    [Range(10, 30)]
    [SerializeField]private int width;
    [Range(10, 30)]
    [SerializeField] private int height;
    [Range(5, 200)]
    [SerializeField] private int zoneNum;

    private void Awake()
    {
        if (_currentMazeRender == null)
            Instance = this;
    }

    public int CZN;
    private void Start()
    {
        Application.targetFrameRate = 80;
        var maze = MazeGenerator.Instance.GenerateMaze(width, height, zoneNum);
        RenderMaze(maze);
        Maze.Instance.setMaze(maze);       
        //CZN = MazeGenerator.Instance.ZN;  //current zone num
        RenderMazeItemMap(Maze.Instance.GetMazeItemsMap());
        RenderCharMap(Maze.Instance.GetChatMap());
        UpdateMap();
        Debug.Log("Get Maze with size " + maze.GetLength(0) + " " + maze.GetLength(1));
        Debug.Log("Get MazeItemMap with size " + Maze.Instance.GetMazeItemsMap().GetLength(0) + " " + Maze.Instance.GetMazeItemsMap().GetLength(1));
    }

    public void RenderMaze(State[,] maze)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var zone = maze[i, j];
                var position = new Vector3(j * 3, i * 3, 0);
                switch ((int)zone)
                {
                    case (int)State.nothing:
                        //Debug.Log("nothing" + j + i);
                        GameObject obj0 = Instantiate(nothing, transform);
                        obj0.transform.position = position;
                        break;
                    case (int)State.endZone:
                        //Debug.Log("END" + j + i);
                        GameObject obj = Instantiate(end, transform);
                        obj.transform.position = position;
                        break;
                    case (int)State.startZone:
                        //Debug.Log("START" + j + i);
                        GameObject obj1 = Instantiate(start, transform);
                        obj1.transform.position = position;
                        break;
                    case (int)State.normalZone:
                        //Debug.Log("normal" + j + i);
                        GameObject obj2 = Instantiate(plant, transform);
                        obj2.transform.position = position;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void RenderMazeItemMap(MazeItem[,] itemMap)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var zone = itemMap[i, j];
                var position = new Vector3(j * 3, i * 3, -0.5f);
                switch (zone)
                {
                    case MazeItem.nothing:
                        var obj = Instantiate(noUse, transform).transform;
                        obj.position = position;
                        break;
                    case MazeItem.point:
                        var obj2 = Instantiate(point, transform);
                        obj2.transform.position = position;
                        pointMap[i, j] = obj2;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void RenderCharMap(Char[,] charMap)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var zone = charMap[i, j];
                var position = new Vector3(j * 3, i * 3, -1);
                switch (zone)
                {
                    case Char.nothing:
                        break;
                    case Char.monster:
                        break;
                    case Char.player:
                        if (gPlayer == null)
                        {
                            gPlayer = Instantiate(player, transform).transform;
                            gPlayer.position = position;
                            Debug.Log("game player set in " + i + j);
                        }
                        else
                        {
                            gPlayer.gameObject.SetActive(true);
                        }
                        var pos = new Vector3(position.x, position.y, Camera.main.transform.position.z);
                        Camera.main.GetComponent<CameraFollow>().NewDes(pos);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void UpdateMap()
    {
        if (gPlayer == null) return;
        if (!Maze.Instance.GetPlayer().isDead)
        {
            var playerPos = Maze.Instance.GetPlayer().c.pos;
            gPlayer.position = new Vector3(playerPos.x * 3, playerPos.y * 3, -1);
            gPlayer.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + Maze.Instance.GetPlayer().power;
            var orgScale = gPlayer.transform.localScale;
            float scale = (float)((float)Maze.Instance.GetPlayer().c.mass / (float)Maze.Instance.GetPlayer().maxMass) * 2.0f;
            if (scale < 0.5f)
                scale = 0.5f;
            gPlayer.transform.localScale = new Vector3(scale, scale, orgScale.z);
        }
        List<Enemy> enemyPos = Maze.Instance.GetEnemy();
        for(int i = 0; i < gMonster.Count; i++)
        {
            var pos = enemyPos[i].c.pos;
            gMonster[i].position = new Vector3(pos.x * 3, pos.y * 3, -1);
            gMonster[i].GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + enemyPos[i].power;
            var orgScale = gMonster[i].localScale;
            float scale2 = (float)((float)enemyPos[i].c.mass / (float)Maze.Instance.GetPlayer().maxMass) * 3.0f;
            if (scale2 < 2.0f)
                scale2 = 2.0f;
            gMonster[i].transform.localScale = new Vector3(scale2, scale2, orgScale.z);
        }
        Camera.main.GetComponent<CameraFollow>().NewDes(gPlayer.position);
    }

    public void destroyPoint(int x, int y)
    {
        Debug.Log("destroy point in " + y + " " + x);
        if(pointMap[y, x] != null)
        Destroy(pointMap[y, x]);
        pointMap[y, x] = null;
    }

    public void spawnMonster(Vector2 pos, int type)
    {
        var position = new Vector3(pos.x * 3, pos.y * 3, -1);
        if (type == 2)
        {
            var obj = Instantiate(monsterB, transform).transform;
            obj.position = position;
            gMonster.Add(obj);
        }
        else
        {
            var obj = Instantiate(monster, transform).transform;
            obj.position = position;
            gMonster.Add(obj);
        }
    }

    public void destroyMonster(int i)
    {
        Destroy(gMonster[i].gameObject);
        gMonster.RemoveAt(i);
    }

    public void destroyPlayer()
    {
        if (gPlayer != null)
            gPlayer.gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    public void generatePointMap()
    {
        pointMap = new GameObject[height, width];
    }

    IEnumerator newMaze()
    {
        var cMaze = gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < cMaze.Length; i++)
        {
            if (cMaze[i].transform.tag == "Maze" || cMaze[i].transform.tag == "Enemy")
                Destroy(cMaze[i].gameObject);
        }
        for (int i = 0; i < gMonster.Count; i++)
        {
            destroyMonster(i);
        }
        Debug.Log("Destroy Point Map with size " + height + " " + width);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                destroyPoint(y, x);
            }
        }
        gMonster.Clear();
        var maze = MazeGenerator.Instance.GenerateMaze(width, height, zoneNum);
        RenderMaze(maze);
        Maze.Instance.setMaze(maze);
        //RenderCharMap(Maze.Instance.GetChatMap());
        PlayerController.Instance.dontMove(2.0f);
        yield return new WaitForSeconds(2.0f);
        RenderMazeItemMap(Maze.Instance.GetMazeItemsMap());
        RenderCharMap(Maze.Instance.GetChatMap());
        UpdateMap();
    }
    public void NewMaze()
    {
        StartCoroutine(newMaze());
    }

    public void DisplayDmg(int damage)
    {
        damageDisplay.GetComponent<TextMeshPro>().text = "" + damage;
        damageDisplay.GetComponent<RectTransform>().position = gPlayer.position;
        damageDisplay.GetComponent<Animation>().Play();
    }
}
