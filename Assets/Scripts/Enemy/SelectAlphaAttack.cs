using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAlphaAttack : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Instance.AlphaAttack();
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Instance.Alpha.isAttack = false;
    }
}
