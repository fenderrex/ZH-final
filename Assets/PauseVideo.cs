using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseVideo : MonoBehaviour {
    public UnityEngine.Video.VideoPlayer VP;
    void NodeOpperate(GameObject name)
    {
        if (name == transform.gameObject)
        {
            VP.Pause();
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
