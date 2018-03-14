using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class falling_to_idle : MonoBehaviour {
    Animator anim;
    Vector3 move_old;
    analock pramGate;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
    void Start()
    {
        anim = GetComponent<Animator>();
        pramGate = new analock(this, anim);
        pramGate.loadToParent();
    }
    void Update()
    {
        Vector3 move = transform.position - move_old;
        move_old = transform.position;
        
       // anim.SetFloat("down speed", move.y);
       //       anim.SetBool("falling", true);
       //       anim.SetBool("idleing", true);
       //       anim.SetBool("walking", false);

    }
    class analock
    {
        Animator anim;
        bool _falling, _stubble, _idleing, _walking, _sitting = false;
        falling_to_idle inputPipe;
        public analock(falling_to_idle _alt,Animator anim) {
            this.anim = anim;
            inputPipe = _alt;
        }
        /// <summary>
        /// because of this function 
        /// </summary>
        public void loadToParent()
        {
            locomove interfacer =inputPipe.transform.parent.GetComponent<locomove>();
            interfacer.movments.setwalker(new aniamation(anim));

        }
        public class aniamation : locomove.Actuation.IWalkable
        {
            Animator anim;
            public aniamation(Animator anim)
            {
                this.anim = anim;

            }
            bool _falling, _stubble, _idleing, _walking, _sitting = false;

            public void walk(Vector3 location)
            {
                print(location.magnitude);
                if (location.magnitude > .2f)
                {
                    walking = true;
                    idleing = false;
                    falling = false;
                    move(location);
                }
                else
                {
                    walking = false;
                    idleing = true;
                    move(new Vector3(0,0,0));
                }
            }
            public void stumble()
            {
                stubble = true;
            }
            public void fall(Vector3 displace)
            {
                if (displace.y < -.3)
                {
                    falling = true;
                    idleing = false;
                    walking = false;
                }

            }
    



            public bool falling
        {
            get
            {
                return this.anim.GetBool("falling");

            }
            set
            {
                this.anim.SetBool("falling",value);
                _falling = value;
            }
        }
        public bool stubble
        {
            get
            {
                return this.anim.GetBool("stubble");

            }
            set
            {
                this.anim.SetBool("stubble", value);
                _stubble = value;
            }
        }

        public bool idleing
        {
            get
            {
                return this.anim.GetBool("idleing");

            }
            set
            {
                this.anim.SetBool("idleing", value);
                _idleing = value;
            }
        }
        public bool walking
        {
            get
            {
                return this.anim.GetBool("walking");

            }
            set
            {
                this.anim.SetBool("walking", value);
                if (value == true)
                {

                    this.anim.SetBool("idleing", false);
                }
                _walking = value;
            }
        }

            public bool sitting
            {
                get
                {
                    return this.anim.GetBool("sitting");

                }
                set
                {
                    this.anim.SetBool("sitting", value);
                    _sitting = value;
                }
            }
            public void move(Vector3 loco) 
            {

                this.anim.SetFloat("walkingForward", loco.x);
                this.anim.SetFloat("walkingLeft", loco.z);



            }

        }

    }
}
