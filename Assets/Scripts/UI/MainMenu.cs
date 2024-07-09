using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject LoadMenu;
    [SerializeField] private GameObject OptionMenu;
    [SerializeField] private GameObject Info;
    [SerializeField] private List<Button> MainUI_Btn;

    public void OnClick_StartBtn()
    {
        SceneManager.LoadScene("PlayScene");
        //StartMenu.SetActive(true);
        //Info.SetActive(true);
        //Info.GetComponentInChildren<TMP_Text>().text = "Please select a location to save the new game";
        //UnInteractableBtn();
    }

    public void OnClick_LoadBtn()
    { 
        LoadMenu.SetActive(true);
        Info.SetActive(true);
        Info.GetComponentInChildren<TMP_Text>().text = "Please select a game to load";
        UnInteractableBtn();
    }

    public void OnClick_OptionBtn()
    {
        OptionMenu.SetActive(true);
        Info.SetActive(true);
        Info.GetComponentInChildren<TMP_Text>().text = "Change settings";
        UnInteractableBtn();
    }

    public void OnClick_ExitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Quit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (StartMenu.activeSelf)
            {
                DisableInfoUI();
                DisableStartUI();
                InteractableBtn();
                FocusBtn(0);
            }
            else if (LoadMenu.activeSelf)
            {
                DisableInfoUI();
                DisableLoadUI();
                InteractableBtn();
                FocusBtn(1);
            }
            else if (OptionMenu.activeSelf)
            {
                OptionMenu.GetComponent<OptionMenu>().OptionQuit();
            }
        }
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

    private void DisableUI(GameObject gObj)
    {
        gObj.GetComponent<MenuClose>().OnClickCloseBtn();
    }

    public void DisableInfoUI()
    {
        DisableUI(Info);
    }

    public void DisableStartUI()
    {
        DisableUI(StartMenu);
    }

    public void DisableLoadUI()
    {
        DisableUI(LoadMenu);
    }

    public void DisableOptionUI()
    {
        DisableUI(OptionMenu);
    }
}
