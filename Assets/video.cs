using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class video : MonoBehaviour {
    public string fi="test.mp4";
    // Use this for initialization
    public AnimationCurve tt;
    public AnimationCurve PX;
    public AnimationCurve PY;
    GameObject offset;
    // Will attach a VideoPlayer to the main camera.
    // VideoPlayer automatically targets the camera backplane when it is added
    // to a camera object, no need to change videoPlayer.targetCamera.
    UnityEngine.Video.VideoPlayer videoPlayer;//= camera.AddComponent<UnityEngine.Video.VideoPlayer>();
    void Start () {
        // GameObject camera = GameObject.Find("Main Camera");
        offset = new GameObject("video offet grip");
        // offset=(GameObject)Instantiate(offset, transform.position, transform.rotation);
        // offset.transform.position = transform.position;
        offset.transform.parent=transform.parent;
        videoPlayer = this.GetComponent<UnityEngine.Video.VideoPlayer>();//camera.AddComponent<UnityEngine.Video.VideoPlayer>();





        // Play on awake defaults to true. Set it to false to avoid the url set
        // below to auto-start playback since we're in Start().
        videoPlayer.playOnAwake = false;

        // By default, VideoPlayers added to a camera will use the far plane.
        // Let's target the near plane instead.
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;

        // This will cause our scene to be visible through the video being played.
        videoPlayer.targetCameraAlpha = 1F;
        // Set the video to play. URL supports local absolute or relative paths.
        // Here, using absolute.
        videoPlayer.url = fi;// "/Users/graham/movie.mov";
        videoPlayer.Play();
        videoPlayer.Pause();
        //tt = new AnimationCurve(new Keyframe(0, 0,0,Mathf.Sin( videoPlayer.frameCount)*-1f), new Keyframe(videoPlayer.frameCount, videoPlayer.frameCount, Mathf.Sin(videoPlayer.frameCount)*-1,0));
        tt = new AnimationCurve(new Keyframe(0,1), new Keyframe(videoPlayer.frameCount,1));
        // Skip the first 100 frames.
        videoPlayer.frame = 100;
        // Restart from beginning when done.
        videoPlayer.isLooping = true;
        // Each time we reach the end, we slow down the playback by a factor of 10.
        videoPlayer.loopPointReached += EndReached;
        // Start playback. This means the VideoPlayer may have to prepare (reserve
        // resources, pre-load a few frames, etc.). To better control the delays
        // associated with this preparation one can use videoPlayer.Prepare() along with
        // its prepareCompleted event.
        tt.postWrapMode = WrapMode.Loop;
        tt.preWrapMode = WrapMode.Loop;
        videoPlayer.Play();
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        //vp.playbackSpeed = vp.playbackSpeed / 10.0F;
    }
    // Update is called once per frame
    void Update () {
        if (Input.GetKey("p"))
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();//  print("up arrow key is held down");
            }
            else
            {
                videoPlayer.Play();
            }
        }
        if (Input.GetKeyDown("n"))
        {

                videoPlayer.Pause();//  print("up arrow key is held down");
            videoPlayer.frame += 1;
        }
        videoPlayer.playbackSpeed = tt.Evaluate(videoPlayer.frame);
       // videoPlayer.b
        //videoPlayer.time= tt.Evaluate(Time.time);
    }
}
