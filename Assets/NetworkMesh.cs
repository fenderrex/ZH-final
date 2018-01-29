using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class NetworkMesh : System.Object
{

    Mesh i = new Mesh();
    int[] _triangles;
    Vector3[] _vertices;
    Vector2[] _uv;
    int _vertexCount;
    string _name;
    // Use this for initialization
    void Start () {
		
	}

    public int vertexCount
    {
        get
        {
            return vertexCount;

        }
        set
        {
             vertexCount=value;
        }

    }
    public int[] triangles
    {
        get
        {
            return _triangles;
        }
        set
        {
            _triangles = value;
        }
    }
    public Vector3[] vertices
    {
        get
        {
            return _vertices;
        }
        set
        {
            _vertices = value;
            _vertexCount = vertices.Length;
        }
    }
    public Vector2[] uv
    {
        get
        {
            return _uv;
        }
        set
        {
            _uv = value;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
