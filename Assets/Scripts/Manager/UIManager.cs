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

    private void Awake()
    {
        FocusBtn(0);
    }

    private void Update()
    {
        MainUI.Quit();
    }

    public void FocusBtn(int index)
    { 
        MainUI.FocusBtn(index);
    }

    public void InteractableBtn()
    {
        MainUI.InteractableBtn();
    }

    public void DisableInfo()
    { 
        MainUI.DisableInfoUI();
    }

    //public void DisableStart()
    //{
    //    MainUI.DisableStartUI();
    //}
    //public void DisableLoad()
    //{
    //    MainUI.DisableLoadUI();
    //}
    public void DisableOption()
    {
        MainUI.DisableOptionUI();
    }
}
