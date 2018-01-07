using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeFlow : MonoBehaviour {
    public AnimationCurve PosX;
    public AnimationCurve PosY;
    public AnimationCurve PosZ;
    public AnimationCurve TimeSpeed;
    public WrapMode EndBehavior;
    public WrapMode StartBehavior;
    public GameObject cart;
    public GameObject Trail;
    public float TimeScale = 1;
    Vector3 last;

    // Use this for initialization
    void Start () {
        if (cart == null)
        {
            cart = new GameObject();
            cart.name = "a cart";
            Trail = new GameObject();
            Trail.name = "a cart trail";
        }
        last = cart.transform.position;
        RefreshNodes(transform);

    }
    void OnDrawGizmos()
    {
        RefreshNodes(transform);
        Vector3 start= new Vector3(PosX.Evaluate(0), PosY.Evaluate(0), PosZ.Evaluate(0));


        for (float lineSeg = 1/50f; lineSeg < transform.childCount*20; lineSeg+=1/50f)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawLine(start, new Vector3(PosX.Evaluate(lineSeg), PosY.Evaluate(lineSeg), PosZ.Evaluate(lineSeg)));
            start = new Vector3(PosX.Evaluate(lineSeg), PosY.Evaluate(lineSeg), PosZ.Evaluate(lineSeg));

        }
        
    }
    public void RefreshNodes(Transform path)
    {
        PosX = new AnimationCurve();
        PosY = new AnimationCurve();
        PosZ = new AnimationCurve();
        TimeSpeed = new AnimationCurve();
        PosX.postWrapMode = EndBehavior;
        PosY.postWrapMode = EndBehavior;
        PosZ.postWrapMode = EndBehavior;
        TimeSpeed.postWrapMode = EndBehavior;
        PosX.preWrapMode = StartBehavior;
        PosY.preWrapMode = StartBehavior;
        PosZ.preWrapMode = StartBehavior;
        TimeSpeed.preWrapMode = StartBehavior;
        int index = 0;
        
        float min = -Mathf.Infinity;

        foreach (Transform child in path)
        {
            float a = Vector3.Distance(last, child.position);
            if (min < a)
            {
                min = a;
            }
        //    print(child.name);
            PosX.AddKey(new Keyframe(index, child.position.x, child.Find("leadIn").localPosition.x * 4, child.Find("leadOut").localPosition.x * 4));
            PosY.AddKey(new Keyframe(index, child.position.y, child.Find("leadIn").localPosition.y * 4, child.Find("leadOut").localPosition.y * 4));
            PosZ.AddKey(new Keyframe(index, child.position.z, child.Find("leadIn").localPosition.z * 4, child.Find("leadOut").localPosition.z * 4));
            index += 1;
        }
        index = 0;
        Transform[] nodes = new Transform[transform.childCount];
        foreach (Transform child in transform)
        {
            nodes[index] = child;
            index += 1;
        }
        index = 0;
        float lastTT = 1;// Vector3.Distance(last, nodes[nodes.Length].position); ;
        foreach (Transform child in nodes)
        {
            
            //if (transform.childCount - 1 != index) { 

            float a = Vector3.Distance(last, child.position);
            float l = a / min;

            //print(l);
            float tangent = l - lastTT;
            if (index+1 < nodes.Length)
            {
               // Debug.Log(nodes.Length);


                if (a<Vector3.Distance(nodes[index].position, nodes[index + 1].position) ){
                 //   tangent = tangent * -1;

                }
            }
            TimeSpeed.AddKey(new Keyframe(index, l, Mathf.Tan(l - lastTT), Mathf.Tan(tangent)));
            lastTT = l;
            last = child.position;
            //  }
            index += 1;
        }

    }

    // Update is called once per frame
    void Update() {

        if (Time.frameCount % 20 == 0)
        {
            RefreshNodes(transform);
        }
        float currentT = 0;
        //  print(Time.time % currentT);
       float time = Time.time* Mathf.Round(TimeScale * 100f) / 100f; ;
        if (time > currentT) //- //(currentT*TimeScale * 5))
        {
            currentT = (Time.time + (TimeSpeed.Evaluate(time)));
            cart.transform.position = new Vector3(PosX.Evaluate(time), PosY.Evaluate(time), PosZ.Evaluate(time));
            Trail.transform.position= new Vector3(PosX.Evaluate((time+.1f)), PosY.Evaluate((time+.1f)), PosZ.Evaluate((time+ .1f)));
            // currentT = (time + (TimeSpeed.Evaluate(time)));
        }

	}
}