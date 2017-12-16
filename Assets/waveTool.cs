
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

    Arch[] oldRails = null;
    void RefreshRails(bool newBlock)
    {
        //   last = cart.transform.position;
       // RefreshNodes();
        Arch[] rails = RefreshNodes();
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
        Vector3 start = new Vector3(PosX.Evaluate(0), PosY.Evaluate(0), PosZ.Evaluate(0));
        Gizmos.color = Color.red;
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
            ra.EndBehavior = masterRail.parent.gameObject.GetComponent<nodeFlow>().EndBehavior;
            ra.StartBehavior = masterRail.parent.gameObject.GetComponent<nodeFlow>().StartBehavior;
            
        }
        

    }

    void OnDrawGizmos()
    {
        Arch[] rails = RefreshNodes();
        Vector3 start = new Vector3(PosX.Evaluate(0), PosY.Evaluate(0), PosZ.Evaluate(0));
        Gizmos.color = Color.red;
        foreach (Arch segmant in rails)
        {
           // print(segmant);
           // print(segmant.PhysicalNode);
            while (segmant.gotNode())
            {
                Gizmos.DrawSphere(segmant.getNode(), .3f);

            }
           // segmant.resetShape();
            //Gizmos.DrawSphere(side.getNode(), .3f);
            //Gizmos.DrawSphere(side.getNode(), .3f);
        }

        for (float lineSeg = 1 / 50f; lineSeg < transform.childCount * 20; lineSeg += 1 / 50f)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawLine(start, new Vector3(PosX.Evaluate(lineSeg), PosY.Evaluate(lineSeg), PosZ.Evaluate(lineSeg)));
            start = new Vector3(PosX.Evaluate(lineSeg), PosY.Evaluate(lineSeg), PosZ.Evaluate(lineSeg));

        }

    }
    void debugGraphs(){
        Arch[] rails = RefreshNodes();
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
            Vector3[] shapei = { child.position + (child.right * 3),
                    child.position + (child.right * 3)+(child.up * 7), child.position - (child.right * 3), child.position - (child.right * 3)+(child.up * 7)};

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
        debugGraphs();
    }
    void Update()
    {

        if (Time.frameCount % 20 == 0)
        {
          //  RefreshRails(false);
        }
        float currentT = 0;
        //  print(Time.time % currentT);
        float time = Time.time * Mathf.Round(TimeScale * 100f) / 100f; 


    }
    public class Shape
    {
        public Node[] shape;
        public Node main;
        Shape(Node i, Node[] ii)
        {
            main = i;
            shape = ii;
        }

    }
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
        public Arch(Transform inspiration,Vector3[] shape)//should we copy the array??
        {
            this._inspiration = inspiration;

            index = shape.Length-1;
            size = index;
            this.shape = shape;
        }
    }

}