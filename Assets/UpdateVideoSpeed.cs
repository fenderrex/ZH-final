using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateVideoSpeed : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    public float playReate = 1;
public UnityEngine.Video.VideoPlayer VP;
    void NodeOpperate(GameObject name)
    {
        if (name == transform.gameObject)
        {
            VP.playbackSpeed = playReate;
        }
    }
}
