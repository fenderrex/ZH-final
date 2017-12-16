using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookpos : MonoBehaviour {
    public GameObject target;
    GameObject body;
    public GameObject bodyTween;
    bool active;
    // Use this for initialization
    void Start () {
       bodyTween= new GameObject("name");
        body=transform.parent.GetComponent<nodeFlow>().cart;

       // active = false;

        // Instantiate(bodyTween, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update() {
      //  body.GetComponent<Cart>().isRotating = active;

        if (body.GetComponent<Cart>().isRotating)
        {

            bodyTween.transform.position = body.transform.position;
            bodyTween.transform.LookAt(target.transform);
            body.transform.rotation= Quaternion.RotateTowards(body.transform.rotation, bodyTween.transform.rotation,48f);

        }

    }

    void NodeOpperateExit(GameObject name)
    {
        if (name == transform.gameObject)
        {
            body = name.GetComponentInParent<nodeFlow>().cart;

            body.GetComponent<Cart>().isRotating = !body.GetComponent<Cart>().isRotating;
          //  body.GetComponent<Cart>().isRotating = !body.GetComponent<Cart>().isRotating;
          //  print("11");
               

        }
    }

}
