using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class falling_to_idle : MonoBehaviour {
    Animator anim;
    Vector3 move_old;
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

        public analock(Animator anim) {
            this.anim = anim;
            falling = false;
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

    }
}
