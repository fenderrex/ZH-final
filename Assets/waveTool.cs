
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveTool : MonoBehaviour {
    public GameObject pathNode;


    public AnimationCurve PosX;
    public AnimationCurve PosY;
    public AnimationCurve PosZ;
    public AnimationCurve TimeSpeed;
    public WrapMode EndBehavior;
    public WrapMode StartBehavior;
    public GameObject Trail;
    public float TimeScale = 1;
    Vector3 last;
    public Vector3[] newVertices = new Vector3[15];

    public Vector2[] newUV = new Vector2[4];
    public Vector3[] newNormals = new Vector3[4];
    public int[] newTriangles=new int[6];
    Mesh BuildMeshI()
    {
        //= new Mesh();
        for (int wl = 0; wl+1 <=2; wl++)
        {

            newVertices[0] = new Vector3(waveflow[0].PosX.Evaluate(0), waveflow[0].PosY.Evaluate(0),waveflow[wl].PosZ.Evaluate(0));
            newVertices[1] = new Vector3(waveflow[0+1].PosX.Evaluate(0), waveflow[0+1].PosY.Evaluate(0), waveflow[wl + 1].PosZ.Evaluate(0));
            newVertices[2] = new Vector3(waveflow[0].PosX.Evaluate(10/50f), waveflow[0].PosY.Evaluate(10/50), waveflow[wl].PosZ.Evaluate(10/50));

        }

        newUV[0] = new Vector2(0, 0);
        newUV[1] = new Vector2(1, 0);
        newUV[2] = new Vector2(0, 1);
        newUV[3] = new Vector2(1, 1);
        newTriangles[0] = 0;
        newTriangles[1] = 2;
        newTriangles[2] = 1;
        newTriangles[3] = 3;
        newTriangles[4] = 5;
        newTriangles[5] = 4;
        newTriangles[6] = 6;
        newTriangles[7] = 8;
        newTriangles[8] = 7;


        print("");
        newNormals[0] = -Vector3.forward;
        newNormals[1] = -Vector3.forward;
        newNormals[2] = -Vector3.forward;
        newNormals[3] = -Vector3.forward;

        Mesh mesh =GetComponent<MeshFilter>().mesh;
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.normals = newNormals;
        mesh.uv = newUV;
        GetComponent<MeshFilter>().mesh = mesh;
        return mesh;

    }

    
    Arch[] oldRails = null;
    nodeFlow[] waveflow;
    void RefreshRails(bool newBlock)
    {
        //   last = cart.transform.position;
       // RefreshNodes();
        Arch[] rails = RefreshNodes();
        waveflow = new nodeFlow[rails[0].Length];
        if (oldRails != null)
        {
            int iy = 0;
            foreach(Arch r in oldRails) //BigO (1)
            {
                rails[iy].PhysicalNode = r.PhysicalNode;//save PhysicalNode
                rails[iy].rail = r.rail;//save rail
                iy += 1;
            }
        }
        oldRails = rails;

        //nested loop goes over each node once # of node is n Big O (1)
        for (int i=0;i< rails[0].Length;i+=1)//for segmant size
        {
            
            GameObject rail;//the spawning rail
            Transform masterRail=null;//the editors rail
            if (newBlock)//if building a new nodflow rail
            {
                rail = GameObject.CreatePrimitive(PrimitiveType.Cube);//spawn node
                rail.transform.name = "a path of wave";
            }
            else//or load current rail
            {
                // node = Instantiate(pathNode, pathNode.transform.position, new Quaternion(0, 0, 0, 0));// GameObject.CreatePrimitive(PrimitiveType.Cube);
                rail = rails[i].rail;//Inspiration.gameObject;//no
            }


            foreach (Arch segmant in rails)//for segments in rail
            {
                
              //  if (segmant.gotNode())//if we are still processing the segments...
              //  {
                    //side.getNode();
                    GameObject node=null;
                    if (newBlock) {
                        node = Instantiate(pathNode, pathNode.transform.position, new Quaternion(0, 0, 0, 0));// GameObject.CreatePrimitive(PrimitiveType.Cube);
                        segmant.PhysicalNode = node;
                        segmant.rail = rail;
                    }
                    else
                    {
                        // node = Instantiate(pathNode, pathNode.transform.position, new Quaternion(0, 0, 0, 0));// GameObject.CreatePrimitive(PrimitiveType.Cube);

                        node = segmant.PhysicalNode;//why you no work?
                        rail= segmant.rail;
                        //print(node);
                    }
                    
                    node.transform.name = "a node of wave";
                    masterRail = segmant.Inspiration;
                    node.transform.Find("leadOut").localRotation = segmant.Inspiration.Find("leadOut").localRotation;
                    node.transform.Find("leadOut").localPosition = segmant.Inspiration.Find("leadOut").localPosition;
                    node.transform.Find("leadIn").localRotation = segmant.Inspiration.Find("leadIn").localRotation;
                    node.transform.Find("leadIn").localPosition = segmant.Inspiration.Find("leadIn").localPosition;
                   // print(node.transform.name);
                    // print(side.Inspiration.GetComponent<nodeFlow>().EndBehavior);
                    
                    node.transform.position = segmant.getNode();//gets a node in the rail
                    node.transform.parent = rail.transform;

              //  }
                // Gizmos.DrawSphere(side.getNode(), .3f);
                // Gizmos.DrawSphere(side.getNode(), .3f);
                //segmant.resetShape();//refresh stack
            }
            // print(masterRail.parent.gameObject.GetComponent<nodeFlow>().EndBehavior);
            //print(rail.AddComponent<nodeFlow>().EndBehavior);
            if (rail.GetComponent<nodeFlow>() == null)
                rail.AddComponent<nodeFlow>();
            nodeFlow ra=rail.GetComponent<nodeFlow>();
           // print(rail.gameObject.name);
            waveflow[i]= ra;
            
            //  print(masterRail.parent.parent.name);
            rail.transform.parent = masterRail.parent.parent;
            ra.EndBehavior = masterRail.parent.gameObject.GetComponent<nodeFlow>().EndBehavior;
            ra.StartBehavior = masterRail.parent.gameObject.GetComponent<nodeFlow>().StartBehavior;
            
        }

        
    }

    public GameObject wave;
    // Use this for initialization
    void demo()
    {
        wave = transform.gameObject;
        waveTool flow = wave.GetComponent<waveTool>();
        waveTool.Arch[] rails = flow.RefreshNodes();

        for (int wl = 0; wl < waveflow.Length; wl++)
        {
            // print(waveflow[0].PosX);//.Evaluate(0));
            // print(waveflow.Length);
            int index = wl * 3;
            print(1 + index);
            print(newVertices.Length);
            newVertices[0 + index] = new Vector3(waveflow[wl].PosX.Evaluate(0), waveflow[wl].PosY.Evaluate(0), waveflow[wl].PosZ.Evaluate(0));
            newVertices[1 + index] = new Vector3(waveflow[wl + 1].PosX.Evaluate(0), waveflow[wl + 1].PosY.Evaluate(0), waveflow[wl + 1].PosZ.Evaluate(0));
            newVertices[2 + index] = new Vector3(waveflow[wl].PosX.Evaluate(1 / 50f), waveflow[wl].PosY.Evaluate(1 / 50f), waveflow[wl].PosZ.Evaluate(1 / 50f));



        }
        newTriangles[0] = 0;
        newTriangles[1] = 2;
        newTriangles[2] = 1;

        newTriangles[3] = 3;
        newTriangles[4] = 4;
        newTriangles[5] = 5;

        newTriangles[6] = 6;
        newTriangles[7] = 7;
        newTriangles[8] = 8;

        newTriangles[9] = 2;
        newTriangles[10] = 0;
        newTriangles[11] = 7;
        // newVertices[0] = new Vector3(0, 0, 0);
        // newVertices[1] = new Vector3(100, 0, 0);
        // newVertices[2] = new Vector3(0, 100, 0);
        //  newVertices[3] = new Vector3(100, 100, 0);





        newNormals[0] = -Vector3.forward;
        newNormals[1] = -Vector3.forward;
        newNormals[2] = -Vector3.forward;
        newNormals[3] = -Vector3.forward;
        newUV[0] = new Vector2(0, 0);
        newUV[1] = new Vector2(1, 0);
        newUV[2] = new Vector2(0, 1);
        newUV[3] = new Vector2(1, 1);

    }



    void build()
    {
        print("Refreshed nodes");
        wave = transform.gameObject;
        waveTool flow = wave.GetComponent<waveTool>();
        waveTool.Arch[] rails = flow.RefreshNodes();
        print("Refreshed nodes");
        Vector3[] newVertices = new Vector3[90];
        // for (int i = 0; i < waveflow.Length; i+=1)
        //{
        print("sorting rails");
        //      for (int y = 0; y < waveflow[i].PosX.length; y += 2)
        //     {

        int y = 0;
        int i = 0;

        print("building newVertices");
        // float y_alt =  y*.06f;
        //y = y / 50;

        float y_alt = 0;
        if (false) {
            newVertices = new Vector3[4 + (36 * 2)];

            newVertices[0] = new Vector3(waveflow[i].PosX.Evaluate(y_alt), waveflow[i].PosY.Evaluate(y_alt), waveflow[i].PosZ.Evaluate(y_alt));

            newVertices[1] = new Vector3(waveflow[i + 1].PosX.Evaluate(y_alt), waveflow[i + 1].PosY.Evaluate(y_alt), waveflow[i + 1].PosZ.Evaluate(y_alt));

            newVertices[2] = new Vector3(waveflow[i + 1].PosX.Evaluate(y_alt + .03f), waveflow[i + 1].PosY.Evaluate(y_alt + .03f), waveflow[i + 1].PosZ.Evaluate(y_alt + .03f));

            newVertices[3] = new Vector3(waveflow[i].PosX.Evaluate(y_alt + .03f), waveflow[i].PosY.Evaluate(y_alt + .03f), waveflow[i].PosZ.Evaluate(y_alt + .03f));

            y_alt = .06f;
            int index = 4;
            for (; y_alt < 1; y_alt += .06f)
            {
                newVertices[index] = new Vector3(waveflow[i + 1].PosX.Evaluate(y_alt + .03f), waveflow[i + 1].PosY.Evaluate(y_alt + .03f), waveflow[i + 1].PosZ.Evaluate(y_alt + .03f));
                index++;
                newVertices[index] = new Vector3(waveflow[i].PosX.Evaluate(y_alt + .03f), waveflow[i].PosY.Evaluate(y_alt + .03f), waveflow[i].PosZ.Evaluate(y_alt + .03f));
                index++;
            }
            i++;
            //   print(index);
            y_alt = 0;

            newVertices[index] = new Vector3(waveflow[i].PosX.Evaluate(y_alt), waveflow[i].PosY.Evaluate(y_alt), waveflow[i].PosZ.Evaluate(y_alt));
            index++;
            newVertices[index] = new Vector3(waveflow[i + 1].PosX.Evaluate(y_alt), waveflow[i + 1].PosY.Evaluate(y_alt), waveflow[i + 1].PosZ.Evaluate(y_alt));
            index++;
            newVertices[index] = new Vector3(waveflow[i + 1].PosX.Evaluate(y_alt + .03f), waveflow[i + 1].PosY.Evaluate(y_alt + .03f), waveflow[i + 1].PosZ.Evaluate(y_alt + .03f));
            index++;
            newVertices[index] = new Vector3(waveflow[i].PosX.Evaluate(y_alt + .03f), waveflow[i].PosY.Evaluate(y_alt + .03f), waveflow[i].PosZ.Evaluate(y_alt + .03f));
            index++;
            for (; y_alt < 1; y_alt += .06f)
            {
                //       newVertices[index] = new Vector3(waveflow[i + 1].PosX.Evaluate(y_alt + .03f), waveflow[i + 1].PosY.Evaluate(y_alt + .03f), waveflow[i + 1].PosZ.Evaluate(y_alt + .03f));
                //      index++;
                newVertices[index] = new Vector3(waveflow[i].PosX.Evaluate(y_alt + .03f), waveflow[i].PosY.Evaluate(y_alt + .03f), waveflow[i].PosZ.Evaluate(y_alt + .03f));
                index++;
            }

        }
        else if(false)
        {
            int e = 4;
            if (e > 0)
            {
                newVertices[0] = new Vector3(waveflow[i].PosX.Evaluate(y_alt), waveflow[i].PosY.Evaluate(y_alt), waveflow[i].PosZ.Evaluate(y_alt));

                newVertices[1] = new Vector3(waveflow[i + 1].PosX.Evaluate(y_alt), waveflow[i + 1].PosY.Evaluate(y_alt), waveflow[i + 1].PosZ.Evaluate(y_alt));

                newVertices[2] = new Vector3(waveflow[i + 1].PosX.Evaluate(y_alt + .03f), waveflow[i + 1].PosY.Evaluate(y_alt + .03f), waveflow[i + 1].PosZ.Evaluate(y_alt + .03f));

                newVertices[3] = new Vector3(waveflow[i].PosX.Evaluate(y_alt + .03f), waveflow[i].PosY.Evaluate(y_alt + .03f), waveflow[i].PosZ.Evaluate(y_alt + .03f));
            }  if (e > 1) {


                newVertices[4] = new Vector3(waveflow[i + 2].PosX.Evaluate(y_alt), waveflow[i + 2].PosY.Evaluate(y_alt), waveflow[i + 2].PosZ.Evaluate(y_alt));

                newVertices[5] = new Vector3(waveflow[i + 2].PosX.Evaluate(y_alt + .03f), waveflow[i + 2].PosY.Evaluate(y_alt + .03f), waveflow[i + 2].PosZ.Evaluate(y_alt + .03f));

                newVertices[6] = new Vector3(waveflow[i + 3].PosX.Evaluate(y_alt + .03f), waveflow[i + 3].PosY.Evaluate(y_alt + .03f), waveflow[i + 3].PosZ.Evaluate(y_alt + .03f));

                newVertices[7] = new Vector3(waveflow[i + 3].PosX.Evaluate(y_alt), waveflow[i + 3].PosY.Evaluate(y_alt), waveflow[i + 3].PosZ.Evaluate(y_alt));
            }  if (e > 2) { 

                y_alt = .06f;
                newVertices[9] = new Vector3(waveflow[i].PosX.Evaluate(y_alt), waveflow[i].PosY.Evaluate(y_alt), waveflow[i].PosZ.Evaluate(y_alt));

                newVertices[8] = new Vector3(waveflow[i + 1].PosX.Evaluate(y_alt), waveflow[i + 1].PosY.Evaluate(y_alt), waveflow[i + 1].PosZ.Evaluate(y_alt));

                newVertices[11] = new Vector3(waveflow[i + 2].PosX.Evaluate(y_alt + .03f), waveflow[i + 2].PosY.Evaluate(y_alt + .03f), waveflow[i + 2].PosZ.Evaluate(y_alt + .03f));

                newVertices[10] = new Vector3(waveflow[i + 3].PosX.Evaluate(y_alt + .03f), waveflow[i + 3].PosY.Evaluate(y_alt + .03f), waveflow[i + 3].PosZ.Evaluate(y_alt + .03f));
            }
        }else if (true)
        {
            Cmesh cm = GetComponent<Cmesh>();
            cm.MeshObject("wall");
            cm.MeshObject("roof");
            cm.MeshObject("wall");
            cm.MeshObject("floor");
            i = 0;
            for (int rLoop = 0 ;rLoop <= cm._meshes.Count-1 ;rLoop++)
            {
                int a, b;
                if (cm._meshes.Count-1 == rLoop)
                {
                    print("we are not over indexing");
                    a = i;
                    b = 0;//don't over index
                }
                else
                {
                    a = i;
                    b = i+1;
                }
                for (int sub_I = 0; sub_I < 80; sub_I += 2)
                {
                    y_alt = sub_I / 40.0f;


                    
                    newVertices[sub_I + 1] = new Vector3(waveflow[a].PosX.Evaluate(y_alt), waveflow[a].PosY.Evaluate(y_alt), waveflow[a].PosZ.Evaluate(y_alt));

                    newVertices[sub_I] = new Vector3(waveflow[b].PosX.Evaluate(y_alt), waveflow[b].PosY.Evaluate(y_alt), waveflow[b].PosZ.Evaluate(y_alt));
                    
                }
                cm.buildMesh(newVertices, cm._meshes.ToArray()[i].GetComponent<MeshFilter>());
                i++;
            }
           /* for (int sub_I = 0; sub_I < 20; sub_I += 2)
            {
                y_alt = (sub_I) / 20.0f;
                newVertices[sub_I + 1] = new Vector3(waveflow[i].PosX.Evaluate(y_alt), waveflow[i].PosY.Evaluate(y_alt), waveflow[i].PosZ.Evaluate(y_alt));

                newVertices[sub_I] = new Vector3(waveflow[i + 1].PosX.Evaluate(y_alt), waveflow[i + 1].PosY.Evaluate(y_alt), waveflow[i + 1].PosZ.Evaluate(y_alt));

            }
            cm.buildMesh(newVertices, cm._meshes.ToArray()[i].GetComponent<MeshFilter>());
            */





        }



        //    }
        // }





    }




    // Update is called once per frame

    Mesh BuildMesh()
    {
        //= new Mesh();
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
    void debugGraphs(){
        Arch[] rails = RefreshNodes();
       // BuildMesh();
        Vector3 start = new Vector3(PosX.Evaluate(0), PosY.Evaluate(0), PosZ.Evaluate(0));
    Gizmos.color = Color.red;
        foreach (Arch segmant in rails)
        {
           // print(segmant);
           // print(segmant.PhysicalNode);
      
             //   Gizmos.DrawSphere(segmant.getNode(), .3f);

            
           // segmant.resetShape();
            //Gizmos.DrawSphere(side.getNode(), .3f);
            //Gizmos.DrawSphere(side.getNode(), .3f);
        }

        for (float lineSeg = 1 / 50f; lineSeg<transform.childCount* 20; lineSeg += 1 / 50f)
        {
            Gizmos.color = Color.blue;
            GameObject myLine = new GameObject();
            myLine.transform.position = start;
            myLine.AddComponent<LineRenderer>();
            LineRenderer lr = myLine.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lr.startColor = Color.blue;
            lr.endColor = Color.blue;
            lr.startWidth = .1f;
            lr.endWidth = .1f;
            //(0.1f, 0.1f);
            lr.SetPosition(0, start);
            lr.SetPosition(1, new Vector3(PosX.Evaluate(lineSeg), PosY.Evaluate(lineSeg), PosZ.Evaluate(lineSeg)));
            Gizmos.DrawLine(start, new Vector3(PosX.Evaluate(lineSeg), PosY.Evaluate(lineSeg), PosZ.Evaluate(lineSeg)));
            start = new Vector3(PosX.Evaluate(lineSeg), PosY.Evaluate(lineSeg), PosZ.Evaluate(lineSeg));

        }

    }
    public Arch[] RefreshNodes()
    {

    int index = 0;
        
        float min = -Mathf.Infinity;
        Arch[] data = new Arch[transform.childCount];
        //arch(child.position + (child.right * 3), child.position - (child.right * 3));

        foreach (Transform child in transform)
        {
            // Arch i=new Arch(child.position + (child.right * 3), child.position - (child.right * 3));
            Vector3[] shapei = {
                    child.position + (child.right * 3),
                    child.position + (child.right * 3)+(child.up * 7),
                    //child.position +(child.up * 7),
                    child.position - (child.right * 3)+(child.up * 7),
                    child.position - (child.right * 3)};

           // data[index] = new Arch(child, child.position + (child.right * 3), child.position - (child.right * 3));

            data[index] = new Arch(child, shapei);


            index += 1;
        }
        // print(index);
        index = 0;
        return data;
       

    }

    // Update is called once per frame
    private void Start()
    {
       
        RefreshRails(true);
        // BuildMesh();
       // build();
        //debugGraphs();
    }
    void Update()
    {


        

        if (Time.frameCount % 240 == 0)
        {
            Debug.Log("lets fuckup!");
            build();//  RefreshRails(false);
        }
        float currentT = 0;
        //  print(Time.time % currentT);
        float time = Time.time * Mathf.Round(TimeScale * 100f) / 100f;

        //BuildMesh();
        
    }
    public bool setVer=true;

    /// <summary>
    /// holds nodes in a shape
    /// </summary>
    public class Shape
    {
        public Node[] shape;
        public Node main;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="ii"></param>
        Shape(Node i, Node[] ii)
        {
            main = i;
            shape = ii;
        }

    }
    /// <summary>
    /// Holds a wave segment
    /// </summary>
    public class Arch//this holds a rail segment
    {

        Transform _inspiration;
        GameObject _PhysicalNode;
        GameObject _rail;
        public GameObject rail
        {
            get { return _rail; }
            set { _rail = value; }
        }
        public int Length
        {
            get { return shape.Length; }

        }
        public GameObject PhysicalNode
        {
            get { return _PhysicalNode; }
            set { _PhysicalNode = value; }
        }
        public Transform Inspiration
        {
            get { return _inspiration; }
            set { _inspiration = value; }

        }
        public override string ToString()
        {
            string dat = "[";
            foreach (Vector3 i in shape) {
               
                dat = dat + " " + i + ", ";
            }
            dat += " ]";
            return dat;
        }
        Vector3[] shape;//all parts of the rail segment
        int size = 0;
        int index = 0;
        public void resetShape()
        {
            //Vector3 node = this.shape[index];
            index = size;
        }
        public Vector3 getNode()
        {

            Vector3 node = this.shape[index];
           // Shape(_inspiration);
            index -= 1;
            return node;
        }
        public bool gotNode()
        {
            return (index >= 0);
        }
        public Arch(Transform inspiration, Vector3 left, Vector3 right)
        {
            this._inspiration = inspiration;
            index = 1;
            size = 1;
            this.shape =new Vector3[]{ left,right};
          //  e = 1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspiration"></param>
        /// <param name="shape"></param>
        public Arch(Transform inspiration,Vector3[] shape)//should we copy the array??
        {
            this._inspiration = inspiration;

            index = shape.Length-1;
            size = index;
            this.shape = shape;
        }
    }

}