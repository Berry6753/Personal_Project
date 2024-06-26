using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider Slider_MV;
    [SerializeField] private Slider Slider_BGM;
    [SerializeField] private Slider Slider_SFX;

    private void Start()
    {
        Slider_MV.onValueChanged.AddListener(SetMVVolume);
        Slider_BGM.onValueChanged.AddListener(SetBGMVolume);
        Slider_SFX.onValueChanged.AddListener(SetSFXVolume);
    }

    private void SetMVVolume(float volume)
    {
        _audioMixer.SetFloat("MV", Mathf.Log10(volume) * 20);
    }

    private void SetBGMVolume(float volume)
    {
        _audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    private void SetSFXVolume(float volume)
    {
        _audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
