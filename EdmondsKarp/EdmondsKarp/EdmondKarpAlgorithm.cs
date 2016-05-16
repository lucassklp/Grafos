using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmondsKarp
{
    class EdmondKarpAlgorithm
    {

        private List<Vertex> Nodes;
        private List<Edge> Vertexes;
        private Vertex Source;
        private Vertex Destination;
        public EdmondKarpAlgorithm(List<Vertex> Nodes, List<Edge> Vertexes, Vertex Source, Vertex Destination)
        {
            this.Nodes = Nodes;
            this.Vertexes = Vertexes;
            this.Source = Source;
            this.Destination = Destination;
        }



        public void GetMaxFlow()
        {




        }

    }
}
