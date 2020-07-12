using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Vertex {
    public Vector3 position;
    public Vector3 disp;
}

public class Edge {
    public Vertex startVertex;
    public Vertex destinationVertex;
}

public class Graph {

    public Graph() {
        vertices = new List<Vertex>();
        edges = new List<Edge>();
    }
    public List<Vertex> vertices;
    public List<Edge> edges;

}

public class GraphController : MonoBehaviour
{
    public float width;
    public float height;
    public float depth;
    public int iteration;

    public Graph graph;

    private float area;
    private float k; // optimal distance between vertices
    private float temperature;

    public void Start()
    {
        CreateGraph();
        ForcedDirectedGraph();
        DrawGraph();
    }


    public void Update() {

    }

    private float GetArea() {
        return width * height;    
    }

    private float CalculateRepulsiveForce(float distance) {
        return -k * k / distance;
    }

    private float CalculateAttractiveForce(float distance)
    {
        return distance * distance / k;
    }

    private void CreateGraph() {
        graph = new Graph();

        Vertex a = new Vertex();
        Vertex b = new Vertex();
        Vertex c = new Vertex();
        Vertex d = new Vertex();
        graph.vertices.Add(a);
        graph.vertices.Add(b);
        graph.vertices.Add(c);
        graph.vertices.Add(d);

        Edge a_b = new Edge()
        {
            startVertex = a,
            destinationVertex = b
        };

        Edge b_c = new Edge()
        {
            startVertex = b,
            destinationVertex = c
        };

        Edge a_d = new Edge()
        {
            startVertex = a,
            destinationVertex = d
        };

        Edge d_c = new Edge()
        {
            startVertex = d,
            destinationVertex = c
        };

        graph.edges.Add(a_b);
        graph.edges.Add(b_c);
        graph.edges.Add(a_d);
        graph.edges.Add(d_c);
    }

    private void ForcedDirectedGraph() {
        area = GetArea();
        temperature = width / 10;
        SetVerticesRandomInitialPositions();
        k = Mathf.Sqrt(area / graph.vertices.Count);

        for(int i = 0; i < iteration; i++) {
            // calculate repulsive forces
            foreach (Vertex v in graph.vertices)
            {
                v.disp = Vector3.zero;
                foreach (Vertex u in graph.vertices)
                {
                    if(u != v) {
                        Vector3 delta = v.position - u.position;
                        v.disp = v.disp + (delta / delta.magnitude) * CalculateRepulsiveForce(delta.magnitude);
                    }
                }
            }

            // calculate attractive forces
            foreach(Edge e in graph.edges) {
                Vector3 delta = e.startVertex.position - e.destinationVertex.position;
                e.startVertex.disp = e.startVertex.disp - (delta / delta.magnitude) * CalculateAttractiveForce(delta.magnitude);
                e.destinationVertex.disp = e.destinationVertex.disp - (delta / delta.magnitude) * CalculateAttractiveForce(delta.magnitude);
            }

            foreach(Vertex v in graph.vertices) {
                v.position = v.position + (v.disp / v.disp.magnitude) * Mathf.Min(v.disp.magnitude, temperature);
                v.position.x = Mathf.Min(width / 2, Mathf.Max(-width / 2, v.position.x));
                v.position.y = Mathf.Min(height / 2, Mathf.Max(-height / 2, v.position.y));
            }
            temperature = Cool(temperature);
        }
    }

    private float Cool(float temp)
    {
        return temp;
    }

    private void SetVerticesRandomInitialPositions()
    {
        foreach (Vertex v in graph.vertices)
        {
            v.position = GetRandomPosition();
        }
    }

    private Vector3 GetRandomPosition() {
        Vector3 pos = new Vector3(Random.Range(-width / 2, width / 2),
            Random.Range(-height / 2, height / 2), Random.Range(-depth / 2, depth / 2));
        return pos;
    }

    private void DrawGraph()
    {
        foreach(Vertex vertex in graph.vertices) {
            GameObject node = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            node.transform.position = vertex.position;
        }

        foreach (Edge edge in graph.edges)
        {
            GameObject line = new GameObject();
            line.AddComponent<LineRenderer>();
            line.GetComponent<LineRenderer>().startWidth = 0.1f;
            line.GetComponent<LineRenderer>().endWidth = 0.1f;
            line.GetComponent<LineRenderer>().startColor = Color.black;
            line.GetComponent<LineRenderer>().endColor = Color.blue;
            line.GetComponent<LineRenderer>().SetPosition(0, edge.startVertex.position);
            line.GetComponent<LineRenderer>().SetPosition(1, edge.destinationVertex.position);
        }
    }
}
