using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmondsKarp
{
    [Serializable]
    public class Graph
    {
        private List<Vertex> _listVertex;
        private List<Edge> _listEdges;

        public Vertex Destination { get; private set; }
        public Vertex Source{ get; private set; }

    #region Properties
    public List<Vertex> ListVertex
        {
            get {return this._listVertex;}
        }

        public List<Edge> ListEdges
        {
            get{return this._listEdges;}
        }
        #endregion

        public Graph(List<Vertex> listVertex, List<Edge> listEdge, Vertex Destination, Vertex Source)
        {
            this._listEdges = listEdge;
            this._listVertex = listVertex;
            this.Destination = Destination;
            this.Source = Source;
        }

        public Graph() 
        {
            this._listEdges = new List<Edge>();
            this._listVertex = new List<Vertex>();
        }

        public void AddVertex(Vertex item)
        {
            this._listVertex.Add(item);
        }

        public void AddEdge(Edge item)
        {
            this._listEdges.Add(item);
        }


        public List<Vertex> Neighbors(Vertex u)
        {
            List<Vertex> Neighbors = new List<Vertex>();
            List<Edge> Edges = this._listEdges.FindAll(p => p.From == u);

            foreach (var item in Edges)
                Neighbors.Add(item.To);

            return Neighbors;
        }

        public int GetCapacity(Vertex u, Vertex v)
        {
            return this._listEdges.Find(p => p.From == u && p.To == v || p.From == v && p.To == u ).Capacity;
        }

        public void SetEdge(Vertex u, Vertex v, int n)
        {
            Edge toSet = this._listEdges.Find(p => p.From == u && p.To == v);
            if(toSet != null)
                toSet.Load += n;

        }

        public int GetLoad(Vertex u, Vertex v)
        {
            return this._listEdges.Find(p => p.From == u && p.To == v).Load;
        }
    }
}
