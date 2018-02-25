using System;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("Camera-Control/Mouse Look")]
public class lookpos : MonoBehaviour {
    public GameObject target;
    GameObject body;
    public GameObject bodyTween;
    public bool active;


    public enum RotationAxes { XAndY = 0, X = 1, Y = 2 }
    public RotationAxes axes = RotationAxes.XAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationX = 0F;
    float rotationY = 0F;
    Quaternion originalRotation;
    void Start()
    {
        body = this.gameObject;
        bodyTween = new GameObject();
    }
    void smothVector(Vector2 Vin)
    {
        body = this.gameObject;
        if (axes == RotationAxes.XAndY)
        {
            // Read the mouse input axis
            rotationX += Vin.x * sensitivityX;
            rotationY += Vin.y * sensitivityY;
            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY, maximumY);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
            transform.rotation = originalRotation * xQuaternion * yQuaternion;
        }
        else if (axes == RotationAxes.X)
        {
            rotationX += Vin.x * sensitivityX;
            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);



 

            Quaternion a= originalRotation * xQuaternion;
            float w = a.w;
            float x = a.x;
            float y = a.y;
            float z = a.z;

            float roll = Mathf.Atan2(2 * y * w + 2 * x * z, 1 - 2 * y * y - 2 * z * z);
            float pitch = Mathf.Atan2(2 * x * w + 2 * y * z, 1 - 2 * x * x - 2 * z * z);
            float yaw = Mathf.Asin(2 * x * y + 2 * z * w);
            transform.localEulerAngles = new Vector3(roll, 0, yaw);

        }
        else
        {
           // rotationY += Input.GetAxis("Y") * sensitivityY;
            rotationY = ClampAngle(rotationY, minimumY, maximumY);
            Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
            transform.localRotation = originalRotation * yQuaternion;




        }

        
        // Use this for initialization

        //    Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));

        

    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
    // Update is called once per frame
    void Update() {
        body = this.gameObject;
        //  body.GetComponent<Cart>().isRotating = active;
        if (active)
        {
            lookNow();
        }
        else if (body.GetComponent<Cart>().isRotating)
        {
            lookNow();
        }


    }
    void lookNow()
    {
        Quaternion xQuaternion;
        bodyTween.transform.position = body.transform.position;
        bodyTween.transform.LookAt(target.transform);
        Quaternion lookto;
        if (axes == RotationAxes.XAndY)
        {
            gameObject.transform.rotation = bodyTween.transform.rotation;
            //  gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);


        }
        else if (axes == RotationAxes.X)
        {

            float log = AngleOffAroundAxis(bodyTween.transform.forward, gameObject.transform.position, bodyTween.transform.up);
            xQuaternion = Quaternion.AngleAxis(log, Vector3.up);

            bodyTween.transform.rotation = xQuaternion * bodyTween.transform.rotation;
            //  gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, bodyTween.transform.rotation, 48f);
        }
        gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, bodyTween.transform.rotation, 48f);

    }
    public static float AngleOffAroundAxis(Vector3 v, Vector3 forward, Vector3 axis)
    {
        Vector3 right = Vector3.Cross(axis, forward).normalized;
        forward = Vector3.Cross(right, axis).normalized;
        return Mathf.Atan2(Vector3.Dot(v, right), Vector3.Dot(v, forward)) * Mathf.Rad2Deg;

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
