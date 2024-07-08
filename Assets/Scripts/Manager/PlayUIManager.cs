using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUIManager : Singleton<PlayUIManager>
{
    [SerializeField] private GameObject saveUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject townNoticeUI;
    [SerializeField] private TMP_Text Text_TownNotice;
    Coroutine co;

    public void TownNotice(string text)
    {
        if (co == null)
            co = StartCoroutine(CoTownNotice(text));
        else
        { 
            StopCoroutine(co);
            co = StartCoroutine(CoTownNotice(text));
    }
    }

    private IEnumerator CoTownNotice(string text)
    {
        townNoticeUI.SetActive(false);
        townNoticeUI.SetActive(true);
        Text_TownNotice.text = text;
        yield return new WaitForSeconds(5.0f);
        townNoticeUI.SetActive(false);
    }
}
