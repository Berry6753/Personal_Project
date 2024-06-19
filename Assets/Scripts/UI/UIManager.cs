using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public enum UIType
{ 
    MainUI,
    InfoPopup
}

public class UIManager : Singleton<UIManager>
{
    //[SerializeField] private MainMenu mainMenu;
    //public MainMenu MainMenu { get { return mainMenu; } }

    [SerializeField] private MainMenu MainUI;

    public void FocusBtn(int index)
    { 
        MainUI.FocusBtn(index);
    }

    public void InteractableBtn()
    {
        MainUI.InteractableBtn();
    }
}
