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
        newVertices[2] = a1.refPoint;    //        |  /   |
        newVertices[3] = a2.refPoint;    //        |/     |
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
    /// <summary>
    /// solves where the vertices attach to the mesh by proximity to edges
    /// </summary>
    /// <param name="pre">The mesh to build on</param>//make a MeshRefrence 
    /// <param name="edges">The edges</param>
    /// <param name="append">VectorReffrence to add</param>
    /// <returns></returns>
    Mesh analogMesh(Mesh pre, int[] edges, VectorReffrence[] append)
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
        int size = pre.vertices.Length + append.Length;
        VectorReffrence[] ves = new VectorReffrence[size];
        int ii = 0;
        int[] tri = new int[pre.triangles.Length + ((edges.Length / 2) * 6)];
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
        QuadReffrence preQuad = pain(pre, ves[0], ves[1], ves[2], ves[3]);//

        return pre;
    }
    /// <summary>
    /// ONLY FOR SINGLE PAIN! NOT FOR MESH USE WILL BUILD UN-OPPTOMISED MESH good for starting meshes
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    QuadReffrence pain(Mesh mesh, Vector3 a, Vector3 b, Vector3 c, Vector3 d)//ONLY FOR SINGLE PAIN! NOT FOR MESH USE WILL BUILD UN-OPPTOMISED MESH
    {
        return pain(mesh, new VectorReffrence(a), new VectorReffrence(b), new VectorReffrence(c), new VectorReffrence(d));// for quick start qauds/pains

    }
    void vectorError(string vector)
    {
    //    print("error " + vector + " is a single refrence vector not a edge");
    }

    QuadReffrence pain(Mesh mesh, VectorReffrence a, VectorReffrence b, VectorReffrence c, VectorReffrence d)
    {



        /*
        //TODO: 
        //  Normals
        //  UVs
        //
        //  
        //
        //  make a object that can hold a mesh and VectorReffrence's of that mesh.
        //   this object should have a function that can be called by VectorReffrence interface.
        //    aswell as all edges.
        //    
        */
        int[] t1 = new int[3];
        int[] t2 = new int[3];
        int[] q1 = new int[6];
        int[] tri = new int[mesh.triangles.Length + 6];
        //int newVerts = 0;
        //Vector3[] newQuad = new Vector3[4];
        //b---c
        //|   |
        //a . d
        //b---c
        //.   |
        //a---d
        //     edge 
        //b  1-------   2 c b 1-------   2 c
        //    | t1 / |         | t1 / |
        //edge|  /   |         |  /   | 
        //    |/ t2  |         |/ t2  |
        //a 0 -------  3   d 0 -------  3  d 

        //b  1-------   2 c
        //    | t1 / |
        //edge|  /   |  edge
        //    |/ t2  |
        //a 0 -------  3  d 
        //     edge
        //

        // for debugging we will not use the pos calulation 
        // but char in hopes to make it easyer

        char[] edges = {'a', 'b',//1
                        'b', 'c',//3
                        'c', 'd',//5
                        'd', 'a'};//7    
        //b or d must be a reffrence
        //we do this edge detection so we know what not to repeat
        a.ID = "a";
        b.ID = "b";
        c.ID = "c";
        d.ID = "d";
        if (a.byRef && b.byRef)
        {
            //  newVerts+=2;
            edges[0] = '0';
            edges[1] = '0';
            //because every other vertex in a quad shares one vertex with two triangles we will allways set 3 vertices in triagles
            t1[0] = mesh.triangles[a.refPoss];
            t1[2] = mesh.triangles[b.refPoss];
            t2[0] = mesh.triangles[a.refPoss];
        }

        if (c.byRef && b.byRef)//a && b accounted for
        {
            //   newVerts += 2;
            edges[2] = '0';
            edges[3] = '0';
            t1[1] = mesh.triangles[c.refPoss];
            t1[2] = mesh.triangles[b.refPoss];
            t2[2] = mesh.triangles[c.refPoss];
        }
        if (a.byRef && d.byRef)
        {
            //   newVerts += 2;
            edges[6] = '0';
            edges[7] = '0';
            t2[0] = mesh.triangles[a.refPoss];
            t2[1] = mesh.triangles[d.refPoss];
            t1[0] = mesh.triangles[a.refPoss];
        }
        if (c.byRef && d.byRef)
        {
            // newVerts += 2;
            edges[4] = '0';
            edges[5] = '0';
            t2[1] = mesh.triangles[d.refPoss];
            t2[2] = mesh.triangles[c.refPoss];
            t1[1] = mesh.triangles[c.refPoss];
        }


        //c && b accounted for

        //     edge 
        //b  1-------   2 c
        //    | t1 / |
        //edge|  /   |  edge
        //    |/ t2  |
        //a 0 -------  3  d 
        //     edge
        List<Vector3> newQuad = new List<Vector3>();
        int pos = mesh.vertices.Length;
      //  print("ver l: " + pos.ToString());
        if (a.byRef == false)//we do not look up this vertex in the mesh with refpos
        {

            newQuad.Add(a.refPoint);
            a.refSpace = pos + newQuad.Count;
            t1[0] = pos + newQuad.Count;//because we are adding a new vertec we ensure we predict its location in the mesh and store thats position in the triangles
            t2[0] = pos + newQuad.Count;
        }
        if (b.byRef == false)//we do not look up this vertex in the mesh with refpos
        {

            newQuad.Add(b.refPoint); //newQuad[1] = b.refPoint;
            b.refSpace = pos + newQuad.Count;
            t1[2] = pos + newQuad.Count;
        }
        if (c.byRef == false)
        {

            newQuad.Add(c.refPoint);
            c.refSpace = pos + newQuad.Count;
            t1[1] = pos + newQuad.Count;
            t2[2] = pos + newQuad.Count;
        }
        if (d.byRef == false)
        {

            newQuad.Add(d.refPoint);
            d.refSpace = pos + newQuad.Count;
            t2[1] = pos + newQuad.Count;
        }

        if ((a.byRef == true) && (b.byRef == false) && (d.byRef == false))
        {
            vectorError("A"); //error a is a single refrence vector not a edge //TODO: add debug
        }
        if ((b.byRef == true) && (a.byRef == false) && (d.byRef == false))
        {
            vectorError("B"); //error b is a single refrence vector not a edge //TODO: add debug
        }
        if ((c.byRef == true) && (b.byRef == false) && (d.byRef == false))
        {
            vectorError("C");  //error c is a single refrence vector not a edge //TODO: add debug
        }
        if ((d.byRef == true) && (a.byRef == false) && (c.byRef == false))
        {
            vectorError("D");//error d is a single refrence vector not a edge //TODO: add debug
        }

        Vector3[] vect = new Vector3[mesh.vertices.Length + newQuad.Count];
        int placmentIndex = mesh.vertices.Length;

        for (int i = 0; i < mesh.vertices.Length; i++)//add mesh
        {
            vect[i] = mesh.vertices[i];

        }
        placmentIndex = mesh.vertices.Length;
        for (int i = 0; i < newQuad.Count; i++)//add quad 1-4
        {
            vect[i + (placmentIndex)] = newQuad[i];
        }




        placmentIndex = mesh.triangles.Length;
        for (int i = 0; i < placmentIndex; i++)//copy triangles for new mesh
        {
            tri[i] = mesh.triangles[i];
        }

        for (int i = 0; i < 3; i++)//add t1
        {
            tri[i + (placmentIndex)] = t1[i] - 1;
        }
        placmentIndex += 3;
        for (int i = 0; i < 3; i++)//add t2
        {
            tri[i + (placmentIndex)] = t2[i] - 1;
        }

        for (int i = 0; i < tri.Length; i++)//add quad 1-4
        {
           // print(tri[i]);

        }

        //Debug.Log("tri lenght: " + tri.Length.ToString() + "\nvect lenght: " + vect.Length.ToString());

        mesh.vertices = vect;
        mesh.triangles = tri;

        QuadReffrence Q = new QuadReffrence(mesh, new VectorReffrence[] { a, b, c, d }, edges);

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

        return Q;



        //return BuildMesh(mesh);

    }

    GameObject[] vert_h;// = new GameObject[8];

    void Start() {
        //  waveTool flow=wave.AddComponent<waveTool>();
        //waveTool.Arch[] rails= flow.RefreshNodes();
        //triTube();
        Vector3[] vert = { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0),
                           new Vector3(2, 1, 0), new Vector3(2, 0, 0),
                           new Vector3(3, 1, 0), new Vector3(3, 0, 0),
                           new Vector3(1, 2, 0), new Vector3(2, 2, 3),
                           new Vector3(-1, 0, 0), new Vector3(-1, 1, 0),
                           new Vector3(0, -1, 0), new Vector3(-1, -1, 0)};//, new Vector3(0, 0, 1), new Vector3(0, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 0, 1) };
        //buildMesh(vert);
        float[] t = { 10, 8, 5, 3, 1, 0 };
        int[] e=insertionSort(t);

    }
    public static int[] insertionSort(float[] arr)
    {
        int[] indices = new int[arr.Length];
        indices[0] = 0;
        for (int i = 1; i < arr.Length; i++)//this sets where to find the postion to start sorting
        {
            int j = i;
            for (; j >= 1 && arr[j] < arr[j - 1]; j--)//if the one before is gratter swap, loop untill it iten is in sorted place this moves one at a time
            {
                float temp = arr[j];
                arr[j] = arr[j - 1];
                arr[j - 1] = temp;
                indices[j] = indices[j - 1];
            }
            indices[j] = i;
        }
        return indices;//indices of sorted elements
    }
    /// <summary>
    /// this is the slower(+n) magical entrance function where all the crazy auto mesh deduction starts
    /// </summary>
    /// <param name="verts">this list of Vector3 is converted into VectorReffrence this can be timely so conciter the VectorReffrence overide</param>
    public void buildMesh(Vector3[] verts)
    {
   
        VectorReffrence[] ht = new VectorReffrence[verts.Length];
        // { a, b, c, d, c1, d1, c2, d2, c3, d3, c4, d4, c5, d5 };
        int i = 0;
        foreach (Vector3 h in verts)
        {
            ht[i] = new VectorReffrence(h);
            i++;
        }
        //   print("newVertices processed");

        buildMesh(ht);

    }
    /// <summary>
    /// this is the magical entrance function where all the crazy auto mesh deduction starts
    /// </summary>
    /// <param name="verts"></param>
    public void buildMesh(VectorReffrence[] verts)
    {
        int ii = 0;
        Transform hierarchyNode = transform.parent.Find("mesh points collection");
        GameObject meshPoints;
        if (hierarchyNode == null)
        {
            meshPoints = new GameObject();// GameObject.CreatePrimitive(PrimitiveType.Sphere);
            meshPoints.name = "mesh points collection";
            meshPoints.transform.parent = transform.parent;
        }
        else
        {
            meshPoints = hierarchyNode.gameObject;
        }
        foreach (VectorReffrence h in verts)
        {
            if (h.isRefObjectNull) {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.parent = meshPoints.transform;
                sphere.transform.localPosition = verts[ii].refPoint;
                sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
                sphere.name = ii.ToString();
            }
            
         //   ht[ii] = new VectorReffrence(vert_h[ii]);
            ii++;
        }
        //   print("newVertices processed");
        MeshFilter meshf = GetComponent<MeshFilter>();
        MeshReffrence MM = new MeshReffrence(this, meshf.mesh, verts);
        MM.buildQuadMesh(this, meshf.mesh, verts);

    }

    // Update is called once per frame
    void Update() {
        // MeshFilter meshf = GetComponent<MeshFilter>();
        //
        //
        //b---c---c1
        //|   |    |
        //a---d---d1
        //
        //a b c d
        //>edges
        //
        /*
        MeshFilter mesh_f = GetComponent<MeshFilter>();
       // Mesh mesh = new Mesh();
        //mesh_f.mesh = mesh;

        VectorReffrence a = new VectorReffrence(vert_h[0].transform.localPosition);
        VectorReffrence b = new VectorReffrence(vert_h[1].transform.localPosition);
        VectorReffrence c = new VectorReffrence(vert_h[2].transform.localPosition);
        VectorReffrence d = new VectorReffrence(vert_h[3].transform.localPosition);
        VectorReffrence c1 = new VectorReffrence(vert_h[2].transform.localPosition);
        VectorReffrence d1 = new VectorReffrence(vert_h[3].transform.localPosition);
        pain(mesh_f.mesh, a, b, c, d);
        */
        // vert_h[1].transform.localPosition
        //meshf.mesh=pain(meshf.mesh, vert_h[0].transform.localPosition, vert_h[1].transform.localPosition, vert_h[2].transform.localPosition, vert_h[3].transform.localPosition);

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

    //  make a object that can hold a mesh and its VectorReffrence's of that mesh aswell as edges.
    //   this object should have a function that can be called by VectorObject interface.
    //    
    //    
    /// <summary>
    /// holds edges
    /// </summary>
    class QuadReffrence //TODO: :MeshReffrence
    {
        VectorReffrence[] meshVectorRef = new VectorReffrence[4 * 6];
        VectorReffrence[] _edges = new VectorReffrence[4 * 2];
        Mesh _mesh = new Mesh();
        Cmesh _this;
        public QuadReffrence()
        {

        }
        public QuadReffrence(Mesh mesh, VectorReffrence[] meshVectorRef, char[] edges)
        {
            this._mesh = mesh;
            this.meshVectorRef[0] = meshVectorRef[0];
            this.meshVectorRef[1] = meshVectorRef[1];

            this.meshVectorRef[2] = meshVectorRef[1];
            this.meshVectorRef[3] = meshVectorRef[2];

            this.meshVectorRef[4] = meshVectorRef[2];
            this.meshVectorRef[5] = meshVectorRef[3];

            this.meshVectorRef[6] = meshVectorRef[3];
            this.meshVectorRef[7] = meshVectorRef[0];
            meshVectorRef = this.meshVectorRef;

            for (int i = 0; i < 7; i += 2)
            {
                if ((edges[i] != '0' && edges[i + 1] != '0'))
                {
                    //  print(i + 1);
                    if (meshVectorRef[i].hasRef && meshVectorRef[i + 1].hasRef)
                    {
                        this._edges[i] = meshVectorRef[i];
                        this._edges[i + 1] = meshVectorRef[i + 1];
                    }
                    else
                    {
                        Debug.LogError("VectorReffrence does not hold a refrance\ncall the construscter with a refrance or assin one");
                    }

                }
            }
        }
        /// <summary>
        /// returns a array of VectorReffrence paird with que 
        /// </summary>
        /// <param name="que">VectorReffrence list to find closest edge</param>
        /// <returns></returns>
        public VectorReffrence[][] getPairableVertex(VectorReffrence[] que, VectorReffrence[] edges)
        {
            List<Vector3> Aout = new List<Vector3>();
            foreach (VectorReffrence vr in que)
            {
                Aout.Add(vr.refPoint);
            }

            //print("VRTV");
            return getPairableVertex(Aout.ToArray(),edges);


        }
        /// <summary>
        /// returns a array of VectorReffrence paird with que 
        /// </summary>
        /// <param name="que">Vector3 list to find closest edge</param>
        public VectorReffrence[][] getPairableVertex(Vector3[] que, VectorReffrence[] edges)//we accept a list so we only go through the edge list once
        {
            
            float[][] distace = new float[que.Length][];//[Mathf.Min(_edges.Length,10)];
            VectorReffrence[][] refP = new VectorReffrence[que.Length][];// Mathf.Min(_edges.Length, 10)];
                                                                         // for (int i = 0; i < _edges.Length; i += 1)
                                                                         //{
                                                                         //double distace=Mathf.Infinity;
                                                                         //    for (int r = 0; r < que.Length; r += 1)
                                                                         //    {
                                                                         //        float d = Vector3.Distance(_edges[i].refPoint, que[r]);
                                                                         //        if (d < distace[r, i])
                                                                         //        {
                                                                         //            distace[r, i] = Mathf.Infinity;

            //        }
            //    }
            //}
           // print("spaces " + edges.Length);
           for (int q = 0; q < que.Length; q += 1) {
                //double distace=Mathf.Infinity;
                distace[q] = new float[edges.Length];
                for (int e = 0; e < edges.Length; e += 1){
                    float d = Vector3.Distance(edges[e].refPoint, que[q]);
                //    print("spaces " + e+" "+q+ " "+ d);

                    distace[q][e] = d;
    
                }
               // distace[q]=qqq;
    // print("spaces-" + que.Length);
            }
           // print("will not print");
            for (int q = 0; q < que.Length; q += 1)
            {

                int[] sorted =insertionSort(distace[q]);//distace[q] holds a list of distances of edges from point q orderd by edge list if the last point is the closest the index of the last item is returned in first postion of the returnd array
                int index = 0;
                refP[q] = new VectorReffrence[sorted.Length];//sorted~= edges// may trunkate
                foreach (int e in sorted)//for loops would give the index but the compiller can predict the block size
                {
                    VectorReffrence y = edges[e];
                    refP[q][index] = y; // for each quary give a list of the closest items in assending order 
               //     print(e);
                    index++;
                }

            }

            return refP;


        }
        public VectorReffrence[] edges
        {
            get
            {
                return _edges;
            }
        }
        public Mesh mesh
        {
            get
            {
                return _mesh;
            }
        }

        /*
        public QuadReffrence(Cmesh _this, Mesh mesh, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {

            _this.pain(mesh, new VectorReffrence(a), new VectorReffrence(b), new VectorReffrence(c), new VectorReffrence(d));

            //  Cmesh e = GetComponent<Cmesh>();
            this._this = _this;
            //_this.pain(mesh, a, b, c, d);
        }
        public QuadReffrence(Cmesh _this, Mesh mesh, VectorReffrence a, VectorReffrence b, VectorReffrence c, VectorReffrence d)
        {

            _this.pain(mesh, a, b, c, d);

            //  Cmesh e = GetComponent<Cmesh>();
            this._this = _this;
            //_this.pain(mesh, a, b, c, d);
        }
        
        /// <summary>
        /// uses the triagles indexes of supplyed mesh to define a quad
        /// this is usefull if you want to join meshes 
        /// </summary>
        /// <param name="_this">a refrence to Cmesh for the functions</param>
        /// <param name="mesh">the data mesh</param>
        /// <param name="a">index in triagnles of vertex a</param>
        /// <param name="b">index in triagnles b</param>
        /// <param name="c">index in triagnles c</param>
        /// <param name="d">index in triagnles d</param>
        public QuadReffrence(Cmesh _this, Mesh mesh, int a, int b, int c, int d)
        {

            //

            //  Cmesh e = GetComponent<Cmesh>();
            this._this = _this;
            _this.pain(mesh, new VectorReffrence(a), new VectorReffrence(b), new VectorReffrence(c), new VectorReffrence(d));
            
           
        }
        */


    }

    class MeshReffrence
    {

        Mesh mesh = null;
        VectorReffrence[] meshVectorRef = null;
        List<QuadReffrence> _MeshQuadReffrence = new List<QuadReffrence>();
        Cmesh _this;
        int[] edges = null;
        public MeshReffrence(Cmesh _this, Mesh mesh, int[] edges, VectorReffrence[] meshVectorRef)
        {
            this._this = _this;
            this.mesh = mesh;
            this.edges = edges;
            this.meshVectorRef = meshVectorRef;
        }
        public MeshReffrence(Cmesh _this,Mesh mesh, VectorReffrence[] meshVectorRef)
        {
            this.mesh = mesh;
            this.meshVectorRef = meshVectorRef;
        }

        VectorReffrence[][] getPairableVertex(VectorReffrence[] que)//TODO: use direction for opptoaztion
        {
        //    print("VR HMM");
            QuadReffrence testE = new QuadReffrence();
            return testE.getPairableVertex(que, findEdges());

           // return refP;
        }
        public List<QuadReffrence> MeshQuadReffrence
        {
            get { return _MeshQuadReffrence; }
        }

        public void buildQuadMesh(Cmesh _this, Mesh mesh, VectorReffrence[] points) {
            QuadReffrence e;
            VectorReffrence[][] refVerts;
       //     print("ready: "+ points.Length.ToString());
            if (points.Length >= 4)
            {
                
                e = _this.pain(mesh, points[0], points[1], points[2], points[3]);
       //         print("has 4!");
                _MeshQuadReffrence.Add(e);
                
                if (points.Length >= 6){
                    for(int i=4;i< points.Length; i+=2)
                    {

                    
                            refVerts = getPairableVertex(new VectorReffrence[] { points[i], points[i+1] });

                             e = _this.pain(mesh, refVerts[1][1], refVerts[0][1], points[i], points[i + 1]);
                             _MeshQuadReffrence.Add(e);

                           // e = _this.pain(mesh, refVerts[1][0], refVerts[0][0], points[i], points[i + 1]);
                           // _MeshQuadReffrence.Add(e);
                            

                    }
                }
            }

/*            VectorReffrence[] refVerts = e.getPairableVertex(new VectorReffrence[] { c1, d1 });
            e= _this.pain(meshf.mesh, refVerts[1], refVerts[0], c1, d1);

            refVerts = e.getPairableVertex(new VectorReffrence[] { c2, d2 });
            e = pain(meshf.mesh, refVerts[1], refVerts[0], c2, d2);
            */
        }
        public VectorReffrence[] findEdges()
        {
            List<VectorReffrence> edgeQue=new List<VectorReffrence>();
            foreach (QuadReffrence quadReffrence in _MeshQuadReffrence)
            {
                foreach (VectorReffrence vert in quadReffrence.edges)
                {
                    if (vert.partEdge)
                    {
                        edgeQue.Add(vert);
                    }
                }

            }
            return edgeQue.ToArray();
        }




    }


    /// <summary>
    /// VectorReffrence is used to hold a vector or the location of a vector in a mesh.
    /// byReff is true if we don't have a vertex
    /// hasReff is true if we have a index and vertex
    /// </summary>
    public class VectorReffrence //VectorReffrence is used to hold a vector or the location of a vector in a mesh. TODO: should it also hold a mesh?
    {
        public string ID = null;


        Vector3 _refPoint;
        int _refPoss=-1;//position in triangles
        int _refSpace=-1;//position in vertices
        bool _byRef = false;
        bool _hasRef = false;
        bool _partEdge = true;
        
        GameObject _refObject = null;
        public override string ToString()
        {
            return base.ToString()+ "\nrefPoss: " + _refPoint.ToString() + "\nrefPoss: " + _refPoss.ToString() + "\nrefSpace: " + _refSpace.ToString() + "\nbyRef: " + _byRef.ToString() + "\nhasRef: " + _hasRef.ToString()+ "\npartEdge: " + _partEdge.ToString();
        }

        // sphere.transform.parent = transform;
        //sphere.transform.localPosition = vert[ii];
        //    sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
        /// <summary>
        /// a refrance in 3D space or a pointer in a mesh
        /// 
        /// </summary>
        /// <param name="shape">a mesh that holds the subjected vector</param>
        /// <param name="refPoss">position in triangles</param>
        public VectorReffrence(Mesh shape, int refPoss)
        {
            int refSpace = shape.triangles[refPoss];
            this._refPoint = shape.vertices[refSpace];
            this._refPoss = refPoss;
            this._refSpace = refSpace;
            _hasRef = true;
        }

        /// <summary>
        /// a refrance in 3D space or a pointer in a mesh
        /// 
        /// </summary>
        /// <param name="refPoint">position in 3d Space</param>
        /// <param name="refPoss">position in triangles</param>
        /// <param name="refSpace">position in mesh</param>
        public VectorReffrence(Vector3 refPoint, int refPoss, int refSpace)
        {
            this._refPoint = refPoint;
            this._refPoss = refPoss;
            this._refSpace = refSpace;
            _hasRef = true;//not by reffrance
        }
        public VectorReffrence(GameObject gg, int refPoss, int refSpace)
        {
            this._refObject = gg;
            this._refPoint = gg.transform.position;
            this._refPoss = refPoss;
            this._refSpace = refSpace;
            _hasRef = true;//not by reffrance
        }
        public VectorReffrence(GameObject gg)
        {
            this._refObject = gg;
            this._refPoint = gg.transform.position;

        }

        /// <summary>
        /// a refrance in 3D space or a pointer in a mesh
        /// 
        /// </summary>
        /// <param name="refPoss">position in triangles</param>
        /// <param name="refSpace">position in mesh</param>
        public VectorReffrence(int refPoss, int refSpace)
        {
            this._refPoss = refPoss;
            this._refSpace = refSpace;
            _byRef = true;
            _hasRef = true;
        }

        /// <summary>
        /// a refrance in 3D space or a pointer in a mesh
        /// 
        /// </summary>
 
        /// <param name="refSpace">position in mesh</param>
        public VectorReffrence(int refSpace)
        {
          //  this._refPoss = refPoss;
            this._refSpace = refSpace;
            _byRef = true;
            _hasRef = true;
        }

        /// <summary>
        /// a refrance in 3D space
        /// 
        /// </summary>
        /// <param name="refPoint">position in 3d Space</param>
        public VectorReffrence(Vector3 refPoint)
        {
            this._refPoint = refPoint;
            _refPoss = -1;
        }
        public bool isRefObjectNull
        {
            get
            {
                return _refObject == null;
            }
        }
        public GameObject refObject
        {
            get
            {
                if (_refObject == null)
                {
                    throw new System.ArgumentException("refObject not set!");
                  //  Debug.LogError("refObject not set! using refPoint!", _refObject);
                }
                else
                {
                    _refPoint = _refObject.transform.localPosition;
                }
                //throw new System.ArgumentException("Parameter cannot be null", "original");

                
                return _refObject;
            }
            set
            {
                _refObject = value;
                _refPoint = value.transform.localPosition;
            }
        }
        public bool partEdge
        {
            get
            {
         

                return _partEdge;
            }
            set
            {
                _partEdge = value;
               
            }
        }
        /// <summary>
        /// postion in triangles
        /// </summary>
        public int refPoss
        {
            get { return _refPoss; }
            set
            {
                if (value == -1)
                {
                    //TODO: throw error
                    byRef = false;
                    _refPoss = value;
                    _hasRef = false;
                }
                else
                {
                    _hasRef = true;
                    _refPoss = value;
                }
            }
        }
        /// <summary>
        /// postion in mesh
        /// </summary>
        public int refSpace
        {
            get { return _refSpace; }
            set
            {
                if (value == -1)
                {
                    //TODO: throw error
                    byRef = false;
                    _refSpace = value;
                    _hasRef = false;
                }
                else
                {
                    _hasRef = true;
                    _refSpace = value;
                }
            }
        }
        /// <summary>
        /// point in 3d space
        /// </summary>
        public Vector3 refPoint
        {
            get
            {
                if (_refObject == null)
                {
                    //throw new System.ArgumentException("refObject not set!");
                    Debug.LogWarning("refObject not set! Using refPoint!", _refObject);
                }
                else
                {
                    _refPoint = _refObject.transform.localPosition;
                }
             
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
        public bool hasRef
        {
            get { return _hasRef; }
            set { _hasRef = value; }
        }


    }
}
