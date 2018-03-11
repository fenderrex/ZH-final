using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecttateType : MonoBehaviour {
    public bool alive;
    public bool forcedMenu;
    public float scrollSencitivty=1;
    public GameObject localized;
    // Use this for initialization
    void Start () {
        alive = true;
        forcedMenu =true;

    }

    // Update is called once per frame
    void Update () {
        float sencitivtyp = scrollSencitivty * 10;
        this.transform.parent.position = localized.transform.position;
        if (alive) { 
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward*Time.smoothDeltaTime* sencitivtyp;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                gameObject.transform.position = gameObject.transform.position - gameObject.transform.forward * Time.smoothDeltaTime* sencitivtyp;
            }

            //TODO: if in "all the way" use first person
        }
        else{
            
            


        }

    }
}
