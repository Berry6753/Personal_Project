using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : MonoBehaviour
{
    [SerializeField] private GameObject _questMarkOnWorld;
    [SerializeField] private GameObject _questMarkOnMinimap;
    [SerializeField] private GameObject _minimapIcon;

    private bool _isClearQuest = false;

    public void OnClick_ClearQuest()
    { 
        _isClearQuest = true;
        _questMarkOnWorld.SetActive(false);
        _questMarkOnMinimap.SetActive(false);
        _minimapIcon.SetActive(true);
    }
}
