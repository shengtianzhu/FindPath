using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dijkstra : MonoBehaviour {

    public class Node
    {
        public int nIndex;                                  //点的索引
        public Vector2 vPos;                                //位置
        public List<Node> ltLinkNode = new List<Node>();    //连接的节点
        public string strName;                              //节点名称，这个用于调试和编辑使用
        public List<Edge> ltLinkEdge = new List<Edge>();    //连接的边
        public Edge AddLink(Node _Node, int nWeight)
        {
            ltLinkNode.Add(_Node);

            Edge _Edge = new Edge(this, _Node, nWeight);
            ltLinkEdge.Add(_Edge);
            _Node.ReversAddLink(this, nWeight);

            return _Edge;
        }

        private void ReversAddLink(Node _Node, int nWeight)
        {
            Edge _Edge = new Edge(this, _Node, nWeight);
            ltLinkEdge.Add(_Edge);
        }
    }
    public class Edge
    {
        public Node StartNode;              //起始点
        public Node EndNode;                //终点
        public int Weight = int.MaxValue;   //边的权重   
        public Edge(Node _StartNode, Node _EndNode, int _Weight)
        {
            StartNode = _StartNode;
            EndNode = _EndNode;
            Weight = _Weight;
        }

        public Edge()
        {
            Weight = int.MaxValue;
        }
        
    }

    public class Graph
    {
        public List<Edge> m_ltAllEdge = new List<Edge>();
        public List<Node> m_ltAllNode = new List<Node>();
        /// <summary>
        /// 
        /// </summary>
        public Graph()
        {
            Node A = new Node();
            A.strName = "A";
            A.nIndex = 0;
            m_ltAllNode.Add(A);

            Node B = new Node();
            B.strName = "B";
            B.nIndex = 1;
            m_ltAllNode.Add(B);

            Node C = new Node();
            C.strName = "C";
            C.nIndex = 2;
            m_ltAllNode.Add(C);

            Node D = new Node();
            D.strName = "D";
            D.nIndex = 3;
            m_ltAllNode.Add(D);

            Node E = new Node();
            E.strName = "E";
            E.nIndex = 4;
            m_ltAllNode.Add(E);

            Node F = new Node();
            F.strName = "F";
            F.nIndex = 5;
            m_ltAllNode.Add(F);

            Node G = new Node();
            G.strName = "G";
            G.nIndex = 6;
            m_ltAllNode.Add(G);

            m_ltAllEdge.Add(A.AddLink(B, 5));
            m_ltAllEdge.Add(A.AddLink(C, 1));
            
            m_ltAllEdge.Add(B.AddLink(E, 4));
            m_ltAllEdge.Add(B.AddLink(D, 1));
            m_ltAllEdge.Add(B.AddLink(C, 3));
            m_ltAllEdge.Add(B.AddLink(G, 1));

            m_ltAllEdge.Add(C.AddLink(E, 2));
            m_ltAllEdge.Add(C.AddLink(D, 6));

            m_ltAllEdge.Add(D.AddLink(F, 2));
            
            m_ltAllEdge.Add(E.AddLink(F, 6));
            

            
        }

        public class Path
        {
            public int nW = int.MaxValue;       //当前的最短路径
            public Path ParentNode = null;      //当前路径下的父节点
            public int nIndex;                  //节点索引
            
        }

        /// <summary>
        /// Dijksra算法思路：用的是贪心的策略，列表ltOpen表示所有的节点，用一个数组dis存放起始点到其他点的最短距离。
        /// 用一个堆栈ltClose来存放已访问过且有最短路径节点。用一个数组used来存放被访问过的点
        /// 1、初始化dis、used数组。并把起始点索引的dis设置为0；其他的默认是无穷大
        /// 2、如果ltOpen不为空：
        ///     把当前节点压进ltClose。把当前节点的used标准设置为true。表示为已经访问过
        ///     找到与当前节点连接的节点。如果连接节点没有被访问过。则用当前节点计算到连接点的距离temp。
        ///     如果该距离小于dis存放的距离。则用temp来更新dis数组的值
        ///     找出与当前节点最短且未被访问的点设置为当前点。重复第二步。
        ///     如果当前节点所有的连接点都被访问过。则弹出此节点。并用堆栈最上面的节点作为当前节点。重复第二步
        ///  3、如果ltOpen为空
        ///     所有节点都遍历过一遍。此时得到的dis即为从起始点出发到各个点的最短路径
        /// </summary>
        /// <param name="nStartIndex"></param>
        /// <param name="nEndIndex"></param>
        /// <returns></returns>
        public IEnumerator DijkstraFun(int nStartIndex, int nEndIndex)
        {
            Stack<Node> ltClose = new Stack<Node>();
            List<Node> ltOpen = new List<Node>(m_ltAllNode);

            Path[] dis = new Path[ltOpen.Count];
            bool[] used = new bool[ltOpen.Count];
            
            for (int i = 0; i < ltOpen.Count; ++i)
            {
                Node aNode = ltOpen[i];
                dis[aNode.nIndex] = new Path();
                dis[aNode.nIndex].nIndex = i;
                used[aNode.nIndex] = false;
            }

            dis[nStartIndex].nW = 0;
            
            Node curNode = ltOpen[nStartIndex];
            ltOpen.Remove(curNode);

            while (ltOpen.Count > 0)
            {
                ltClose.Push(curNode);

                used[curNode.nIndex] = true;

                List<Edge> _ltLink = curNode.ltLinkEdge;
                Edge aEdge = new Edge();
                for (int i = 0; i < _ltLink.Count; ++i)
                {
                    if (used[_ltLink[i].EndNode.nIndex])
                    {
                        continue;
                    }

                    if (aEdge.Weight > _ltLink[i].Weight)
                    {
                        aEdge.StartNode = _ltLink[i].StartNode;
                        aEdge.EndNode = _ltLink[i].EndNode;
                        aEdge.Weight = _ltLink[i].Weight;
                    }

                    int tem = dis[curNode.nIndex].nW + _ltLink[i].Weight;
                    if (tem < dis[_ltLink[i].EndNode.nIndex].nW)
                    {
                        dis[_ltLink[i].EndNode.nIndex].nW = tem;
                        dis[_ltLink[i].EndNode.nIndex].ParentNode = dis[curNode.nIndex];
                    }
                }
                if (aEdge.Weight == int.MaxValue)
                {
                    curNode = ltClose.Pop();
                    curNode = ltClose.Pop();
                }
                else
                {
                    curNode = aEdge.EndNode;
                    ltOpen.Remove(curNode);
                }
                
                yield return new WaitForSeconds(2);

            }
            string strPath = "";
            strPath = nEndIndex.ToString();
            Path _ParentNode = dis[nEndIndex].ParentNode;
            while(null != _ParentNode)
            {
                strPath += " " + _ParentNode.nIndex;
                _ParentNode = _ParentNode.ParentNode;
            }
            
            Debug.Log(strPath);
            string strPrint = "";
            for (int i = 0; i < m_ltAllNode.Count; ++i)
            {
                strPrint += " dis[" + i + "] = " + dis[i].nW;
            }
            Debug.Log(strPrint);
            yield return new WaitForSeconds(2);
        }
    }

    Graph aGraph;
    // Use this for initialization
    void Start () {
        aGraph = new Graph();
        StartCoroutine(aGraph.DijkstraFun(1, 5));

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
