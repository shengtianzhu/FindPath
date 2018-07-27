using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dijkstra : MonoBehaviour {

    public class Node
    {
        public int nIndex;
        public Vector2 vPos;
        public List<Node> ltLinkNode = new List<Node>();
        public string strName;
        public List<Edge> ltLinkEdge = new List<Edge>();
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
        public Node StartNode;
        public Node EndNode;
        public int Weight = int.MaxValue;
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
            public int nW = int.MaxValue;
            public Path ParentNode = null;
            public int nIndex;
            
        }
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
                Debug.Log("aaaaaa " + curNode.strName);
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
                    Debug.Log("CurNode = " + curNode.strName);
                    curNode = ltClose.Pop();
                    curNode = ltClose.Pop();
                }
                else
                {
                    curNode = aEdge.EndNode;
                    Debug.Log("NextNode = " + curNode.strName);
                    ltOpen.Remove(curNode);
                }


                string _strPrint = "";
                for (int i = 0; i < m_ltAllNode.Count; ++i)
                {
                    _strPrint += " dis[" + i + "] = " + dis[i].nW;
                }
                Debug.Log(_strPrint);
                yield return new WaitForSeconds(2);

            }
            Debug.Log("end");
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
