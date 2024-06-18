using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject LoadMenu;
    [SerializeField] private GameObject OptionMenu;

    public void OnClickStartBtn()
    { 
        StartMenu.SetActive(true);
    }

    public void OnClickLoadBtn()
    { 
        LoadMenu.SetActive(true);
    }

    public void OnClickOptionBtn()
    { 
        OptionMenu.SetActive(true);
    }

    public void OnClickExitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
