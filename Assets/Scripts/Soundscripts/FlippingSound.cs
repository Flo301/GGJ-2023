using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlippingSound : StateMachineBehaviour
{
    /// <summary>
    /// On Playback triggers Flipping Event.
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundEventManager.EmitFlipping();
    }
}
