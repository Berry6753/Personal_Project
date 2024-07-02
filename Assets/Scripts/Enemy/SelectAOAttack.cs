using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAOAttack : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Instance.AOAttack();
    }
}
