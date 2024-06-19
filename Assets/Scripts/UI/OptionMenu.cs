using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] private Button Sound_Btn;
    [SerializeField] private Button Resolution_Btn;
    [SerializeField] private GameObject SoundOption;
    [SerializeField] private GameObject ResolutionOption;
    [SerializeField] private MenuClose close;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(Sound_Btn.gameObject);   
    }

    private void Update()
    {
        Quit();
    }

    private void Quit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SoundOption.activeSelf)
            {
                SoundOption.SetActive(false);
                Resolution_Btn.interactable = true;
                Sound_Btn.interactable = true;
                EventSystem.current.SetSelectedGameObject(Sound_Btn.gameObject);
            }
            else if (ResolutionOption.activeSelf)
            {
                ResolutionOption.SetActive(false);
                Resolution_Btn.interactable = true;
                Sound_Btn.interactable = true;
                EventSystem.current.SetSelectedGameObject(Resolution_Btn.gameObject);
            }
            else
            {
                close.OnClick_CloseBtn();
                UIManager.Instance.DisableInfo();
                UIManager.Instance.InteractableBtn();
                UIManager.Instance.FocusBtn(2);
            }
        }
    }

    public void OnClick_SoundBtn()
    { 
        Resolution_Btn.interactable = false;
        SoundOption.SetActive(true);
    }

    public void OnClick_ResolutionBtn()
    {
        Sound_Btn.interactable = false;
        ResolutionOption.SetActive(true);
    }
}
