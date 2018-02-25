using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollZoom : MonoBehaviour
{
  
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            gameObject.transform.position= gameObject.transform.position + gameObject.transform.forward;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            gameObject.transform.position = gameObject.transform.position - gameObject.transform.forward;
        }
        
    }
}
