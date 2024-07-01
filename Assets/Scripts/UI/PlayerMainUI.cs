using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMainUI : MonoBehaviour
{
    [SerializeField] private Slider Slider_Hp;
    [SerializeField] private Slider Slider_Exp;
    [SerializeField] private TMP_Text Text_Level;

    private void Start()
    {
        Slider_Hp.value = GameManager.Instance.CurruntHp();
        Slider_Exp.value = GameManager.Instance.CurruntExp();
    }

    private void Update()
    {
        LerpHp();
        LerpExp();
        LevelText();
    }

    private void LerpHp()
    {
        Slider_Hp.value = Mathf.Lerp(Slider_Hp.value, GameManager.Instance.CurruntHp(), Time.deltaTime * 10);
    }
    private void LerpExp()
    {
        Slider_Exp.value = Mathf.Lerp(Slider_Exp.value, GameManager.Instance.CurruntExp(), Time.deltaTime * 10);
    }
    private void LevelText()
    {
        Text_Level.text = $"Lv : {GameManager.Instance.CurrentLevel()}";
    }
}
