using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A*算法
public class FindPath : MonoBehaviour {

    int[,] dir_4 = new int[4, 2] { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 }, };
    CreateMap m_aCreateMap;
    // Use this for initialization
    void Start () {
        m_aCreateMap = GetComponent<CreateMap>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// A*寻路算法。算法用一个Open和一个Close列表来存储没有访问的节点和访问过的节点.
    /// 当Close列表里面存在目标点或者Open列表为空时。算法结束
    /// 开始的时候。把起始点放到Open列表里面。
    /// 当Open列表不为空时。找出列表里面的节点AllCost最小，且nEndCost也最小的点作为当前节点
    /// 把当前节点移出Open列表。并放进Close列表里面，并判断当前节点
    /// 1、当前节点为目标点。找到路径。算法结束
    ////2、当前节点不是目标点，则查找它的相邻节点。
    ///     从当前节点重新计算相邻节点的StartCost，
    ///     如果相邻节点新的StartCost值比旧的StartCost值小或者没有在Open列表里面。则更新StartCost和nEndCost。
    ///     并把没有在Open列表里面的节点加入到Open列表里
    ///  
    /// </summary>
    /// <param name="map"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public List<NodeItem> FindingPath(NodeItem[,]map, NodeItem startNode, NodeItem endNode)
    {
        List<NodeItem> openList = new List<NodeItem>();
        List<NodeItem> closeList = new List<NodeItem>();
        openList.Add(startNode);
        startNode.nStartCost = 0;
        startNode.nEndCost = getDistanceNodes(startNode, endNode);
        while (openList.Count > 0)
        {
            NodeItem curNode = openList[0];
            for(int i = 1; i < openList.Count; ++i)
            {
                if(openList[i].AllCost < curNode.AllCost && openList[i].nEndCost < curNode.nEndCost)
                {
                    curNode = openList[i];
                }
            }

            openList.Remove(curNode);
            closeList.Add(curNode);
            if(curNode == endNode)
            {
                return generatePath(startNode, endNode);
            }

            List<NodeItem> ltNode = m_aCreateMap.getNeibourhood(curNode);
            for (int i = 0; i < ltNode.Count; ++i)
            {
                NodeItem _tempNode = ltNode[i];
                if (_tempNode.bWall || closeList.Contains(_tempNode))
                    continue;

                int newCost = curNode.nStartCost + getDistanceNodes(curNode, _tempNode);
                if(newCost < _tempNode.nStartCost || !openList.Contains(_tempNode))
                {
                    _tempNode.nStartCost = newCost;
                    _tempNode.nEndCost = getDistanceNodes(_tempNode, endNode);
                    _tempNode.parent = curNode;
                    if(!openList.Contains(_tempNode))
                    {
                        openList.Add(_tempNode);
                    }
                }
            }

        }

        return generatePath(startNode, null);
    }

    int getDistanceNodes(NodeItem a, NodeItem b)
    {
        int cntX = Mathf.Abs(a.x - b.x);
        int cntY = Mathf.Abs(a.y - b.y);
        return (cntX + cntY);
    }

    List<NodeItem> generatePath(NodeItem startNode, NodeItem endNode)
    {
        List<NodeItem> path = new List<NodeItem>();
        if (endNode != null)
        {
            NodeItem temp = endNode;
            while (temp != startNode)
            {
                path.Add(temp);
                temp = temp.parent;
            }
            // 反转路径
            path.Reverse();
        }
        return path;
    }

    
}
