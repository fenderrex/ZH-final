using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.Networking;

using System.Collections;

public class locomove : MonoBehaviour {
    bool isOn = false;
    public bool falling, stubble, idleing, walking, sitting = false;
    public float walkingForward;
    public float walkingLeft;
    // Use this for initialization
    float distToGround =0;
    public string ipAddress = "192.168.1.100";

    Vector3 reflectVec;
    public float speed = 6.0F;
    float jumpSpeed = 16.0F;
    public float jumpSpeedGoal = 16.0F;
    public float gravity = 20.0F;
    bool stableFooting = true;
    private Vector3 moveDirection = Vector3.zero;
    float DJtimer = 0;

    bool jumpRelease=false;
    RaycastHit hit;
    float sy = 0;
    Ray ray = new Ray(new Vector3(0,0,0), new Vector3(0, -1, 0));
    public bool onDrugs = false;
    public Actuation movments;


    bool IsGrounded(Transform t)
    {
        falling = Physics.Raycast(t.position, -Vector3.up, distToGround + 0.1f);
        return falling;
    }

    public class Actuation
    {
      
        DefaltLogWalker _DLW;
        public Actuation()
        {
            
            _DLW = new DefaltLogWalker();
        }
        public interface IWalkable
        {
            void walk(Vector3 to);
            void stumble();
            void fall(Vector3 to);
        }





        public DefaltLogWalker DLW
        {

            get { return _DLW; }
            set { _DLW = value; }

        }

        public class DefaltLogWalker : IWalkable
        {
            public void walk(Vector3 toWrite)
            {
                print(toWrite);
            }
            public void stumble()
            {
                print("stumble");
            }
            public void fall(Vector3 delta)
            {
                print(delta);
            }
        }



     
    private IWalkable walker;
            


        public void setwalker(IWalkable walkable)
        {
            this.walker = walkable;
        }
        public void walk(Vector3 delta)
        {
            this.walker.walk(delta);
        }
        public void fall(Vector3 delta)
        {
            this.walker.fall(delta);
        }


    }

    float hight = 0;
    void Awake()
    {
        movments = new Actuation();
        Actuation.DefaltLogWalker i = movments.DLW;//defalt log walker
      //  i.stumble();
        movments.setwalker(i);//
        
      //  movments.walk(new Vector3(2, 0, 2));//set to idle

        distToGround = transform.GetComponent<Collider>().bounds.extents.y;
        DJtimer = Time.time;


    }

    void Update()
    {
       // moveDirection = Vector3.zero;
        Vector3 slideAngle = Vector3.zero;// = Vector3.Cross(rotHandleForSlide, hit.normal);
       
        // float sy=0;

        ray.origin = transform.position;
        if (jumpRelease == false)//test if jump processing
        {
            jumpRelease = Input.GetButtonUp("Jump");
        }
        if (Physics.Raycast(ray, out hit))
        {
        
            Vector3 incomingVec = hit.point - transform.position;

            reflectVec = Vector3.Reflect(incomingVec, hit.normal);
            Debug.DrawLine(transform.position, hit.point, Color.red);
            Debug.DrawRay(hit.point, reflectVec, Color.green);
            Debug.DrawRay(hit.point, hit.normal, Color.blue);
            Vector3 rotHandleForSlide = Vector3.Cross(reflectVec, hit.normal);
            Debug.DrawRay(hit.point, rotHandleForSlide, Color.white);
            slideAngle = Vector3.Cross(rotHandleForSlide, hit.normal);
            Debug.DrawRay(hit.point, slideAngle, Color.yellow);
            // print(Vector3.AngleBetween(incomingVec, slideAngle));
            sy=((Vector3.Angle(incomingVec, slideAngle) / Mathf.PI));
           // print(sy);
           // print(ray.origin);
            // Vector3.Cross(Vector3.Cross(reflectVec, hit.normal), hit.normal)
           

        }
        CharacterController controller = GetComponent<CharacterController>();
        if (onDrugs)
        {
            if (Input.GetButton("Jump"))//jump
            {
                // print("jump");
                moveDirection.y = jumpSpeed;
                stableFooting = false;
            }
            //moveDirection.y = gravity * Time.deltaTime;
        }
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        walkingForward = Input.GetAxis("Vertical");
        walkingLeft = Input.GetAxis("Horizontal");
        moveDirection.y += jumpSpeed;
        if (jumpSpeed > 0)
        {

            jumpSpeed -= 1f;// print(slide);

        }
        if (controller.isGrounded||IsGrounded(transform))
        {
          //  moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = new Vector3(moveDirection.x,-((gravity) * Time.deltaTime), moveDirection.z);

            moveDirection *= speed;
            Vector3 moveDirection1 = new Vector3(walkingForward, 0, walkingLeft);
            movments.walk(moveDirection);
            movments.fall(moveDirection);
            Vector3 slide = new Vector3((Mathf.Abs(slideAngle.x) * 180) / Mathf.PI, (Mathf.Abs(slideAngle.y) * 180) / Mathf.PI, (Mathf.Abs(slideAngle.z) * 180) / Mathf.PI);//rad to deg? seems to work

            if (onDrugs)
            {
                if (Input.GetButton("Jump") && stableFooting)//jump
                {
                    // print("jump");
                    moveDirection.y = jumpSpeed;
                    stableFooting = false;
                }
                //moveDirection.y = gravity * Time.deltaTime;
            }
            else if(sy+5> controller.slopeLimit)//fall
            {
                
                moveDirection = moveDirection - (slideAngle * (gravity / 7)) + (reflectVec*1);//slide down with a bit of pep
                stableFooting = false;
            }
            else if(Input.GetButton("Jump")&& stableFooting)//jump
            {
                // print("jump");
                
                moveDirection.y = jumpSpeed= jumpSpeedGoal;
                stableFooting = false;
            }
            else if(DJtimer + .1f < Time.time && jumpRelease == true)//
            {
                jumpRelease = false;
                DJtimer = Time.time;
                stableFooting = true;
            }
            
        }
        else if (stableFooting ==false || controller.isGrounded ==false || IsGrounded(transform) == false)
        {
            movments.fall(moveDirection);// stableFooting = false;
        }
        if (hight > transform.position.y + 1)
        {
            hight = transform.position.y;
            falling = true;
            movments.fall(new Vector3(0, hight- transform.position.y,0));// stableFooting = false;

        }
        else if(falling)
        {
            falling = false;
            movments.walk(new Vector3(0, 0, 0));
        }
        if (Time.frameCount % 150 == 0 & !isOn)
        {
           // ulong lengh = player.frameCount;
           // long current = player.frame;
           // StartCoroutine(setVal(sy*2));
            //StartCoroutine(Upload());
            // StartCoroutine(setVal(0));//(int)((long)current/(long)lengh)));
        }
        if (onDrugs==false)
        {
            moveDirection.y -= (gravity * Time.deltaTime);
        }
        else
        {
        }
        controller.Move(moveDirection * Time.deltaTime);
        
    }

       
  

    IEnumerator Upload()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

        UnityWebRequest www = UnityWebRequest.Post("http://www.my-server.com/myform", formData);
        yield return www.Send();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
    IEnumerator setVal(float data)
    {
        isOn = true;
        using (UnityWebRequest www = UnityWebRequest.Get("http://192.168.1.100" + "/?angle="+data  ))// " /?json={\"data\":" + data + "}"))
        {
            yield return www.Send();

            /* if (www.isNetworkError || www.isHttpError)
             {
                 Debug.Log(www.error);
             }
             else
             {
                 // Show results as text
                 string json = www.downloadHandler.text;
                 NewJson myObject = JsonUtility.FromJson<NewJson>(json);
                 float i = (myObject.data - 500) / 200;
                 this.GetComponent<nodeFlow>().TimeScale = i;
                 Debug.Log(i);
                 // Debug.Log(json);
                 // Or retrieve results as binary data
                // player.playbackSpeed = i;
                 byte[] results = www.downloadHandler.data;
             }
             */
            yield return new WaitForSeconds(1);

        }
        isOn = false;
    }
    [System.Serializable]
    public class NewJson
    {
        public float data;
    }
}
