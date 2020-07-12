using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace graph {

    public class Graph
    {
        public List<Vertex> vertices;
        public List<Edge> edges;

        public Graph()
        {
            vertices = new List<Vertex>();
            edges = new List<Edge>();
        }

        public Vertex AddVertex(string name, Vector3 position)
        {
            GameObject nodeObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            nodeObject.name = name;
            nodeObject.transform.position = position;
            Vertex vertex = nodeObject.AddComponent<Vertex>();
            vertices.Add(vertex);
            return vertex;
        }

        public Vertex GetVertex(string name)
        {
            return vertices.Find(x => x.name == name);
        }

        public Vertex RemoveVertex(string name)
        {
            Vertex vertex = GetVertex(name);
            vertices.Remove(vertex);
            return vertex;
        }

        public void RemoveVertex(Vertex vertex)
        {
            vertices.Remove(vertex);
        }

        public Edge AddEdge(Vertex start, Vertex end, string name)
        {
            GameObject lineObject = new GameObject
            {
                name = name
            };
            Edge edge = lineObject.AddComponent<Edge>();
            edge.startVertex = start;
            edge.endVertex = end;
            edges.Add(edge);

            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
            lineRenderer.SetPosition(0, start.transform.position);
            lineRenderer.SetPosition(1, end.transform.position);


            return edge;
        }

        public Edge GetEdge(string name)
        {
            return edges.Find(x => x.name == name);
        }

        public Edge RemoveEdge(string name)
        {
            Edge edge = GetEdge(name);
            edges.Remove(edge);
            return edge;
        }

        public void RemoveEdge(Edge edge)
        {
            edges.Remove(edge);
        }

        internal void UpdateEdges()
        {
            foreach (Edge e in edges)
                e.UpdateLine();
        }
    }
}
