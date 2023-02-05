using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stepping : StateMachineBehaviour
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("end stepping");
        SoundEventManager.EmitStepping(0);
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(stateInfo.length);
        
        Debug.Log("start sepping");
        SoundEventManager.EmitStepping((stateInfo.length/stateInfo.speed)/2);
    }
}
