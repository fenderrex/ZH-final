using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockZero : MonoBehaviour {
    public Vector3 lockOffsetPoss = new Vector3(0, -1.086f, 0);
    public Vector3 lockOffset = new Quaternion().eulerAngles;
    public GameObject outputTargit;

    public enum RotationAxes { pos = 0, rot = 1, both = 2 }
    public RotationAxes axes = RotationAxes.pos;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (axes == RotationAxes.pos)
        {
            gameObject.transform.localPosition = lockOffsetPoss;
        }
        if (axes == RotationAxes.rot)
        {
            UpdateRotation();
        }
        if (axes == RotationAxes.both)
        {
            UpdateRotation();
            gameObject.transform.localPosition = lockOffsetPoss;
            Vector3 lockOffset = outputTargit.transform.eulerAngles;
        }
    }

  

    
    void UpdateRotation()
    {
        Quaternion xQuaternion = Quaternion.AngleAxis(lockOffset.x, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(lockOffset.y, -Vector3.right);
        Quaternion zQuaternion = Quaternion.AngleAxis(lockOffset.z, -Vector3.forward);
        outputTargit.transform.rotation = outputTargit.transform.rotation * yQuaternion;// * yQuaternion * zQuaternion;
    }
}
