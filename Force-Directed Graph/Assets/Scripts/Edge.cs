using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace graph {
    public class Edge : MonoBehaviour
    {
        public Vertex startVertex;
        public Vertex endVertex;

        public Edge(Vertex start, Vertex end)
        {
            this.startVertex = start;
            this.endVertex = end;
        }

        internal void UpdateLine()
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, startVertex.transform.position);
            lineRenderer.SetPosition(1, endVertex.transform.position);
        }
    }
}


