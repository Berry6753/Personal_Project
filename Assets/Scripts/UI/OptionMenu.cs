using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] private Button Sound_Btn;
    [SerializeField] private Button Resolution_Btn;
    [SerializeField] private GameObject SoundOption;
    [SerializeField] private GameObject ResolutionOption;

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
                gameObject.SetActive(false);
                UIManager.Instance.MainMenu.InteractableBtn();
                UIManager.Instance.MainMenu.FocusBtn(2);
            }
        }
    }

    public void OnClickSoundBtn()
    { 
        Resolution_Btn.interactable = false;
        SoundOption.SetActive(true);
    }

    public void OnClickResolution_Btn()
    {
        Sound_Btn.interactable = false;
        Resolution_Btn.interactable = false;
        ResolutionOption.SetActive(true);
    }
}
