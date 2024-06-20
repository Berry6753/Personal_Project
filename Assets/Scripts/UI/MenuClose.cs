using System.Collections;
using UnityEngine;

public class MenuClose : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();    
    }

    public void OnClickCloseBtn()
    {
        StartCoroutine(CoClose());
    }

    private IEnumerator CoClose()
    {
        anim.SetTrigger("Close");
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
        anim.ResetTrigger("Close");
    }
}
