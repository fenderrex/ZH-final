using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVideo : MonoBehaviour {
    public UnityEngine.Video.VideoPlayer VP;
    public int frame = 0;
    public bool RestartFromTopEachTime = true;
    bool first =true;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void NodeOpperate(GameObject name)
    {
        if (name == transform.gameObject)
        {
            if (RestartFromTopEachTime == true)
            {
                VP.frame = frame;
            }
            else if ( first==true)
            {
                VP.frame = frame;
            }
            VP.EnableAudioTrack(0,true);
            VP.Play();
        }
    }
}
