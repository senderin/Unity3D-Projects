using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using graph;
using System;

public class GraphController : MonoBehaviour
{
    public float width;
    public float height;
    public float depth;
    public int iteration;

    private Graph graph;
    private float area;
    private float k; // optimal distance between vertices
    private float temperature; // limits the total displacement

    public void Start()
    {
        area = GetArea();
        CreateGraph();
        k = Mathf.Sqrt(area / graph.vertices.Count);
        UpdateGraph();
    }

    private void CreateGraph() {
        graph = new Graph();
        Vertex a = graph.AddVertex("a", GetRandomPosition());
        Vertex b = graph.AddVertex("b", GetRandomPosition());
        Vertex c = graph.AddVertex("c", GetRandomPosition());
        Vertex d = graph.AddVertex("d", GetRandomPosition());
        Vertex e = graph.AddVertex("e", GetRandomPosition());
        Vertex f = graph.AddVertex("f", GetRandomPosition());
        Vertex g = graph.AddVertex("g", GetRandomPosition());
        Vertex h = graph.AddVertex("h", GetRandomPosition());
        Vertex i = graph.AddVertex("i", GetRandomPosition());
        Vertex j = graph.AddVertex("j", GetRandomPosition());
        Vertex k = graph.AddVertex("k", GetRandomPosition());
        Vertex l = graph.AddVertex("l", GetRandomPosition());
        graph.AddEdge(a, b, "a_b");
        graph.AddEdge(a, c, "a_c");
        graph.AddEdge(b, c, "b_c");
        graph.AddEdge(b, d, "b_d");
        graph.AddEdge(d, c, "d_c");
        graph.AddEdge(d, e, "d_e");
        graph.AddEdge(d, f, "d_f");
        graph.AddEdge(k, b, "k_b");
        graph.AddEdge(j, c, "j_c");
        graph.AddEdge(h, c, "h_c");
        graph.AddEdge(g, d, "g_d");
        graph.AddEdge(l, c, "l_c");
        graph.AddEdge(f, e, "f_e");
        graph.AddEdge(i, f, "i_f");
        graph.AddEdge(a, e, "a_e");
        graph.AddEdge(a, f, "a_f");
        graph.AddEdge(b, g, "b_g");
        graph.AddEdge(b, h, "b_h");
        graph.AddEdge(d, i, "d_i");
        graph.AddEdge(d, j, "d_j");
        graph.AddEdge(d, k, "d_k");
        graph.AddEdge(k, l, "k_l");
        graph.AddEdge(j, d, "j_d");
        graph.AddEdge(h, e, "h_e");
        graph.AddEdge(g, f, "g_f");
    }

    private float GetArea()
    {
        return width * height;
    }

    private float CalculateRepulsiveForce(float distance)
    {
        return k * k / distance;
    }

    private float CalculateAttractiveForce(float distance)
    {
        return distance * distance / k;
    }

    internal void UpdateGraph() {
        temperature = width / 10;

        for(int i = 0; i < iteration; i++) {
            // calculate repulsive forces
            foreach (Vertex v in graph.vertices)
            {
                v.displacement = Vector3.zero;
                foreach (Vertex u in graph.vertices)
                {
                    if(u != v) {
                        Vector3 delta = v.transform.position - u.transform.position;
                        v.displacement = v.displacement + (delta / delta.magnitude) * CalculateRepulsiveForce(delta.magnitude);
                    }
                }
            }

            // calculate attractive forces
            foreach(Edge e in graph.edges) {
                Vector3 delta = e.startVertex.transform.position - e.endVertex.transform.position;
                e.startVertex.displacement = e.startVertex.displacement - (delta / delta.magnitude) * CalculateAttractiveForce(delta.magnitude);
                e.endVertex.displacement = e.endVertex.displacement + (delta / delta.magnitude) * CalculateAttractiveForce(delta.magnitude);
            }

            foreach(Vertex v in graph.vertices) {
                // limit the maximum displacement to the temperature t
                v.transform.position = v.transform.position + (v.displacement / v.displacement.magnitude) * Mathf.Min(v.displacement.magnitude, temperature);
                // prevent from being displaced outside frame
                float xPos = Mathf.Min(width / 2, Mathf.Max(-width / 2, v.transform.position.x));
                float yPos = Mathf.Min(height / 2, Mathf.Max(-height / 2, v.transform.position.y));
                float zPos = Mathf.Min(depth / 2, Mathf.Max(-depth / 2, v.transform.position.z));
                v.transform.position = new Vector3(xPos, yPos, zPos);
            }
            // reduce the temperature for better layout configuration
            temperature = Cool(temperature);
            graph.UpdateEdges();
        }
    }

    private float Cool(float temp)
    {
        temp = temp * 0.95f;
        return temp;
    }

    private Vector3 GetRandomPosition() {
        Vector3 pos = new Vector3(Random.Range(-width / 2, width / 2),
            Random.Range(-height / 2, height / 2), Random.Range(-depth / 2, depth / 2));
        return pos;
    }

    internal void UpdateVertexPosition(GameObject nodeObject, Vector3 curPosition)
    {
        nodeObject.transform.position = curPosition;
        graph.UpdateEdges();
    }

}
