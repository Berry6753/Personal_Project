using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject LoadMenu;
    [SerializeField] private GameObject OptionMenu;
    [SerializeField] private List<Button> MainUI_Btn;

    private void Awake()
    { 
        FocusBtn(0);
    }

    public void OnClickStartBtn()
    {
        StartMenu.SetActive(true);
        UnInteractableBtn();
    }

    public void OnClickLoadBtn()
    { 
        LoadMenu.SetActive(true);
        UnInteractableBtn();
    }

    public void OnClickOptionBtn()
    {
        OptionMenu.SetActive(true);
        UnInteractableBtn();
    }

    public void OnClickExitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void FocusBtn(int index)
    {
        EventSystem.current.SetSelectedGameObject(MainUI_Btn[index].gameObject);
    }

    public void InteractableBtn()
    { 
        foreach (var btn in MainUI_Btn)
        {
            btn.interactable = true;
        }
    }

    private void UnInteractableBtn()
    {
        foreach (var btn in MainUI_Btn)
        { 
            btn.interactable = false;
        }
    }
}
