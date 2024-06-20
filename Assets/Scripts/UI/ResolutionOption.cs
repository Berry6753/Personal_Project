using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionOption : MonoBehaviour
{
    [SerializeField] private Toggle Toggle_FullScreen;
    private FullScreenMode _screenMode;

    private List<Resolution> resolutionList = new List<Resolution>();
    [SerializeField] private TMP_Dropdown Dropdown_Resolution;
    private int _resolutionIndex;

    private void Start()
    {
        InitResolution();
    }

    private void InitResolution()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate >= 50)
            {
                resolutionList.Add(Screen.resolutions[i]);
            }
        }
        //resolutionList.AddRange(Screen.resolutions);
        Dropdown_Resolution.options.Clear();

        int resolutionIndex = 0;
        foreach (Resolution item in resolutionList)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            Dropdown_Resolution.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
                Dropdown_Resolution.value = resolutionIndex;

            resolutionIndex++;
        }
        Dropdown_Resolution.RefreshShownValue();

        Toggle_FullScreen.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void OnValueChanged_Resolution(int index)
    {
        _resolutionIndex = index;
        Screen.SetResolution(resolutionList[_resolutionIndex].width, resolutionList[_resolutionIndex].height, _screenMode);
    }

    public void OnValueChanged_FullScreenToggle(bool isFullScreen)
    {
        _screenMode = isFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
}
