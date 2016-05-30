using System;
using System.Collections.Generic;

namespace EdmondsKarp
{

    public class EdmondsKarp
    {
        private Graph Grafo;

        public EdmondsKarp(Graph graph)
        {
            this.Grafo = graph;
        }



        public void FindMaxFlow(Vertex source, Vertex target)
        {
            int flow = 0;
            while(true)
            {
                IDictionary<Vertex, Vertex> path;
                int capacity = BreadthFirstSearch(Grafo, source, target, out path);

                if (capacity == 0) 
                    break;
                flow += capacity;
                
                Vertex v = target;

                while (!v.Equals(source))
                {
                    Vertex u = path[v];
                    this.Grafo.SetEdge(u, v, capacity);
                    this.Grafo.SetEdge(v, u, (-1) * capacity);

                    v = u;
                }
            }


            
        }

        private int BreadthFirstSearch(Graph graph, Vertex source, Vertex target, out IDictionary<Vertex, Vertex> path)
        {
            path = new Dictionary<Vertex, Vertex>();
            IDictionary<Vertex, int> pathCapacity = new Dictionary<Vertex, int>();

            path[source] = null; // make sure source is not rediscovered
            pathCapacity[source] = int.MaxValue;

            Queue<Vertex> queue = new Queue<Vertex>();
            queue.Enqueue(source);

            while(queue.Count > 0)
            {
                Vertex dequeuedItem = queue.Dequeue();

                List<Vertex> Neighbors = graph.Neighbors(dequeuedItem);
                foreach (Vertex Neighbor in Neighbors)
                {
                    int capacity = graph.GetCapacity(dequeuedItem, Neighbor);
                    int lFlows = graph.GetLoad(dequeuedItem, Neighbor);

                    if (capacity -  lFlows > 0 && !path.ContainsKey(Neighbor))
                    {
                        path[Neighbor] = dequeuedItem;
                        pathCapacity[Neighbor] = Math.Min(pathCapacity[dequeuedItem], graph.GetCapacity(dequeuedItem, Neighbor) - graph.GetLoad(dequeuedItem ,Neighbor));
                        
                        if (!Neighbor.Equals(target)) 
                            queue.Enqueue(Neighbor);
                        else
                            return pathCapacity[target];
                    }
                }
            }

            return 0;
        }

        internal int GetMaxFlow(Vertex destination)
        {
            int flow = 0;
            List<Edge> capacities = this.Grafo.ListEdges.FindAll(x => x.To.Nome == destination.Nome);

            foreach (var item in capacities)
                flow += item.Load;

            return flow;
        }
    }

}