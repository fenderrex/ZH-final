using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagedNetworkMesh : MonoBehaviour
{

    NetworkMesh mesh;
    // Use this for initialization
    ManagedNetworkMesh() {
        mesh = new NetworkMesh();

    }
    ManagedNetworkMesh(NetworkMesh mesh)
    {
        this.mesh = mesh;

    }
    public int vertexCount
    {
        get
        {
            return vertexCount;
        }

    }
    public int[] triangles
    {
        get
        {
            return mesh.triangles;
        }
        set
        {
            mesh.triangles = value;
        }
    }
    public Vector3[] vertices
    {
        get
        {
            return mesh.vertices;
        }
        set
        {
            mesh.vertices = value;
            mesh.vertexCount = vertices.Length;
        }
    }
    public Vector2[] uv
    {
        get
        {
            return mesh.uv;
        }
        set
        {
            mesh.uv =value;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
