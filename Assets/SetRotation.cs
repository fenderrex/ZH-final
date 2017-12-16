using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRotation : MonoBehaviour {
    public GameObject cart;
    public GameObject angle;
    bool isActive;
    // Use this for initialization
    void Start () {
        isActive = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            cart.transform.rotation = Quaternion.RotateTowards(cart.transform.rotation, angle.transform.localRotation, 5);
        }
    }
    void NodeOpperate(GameObject name)
    {
        if (name == transform.gameObject)
        {
            cart = name.GetComponentInParent<nodeFlow>().cart;
            cart.GetComponent<Cart>().isRotating = false;
        }
    }
    void NodeOpperateExit(GameObject name)
    {
        if (name == transform.gameObject)
        {

        
          isActive =! isActive;
            //  print(name.GetComponentInParent<nodeFlow>().cart.transform.rotation);
            
           // print(name.GetComponentInParent<nodeFlow>().cart.transform.rotation);
            //    Vector3.Lerp(name.transform.localRotation, angle, Time.deltaTime * 5);

        }
    }
}
