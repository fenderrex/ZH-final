using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cmesh : MonoBehaviour {
    public Vector3[] newVertices = new Vector3[10];
    public Vector2[] newUV = new Vector2[4];
    public Vector3[] newNormals = new Vector3[4];
    public int[] newTriangles = new int[6];
    public GameObject wave;
    // Use this for initialization
    Mesh tube(VectorReffrence a1, VectorReffrence b1, VectorReffrence c1, VectorReffrence d1, VectorReffrence a2, VectorReffrence b2, VectorReffrence c2, VectorReffrence d2)
    {
        //newVertices 2nd vertex pair is 1,3,5,7

        newVertices[0] = a1.refPoint;//a1 1   -------  2   a2
        newVertices[1] = a2.refPoint;//        |    / |
        newVertices[2] = b1.refPoint;//        |  /   |
        newVertices[3] = b2.refPoint;//        |/     |
                                     //b1 0    -------     b2 3



        //newVertices[4] = b1;       //b1     2     -------     b2   3
        //newVertices[5] = b2;       //             |    / |
        newVertices[4] = c1.refPoint;//             |  /   |
        newVertices[5] = c2.refPoint;//             |/     |
                                     //c1     4      -------     c2   5





       // newVertices[0] = c1;        // c1   4   -------     c2    5
      //  newVertices[1] = c2;         //         |    / |
        newVertices[6] = d1.refPoint;//           |  /   |
        newVertices[7] = d2.refPoint;//           |/     |
                                     // d1     6   -------    d2    7

       


    //  newVertices[0] = d1;         //d1   6  -------  7   d2
    //  newVertices[1] = d2;         //        |    / |
        newVertices[2] = a1.refPoint;//        |  /   |
        newVertices[3] = a2.refPoint;//        |/     |
                                     //a1   0   -------  1   a2







        MeshFilter meshf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshf.mesh = mesh;
        mesh.vertices = newVertices;

        mesh.triangles = newTriangles;

        mesh.normals = newNormals;
        mesh.uv = newUV;
        GetComponent<MeshFilter>().mesh = mesh;
        return mesh;


    }
    Mesh analogTube(Mesh pre,int[] edges, VectorReffrence[] append)
    {
        //      edge 
        //b  1-------   2 c
        //    | t1 / |
        //edge|  /   |  edge
        //    |/ t2  |
        //a 0 -------  3  d 
        //      edge
        //      edge 
        //b  0-------   3 c
        //    | t3 / |
        //edge|  /   |  edge
        //    |/ t4  |
        //a 4 -------  5  d 
        //      edge

        //int[] tri = pre.triangles;
        int size=pre.vertices.Length + append.Length;
        VectorReffrence[] ves = new VectorReffrence[size];
        int ii = 0;
        int[] tri = new int[pre.triangles.Length+((edges.Length/2)*6)];
        foreach (int vet in pre.triangles)
        {

            tri[ii] = vet;
            ii++;

        }
        ii = 0;
        foreach (Vector3 vet in pre.vertices)
        {

            ves[ii] = new VectorReffrence(vet);//, tri[ii]);//no we need the index that has value of the place in vertices// can appear more then onces...
            ii++;

        }
        //ii = 0; no this next part adds the append
        foreach (VectorReffrence vet in append)
        {

            ves[ii] = vet;
            ii++;

        }
        ii = 0;
        /*
        foreach (VectorReffrence meh in ves)
        {

            if (meh.byRef)
            {
                //  ves[ii] = 
                //DOC:
                // we leave meh.refPoint empty so we know to REUSE! a vertex at meh.refPoss eg. ves[tri[meh.refPoss]]
                // 
                // 
                // meh.refPoint== ves[tri[meh.refPoss]].refPoint;//this finds a Vector3 by a vertex postion in a mesh depicted by triangle//

            }
            ii++;
        }
        */
        pre=pain(pre, ves[0], ves[1], ves[2], ves[3]);//

        return pre;
    }

    Mesh pain(Mesh mesh, Vector3 a, Vector3 b, Vector3 c, Vector3 d)//ONLY FOR SINGLE PAIN! NOT FOR MESH USE WILL BUILD UN-OPPTOMISED MESH
    {
        return pain(mesh, new VectorReffrence(a), new VectorReffrence(b), new VectorReffrence(c), new VectorReffrence(d));// for quick start qauds/pains

    }
    void vectorError(string vector)
    {

        print("error " + vector + " is a single refrence vector not a edge");
    }
    Mesh pain(Mesh mesh,VectorReffrence a, VectorReffrence b, VectorReffrence c, VectorReffrence d)
    {

        int[] t1 = new int[3];
        int[] t2 = new int[3];
        int[] q1 = new int[6];
        VectorReffrence[,] edges = new VectorReffrence[3,2];
        
        //b or d must be a reffrence
        if (a.byRef && b.byRef)
        {
            t1[0] = mesh.triangles[a.refPoss];
            t1[2] = mesh.triangles[b.refPoss];
            edges[0, 0] = a;
            edges[0, 1] = d;
            edges[1, 0] = d;
            edges[1, 1] = c;
            edges[2, 0] = c;
            edges[2, 1] = b;

        }
        else if (a.byRef && d.byRef)
        {
            t2[0] = mesh.triangles[a.refPoss];
            t2[1] = mesh.triangles[d.refPoss];
            edges[0, 0] = d;
            edges[0, 1] = c;
            edges[1, 0] = c;
            edges[1, 1] = b;
            edges[2, 0] = b;
            edges[2, 1] = a;
            

        }else if (a.byRef==false)//we do not look up this vertex in the mesh with refpos
        {
            newVertices[0] = a.refPoint;
            t1[0] = 0;
        }
        else
        {
            vectorError("A"); //error a is a single refrence vector not a edge //TODO: add debug
        }
        //     edge 
        //b  1-------   2 c
        //    | t1 / |
        //edge|  /   |  edge
        //    |/ t2  |
        //a 0 -------  3  d 
        //     edge
        if (c.byRef && b.byRef)//a && b accounted for
        {

            t1[1] = mesh.triangles[c.refPoss];
            t1[2] = mesh.triangles[b.refPoss];

            edges[0, 0] = b;
            edges[0, 1] = a;
            edges[1, 0] = a;
            edges[1, 1] = d;
            edges[2, 0] = d;
            edges[2, 1] = c;

        }
        else if (b.byRef==false)//we do not look up this vertex in the mesh with refpos
        {
            newVertices[1] = b.refPoint;
            t1[2] = 1;
            t2[0] = 0;
        }
        else if((b.byRef == true)&& (a.byRef == false))
        {
            vectorError("B"); //error b is a single refrence vector not a edge //TODO: add debug
        }
        if (c.byRef && d.byRef)
        {
            //     edge 
            //b  1-------   2 c
            //    | t1 / |
            //edge|  /   |  edge
            //    |/ t2  |
            //a 0 -------  3  d 
            //     edge
            // meh.refPoint== ves[tri[meh.refPoss]].refPoint;//this finds a Vector3 by a vertex postion in a mesh depicted by triangle//
            //  meh.refPoint== ves[tri[c.byRef]].refPoint;//this finds a Vector3 by a vertex postion in a mesh depicted by triangle//
            // public Vector3[] newVertices = new Vector3[10];
            // public Vector2[] newUV = new Vector2[4];
            // public Vector3[] newNormals = new Vector3[4];
            // public int[] newTriangles = new int[6];

            t2[1] = mesh.triangles[d.refPoss];
            t2[2] = mesh.triangles[c.refPoss];
            //.refPoint;
            edges[0, 0] = c;
            edges[0, 1] = b;
            edges[1, 0] = b;
            edges[1, 1] = a;
            edges[2, 0] = a;
            edges[2, 1] = d;

        } //c && b accounted for
        else if(c.byRef==false)//t2 0,1,2
        {
            newVertices[2] = c.refPoint;
            t1[1] = 1;
            t2[2] = 2;

        }
        else if ((c.byRef == true) && (b.byRef == false))
        {
            vectorError("C");  //error c is a single refrence vector not a edge //TODO: add debug
        }

        if (d.byRef==false)
        {
            newVertices[3] = d.refPoint;
            t2[2] = 3;
        }
        else if ((d.byRef == true) && (a.byRef == false) && (c.byRef == false))
        {
            vectorError("D");//error d is a single refrence vector not a edge //TODO: add debug
        }


        //t1  012;   t2  023
        //     edge 
        //b  1-------   2 c
        //    | t1 / |
        //edge|  /   |  edge
        //    |/ t2  |
        //a 0 -------  3  d 
        //     edge
        //  newTriangles[0] = 0;
        //  newTriangles[1] = 1;
        //  newTriangles[2] = 2;

        //        newTriangles[3] = 0;
        //      newTriangles[4] = 2;
        //    newTriangles[5] = 3;

        newNormals[0] = -Vector3.forward;
        newNormals[1] = -Vector3.forward;
        newNormals[2] = -Vector3.forward;
        newNormals[3] = -Vector3.forward;


        newUV[0] = new Vector2(0, 0);
        newUV[1] = new Vector2(1, 0);
        newUV[2] = new Vector2(0, 1);
        newUV[3] = new Vector2(1, 1);



        return BuildMesh(mesh);

    }

    GameObject[] vert_h;// = new GameObject[8];

    void Start () {
        //  waveTool flow=wave.AddComponent<waveTool>();
        //waveTool.Arch[] rails= flow.RefreshNodes();
        //triTube();
        Vector3[] vert = { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0) };//, new Vector3(0, 0, 1), new Vector3(0, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 0, 1) };
        vert_h = new GameObject[vert.Length];
        int ii = 0;
        foreach (GameObject h in vert_h)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.parent = transform;
            sphere.transform.localPosition = vert[ii];
            sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
            sphere.name = ii.ToString();
            vert_h[ii] = sphere;
            ii++;
        }
        MeshFilter meshf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshf.mesh = mesh;
        meshf.mesh=pain(mesh,vert_h[0].transform.localPosition, vert_h[1].transform.localPosition, vert_h[2].transform.localPosition, vert_h[3].transform.localPosition);
               // tube(vert_h[0].transform.position, vert_h[1].transform.position, vert_h[2].transform.position, vert_h[3].transform.position,
            //   vert_h[4].transform.position, vert_h[5].transform.position, vert_h[6].transform.position, vert_h[7].transform.position);
    }
	
	// Update is called once per frame
	void Update () {
        MeshFilter meshf = GetComponent<MeshFilter>();

        meshf.mesh=pain(meshf.mesh, vert_h[0].transform.localPosition, vert_h[1].transform.localPosition, vert_h[2].transform.localPosition, vert_h[3].transform.localPosition);

     //   tube(vert_h[0].transform.position, vert_h[1].transform.position, vert_h[2].transform.position, vert_h[3].transform.position,
     //          vert_h[4].transform.position, vert_h[5].transform.position, vert_h[6].transform.position, vert_h[7].transform.position);
    }
    void triTube()
    {
        newTriangles[0] = 0;
        newTriangles[1] = 1;
        newTriangles[2] = 2;

        newTriangles[3] = 1;
        newTriangles[4] = 2;
        newTriangles[5] = 3;

        newTriangles[6] = 2;
        newTriangles[7] = 3;
        newTriangles[8] = 4;

        newTriangles[9] = 3;
        newTriangles[10] = 4;
        newTriangles[11] = 5;

        newTriangles[12] = 4;
        newTriangles[13] = 6;
        newTriangles[14] = 5;

        newTriangles[15] = 5;
        newTriangles[16] = 6;
        newTriangles[17] = 7;

        newTriangles[18] = 6;
        newTriangles[19] = 7;
        newTriangles[20] = 0;

        newTriangles[21] = 0;
        newTriangles[22] = 7;
        newTriangles[23] = 1;
    }
    Mesh BuildMesh(Mesh mesh)
    {
       // MeshFilter meshf = GetComponent<MeshFilter>();
       // Mesh mesh = new Mesh();
       // meshf.mesh = mesh;
        mesh.vertices = newVertices;

        mesh.triangles = newTriangles;

        mesh.normals = newNormals;
        mesh.uv = newUV;
        GetComponent<MeshFilter>().mesh = mesh;
        return mesh;

    }
    class VectorReffrence //VectorReffrence is used to hold a vector or the location of a vector in a mesh. TODO: should it also hold a mesh?
    {
        Vector3 _refPoint;
        int _refPoss;
        bool _byRef = false;
        GameObject _refObject = null;


        // sphere.transform.parent = transform;
        //sphere.transform.localPosition = vert[ii];
        //    sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
        public VectorReffrence(Vector3 refPoint, int refPoss)
        {
            this._refPoint = refPoint;
            this._refPoss = refPoss;
        }
        public VectorReffrence(int refPoss)
        {
            this._refPoss = refPoss;
            byRef = true;
        }
        public VectorReffrence(Vector3 refPoint)
        {
            this._refPoint = refPoint;
            _refPoss = -1;
        }
        public GameObject refObject
        {
            get
            {
                _refPoint = _refObject.transform.localPosition;
                return _refObject;
            }
            set
            {
                _refObject = value;
                _refPoint = value.transform.localPosition;
            }
        }
        public int refPoss//possition in mesh
        {
            get { return _refPoss; }
            set
            {
                if (value == -1)
                {
                    //TODO: throw error
                    byRef = false;
                    _refPoss = value;
                }
                else
                {
                    _refPoss = value;
                }
            }
        }
        public Vector3 refPoint//point in 3d space
        {
            get
            {
                _refPoint = _refObject.transform.localPosition;
                return _refPoint;
            }
            set
            {
                _refObject.transform.localPosition = value;//update the gameobjects pos
                _refPoint = value;
            }
        }
        public bool byRef
        {
            get { return _byRef; }
            set { _byRef = value; }
        }


    }
}
