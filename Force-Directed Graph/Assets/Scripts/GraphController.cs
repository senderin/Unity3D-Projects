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
    public string fileName;

    public bool disableForceWhenDragging = false;

    private Graph graph;
    private float area;
    private float k; // optimal distance between vertices
    private float temperature; // limits the total displacement
    private float coolingRate = 0.9f; // from book: graph algorithms and applications 4, liotta ..., page = 263

    public void Start()
    {
        area = GetArea();
        graph = CreateGraph();
        k = Mathf.Sqrt(area / graph.vertices.Count);
        temperature = width / 10;
        // temperature = k;
    }

    private float GetArea()
    {
        return width * height;
    }

    private float CalculateRepulsiveForce(float distance)
    {
        return (k * k / distance);
    }

    private float CalculateAttractiveForce(float distance)
    {
        return distance * distance / k;
    }

    int count = 0;
    private void Update()
    {
        if (count > iteration) 
            return;

        UpdateGraph();
        count++;
    }

    internal void UpdateGraph() {
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
        temperature = Cool(temperature);
        graph.UpdateEdges();
    }

    private float Cool(float temp)
    {
        temp = temp * coolingRate;
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

        if (!disableForceWhenDragging) {
            // elasticity when draging
            if (temperature < 0.0001f)
                temperature = width / 10;
            UpdateGraph();
        }
        else
            graph.UpdateEdges();
    }

    private Graph CreateGraph()
    {
        bool isLoaded = FileOperations.LoadFile(fileName, true);
        Graph gr = new Graph();
        GameObject graphObject = new GameObject();
        graphObject.name = "ProteinInteractionNetwork";
        int lineCount = 0;
        if (isLoaded)
        {
            List<string> lines = FileOperations.GetLines();
            foreach (String line in lines)
            {
                if (lineCount > 500)
                   break;
                string[] tokens = line.Split(' ');
                Vertex vertex1 = CreateVertex(tokens[0], gr);
                Vertex vertex2 = CreateVertex(tokens[1], gr);
                Edge edge = gr.AddEdge(vertex1, vertex2, vertex1.name + "_" + vertex2.name);
                vertex1.transform.SetParent(graphObject.transform);
                vertex2.transform.SetParent(graphObject.transform);
                edge.transform.SetParent(graphObject.transform);
                lineCount++;
                Debug.Log("Loaded %" + (lineCount * (100 / lines.Count)).ToString());
            }
        }
        return gr;
    }

    private Vertex CreateVertex(string name, Graph gr)
    {
        Vertex vertex;
        if (!gr.HasVertex(name))
            vertex = gr.AddVertex(name, GetRandomPosition());
        else
            vertex = gr.GetVertex(name);
        return vertex;
    }


}
