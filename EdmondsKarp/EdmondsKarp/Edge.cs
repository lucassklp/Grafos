using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmondsKarp
{
    public class Edge
    {
        public Vertex From;
        public Vertex To;
        public int Capacity;
        public int Load;

        public Edge(Vertex From, Vertex To, int Capacity)
        {
            this.From = From;
            this.To = To;
            this.Capacity = Capacity;
            this.Load = 0;
        }




    }
}
