using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFireSound : StateMachineBehaviour
{
    /// <summary>
    /// On Playback triggers RapidFire Event(true).
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundEventManager.EmitRapidFire(true);
    }

    /// <summary>
    /// On Playback triggers RapidFire Event(false).
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundEventManager.EmitRapidFire(false);
    }
}
