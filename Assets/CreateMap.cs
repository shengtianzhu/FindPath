using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeItem
{
    public int x;   //在二维数组中的索引
    public int y;   //在二维数组中的索引

    public int nGCost;
    public int nHCost;
    public int AllCost
    {
        get { return nGCost + nHCost; }
    }

    public bool bWall;  //是否为墙。即阻挡

    public Vector2 vPos;    //坐标值

    public NodeItem parent; //父节点

    public NodeItem(bool isWall, Vector3 pos, int x, int y)
    {
        this.bWall = isWall;
        this.vPos = pos;
        this.x = x;
        this.y = y;
    }
}
public class CreateMap : MonoBehaviour {

    int[,] dir_4 = new int[4, 2] { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 }, };
    private List<GameObject> ltGame = new List<GameObject>();
    private NodeItem[,] grid;
    public int nWidth = 30;
    public int nHeight = 20;
    public int nClearAmount = 50;
    public GameObject Wall;
    public GameObject Floot;
    public GameObject Node;

    public GameObject StartPos;
    public GameObject DestPos;

    FindPath m_aFindPath;
    // Use this for initialization
    void Start () {
        m_aFindPath = GetComponent<FindPath>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        if(GUILayout.Button("重新生成地图"))
        {
            CreateMapByOutSide(nWidth, nHeight, nClearAmount);
        }

        if (GUILayout.Button("寻路"))
        {
            List<NodeItem> lt = m_aFindPath.FindingPath(grid, getItem(StartPos.transform.position), getItem(DestPos.transform.position));

            CreatePath(lt);
        }

    }
    public void CreateMapByOutSide(int width, int height, int clearAmount)
    {
        int[,] map;
        float seed = Time.realtimeSinceStartup;
        map = MapFunctions.GenerateArray(width, height, true);
        //Next generate the random walk cave
        map = MapFunctions.RandomWalkCave(map, seed, clearAmount);

        grid = new NodeItem[width, height];
        for(int i = 0; i < width; ++i)
        {
            for(int j = 0; j < height; ++j)
            {
                bool bWall = map[i, j] == 0;
                Vector3 vPos = new Vector3(i + 0.5f, 0, j + 0.5f);
                var aItem = new NodeItem(bWall, vPos, i, j);
                grid[i, j] = aItem;
            }
        }
        CreateCube();
    }

    public List<NodeItem> getNeibourhood(NodeItem node)
    {
        List<NodeItem> list = new List<NodeItem>();
        for (int i = 0; i < 4; ++i)
        {
            int x = node.x + dir_4[i, 0];
            int y = node.y + dir_4[i, 1];
            // 判断是否越界，如果没有，加到列表中
            if (x < nWidth && x >= 0 && y < nHeight && y >= 0)
                list.Add(grid[x, y]);
        }
        return list;
    }

    public NodeItem getItem(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x) ;
        int y = Mathf.FloorToInt(position.z);
        x = Mathf.Clamp(x, 0, nWidth - 1);
        y = Mathf.Clamp(y, 0, nHeight - 1);
        return grid[x, y];
    }

    List<GameObject> ltPath = new List<GameObject>();
    private void CreatePath(List<NodeItem> ltNode)
    {
        foreach (var _a in ltPath)
        {
            Destroy(_a);
        }

        ltPath.Clear();

        for (int i = 0; i < ltNode.Count; i++)
        {
            GameObject cube = GameObject.Instantiate(Node);
            cube.transform.parent = transform;
            cube.transform.position = new Vector3(ltNode[i].x + 0.5f, 1, ltNode[i].y + 0.5f);
            ltPath.Add(cube);
        }
    }

    private void CreateCube()
    {
        foreach (var _a in ltGame)
        {
            Destroy(_a);
        }

        ltGame.Clear();

        for (int i = 0; i < nWidth; i++)
        {
            for (int j = 0; j < nHeight; j++)
            {
                if (grid[i,j].bWall)
                {
                    GameObject cube = GameObject.Instantiate(Wall);
                    cube.transform.parent = transform;
                    cube.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f);
                    ltGame.Add(cube);
                }
                else
                {
                    GameObject cube = GameObject.Instantiate(Floot);
                    cube.transform.parent = transform;
                    cube.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f);
                    ltGame.Add(cube);
                }
            }
        }
    }
}
