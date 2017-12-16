using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour
{
    Collider myCollider;
    // Use this for initialization


    void Start()
    {
        //myCollider = gameObject.GetComponent<Collider>();

    }
    void NodeOpperate(GameObject name)
    {
    }
    void NodeOpperateExit(GameObject name)
    {
    }
    private void OnCollisionExit(Collision collision)
    {
        print(collision.gameObject.name);

        if (collision.gameObject.Equals(transform.parent.GetComponent<nodeFlow>().cart))
        {
            // foreach(GameObject script in scripts)
            //{
            transform.gameObject.SendMessage("NodeOpperateExit", transform.gameObject);

            // }
        }
        // ContactPoint contact = collision.contacts[0];
        //   Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        // Vector3 pos = contact.point;
    }
    void OnCollisionEnter(Collision collision)
    {
        //   print(collision.gameObject.name);

        if (collision.gameObject.Equals(transform.parent.GetComponent<nodeFlow>().cart))
        {
            // foreach(GameObject script in scripts)
            //{
            transform.gameObject.SendMessage("NodeOpperate", transform.gameObject);

            // }
        }
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        //   Instantiate(c, pos, rot);
        //   Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
