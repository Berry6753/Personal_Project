using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _slider;
    [SerializeField] private Sprite Sprite_isOn;
    [SerializeField] private Sprite SPrite_isOff;
    private Toggle Toggle_mute;
    private Image Img_Target;

    private void Start()
    {
        Toggle_mute = GetComponent<Toggle>();
        Img_Target = GetComponent<Image>();

        Toggle_mute.onValueChanged.AddListener(OnValueChangedToggle);
    }

    private void OnValueChangedToggle(bool isOn)
    { 
        UpdateSprite(isOn);
    }

    private void UpdateSprite(bool isOn)
    {
        if (isOn)
            Img_Target.sprite = Sprite_isOn;
        else
            Img_Target.sprite = SPrite_isOff;
    }

    private void UpdateOnOffVolume(bool isOn, string mixerName)
    {
        if (isOn)
            _audioMixer.SetFloat(mixerName, Mathf.Log10(_slider.value) * 20);
        else
            _audioMixer.SetFloat(mixerName, -80); ;
    }

    public void OnValueChanged_UpdateOnOffMV(bool isOn)
    {
        UpdateOnOffVolume(isOn, "MV");
    }
    public void OnValueChanged_UpdateOnOffBGM(bool isOn)
    {
        UpdateOnOffVolume(isOn, "BGM");
    }
    public void OnValueChanged_UpdateOnOffSFX(bool isOn)
    {
        UpdateOnOffVolume(isOn, "SFX");
    }
}
