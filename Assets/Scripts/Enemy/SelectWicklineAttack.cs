using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWicklineAttack : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Instance.WicklineAttack();
    }
}
