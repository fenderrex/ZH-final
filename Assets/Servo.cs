using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Servo : MonoBehaviour {
    public string ipAddress= "192.168.1.100";
   // public PlayVideo videotool;// = this.GetComponent<PlayVideo>();
    UnityEngine.Video.VideoPlayer player;// = videotool.VP;
    
    void Start()
    {
        // videotool =
        player = this.GetComponent<nodeFlow>().cart.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.Video.VideoPlayer>();

    }
    private void Update()
    {
        if (Time.frameCount %30 == 0)
        {
            ulong lengh = player.frameCount;
            long current = player.frame;
            StartCoroutine(setVal((float)(180*((long)current*3 / 45)/400)));
            // StartCoroutine(setVal(0));//(int)((long)current/(long)lengh)));
        }
    }
    IEnumerator setVal(float data)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://"+ipAddress+"/?json={\"data\":"+ data*10 + "}"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                string json=www.downloadHandler.text;
                NewJson myObject = JsonUtility.FromJson<NewJson>(json);
                float i = (myObject.data - 500) / 200;
                this.GetComponent<nodeFlow>().TimeScale = i;
                Debug.Log(i);
                // Debug.Log(json);
                // Or retrieve results as binary data
                player.playbackSpeed = i;
                byte[] results = www.downloadHandler.data;
            }
        }
    }
    [System.Serializable]
    public class NewJson
    {



        public float data;


    }
}
