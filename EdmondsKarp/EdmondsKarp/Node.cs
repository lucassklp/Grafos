using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmondsKarp
{
    [Serializable]
    public class Vertex
    {
        public string Nome;
        public Point Coordenada;
        public Color Cor { get; set; }

        public Vertex(string Nome, Point Coordenada)
        {
            this.Nome = Nome;
            this.Coordenada = Coordenada;
        }
    }
}
