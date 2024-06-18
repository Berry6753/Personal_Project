using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu mainMenu;
    public MainMenu MainMenu { get { return mainMenu; } }


}
