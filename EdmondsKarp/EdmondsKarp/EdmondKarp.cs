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

        public void FindMaxFlow(Vertex source, Vertex target, out Graph legalFlows)
        {

            int flow = 0;

            Graph _legalFlows = new Graph();
            legalFlows = _legalFlows;

            foreach (Vertex v in this.Grafo.ListVertex)
                _legalFlows.AddVertex(v);
            foreach (Edge dg in this.Grafo.ListEdges)
                _legalFlows.AddEdge(dg);


            while(true)
            {
                IDictionary<Vertex, Vertex> path;
                int capacity = BreadthFirstSearch(Grafo, source, target, legalFlows, out path);

                if (capacity == 0) 
                    break;
                flow += capacity;
                
                Vertex v = target;

                while (!v.Equals(source))
                {
                    Vertex u = path[v];
                    _legalFlows.SetEdge(u, v, _legalFlows.GetCapacity(u, v) + capacity);
                    _legalFlows.SetEdge(v, u, _legalFlows.GetCapacity(v, u) - capacity);
                    v = u;
                }
            }

            
        }

        private int BreadthFirstSearch(Graph graph, Vertex source, Vertex target, Graph legalFlows, out IDictionary<Vertex, Vertex> path)
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

                IEnumerable<Vertex> Neighbors = graph.Neighbors(dequeuedItem);
                foreach (Vertex Neighbor in Neighbors)
                {
                    int capacity = graph.GetCapacity(dequeuedItem, Neighbor);
                    int lFlows = legalFlows.GetLoad(dequeuedItem, Neighbor);

                    if (capacity -  lFlows > 0 && path.ContainsKey(Neighbor) == false)
                    {
                        path[Neighbor] = dequeuedItem;
                        pathCapacity[Neighbor] = Math.Min(pathCapacity[dequeuedItem], graph.GetCapacity(dequeuedItem, Neighbor) - legalFlows.GetLoad(dequeuedItem ,Neighbor));
                        
                        if (!Neighbor.Equals(target)) 
                            queue.Enqueue(Neighbor);
                        else
                            return pathCapacity[target];
                    }
                }
            }

            return 0;

        }

    }

}