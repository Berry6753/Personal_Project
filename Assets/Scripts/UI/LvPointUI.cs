using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LvPointUI : MonoBehaviour
{
    [SerializeField] private TMP_Text Text_MaxHp;
    [SerializeField] private TMP_Text Text_Damage;
    [SerializeField] private TMP_Text Text_FordDmg;
    [SerializeField] private TMP_Text Text_LvPoint;

    private void Update()
    {
        ConnectMaxHp();
        ConnectPlayerDamage();
        ConnectFordDamage();
        ConnectLevelPoint();

        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            gameObject.SetActive(false);
        }
    }

    private void ConnectMaxHp()
    { 
        Text_MaxHp.text = "�ִ� ü�� : " + GameManager.Instance.PlayerInfo._maxHp.ToString();
    }
    private void ConnectPlayerDamage()
    {
        Text_Damage.text = "�÷��̾� ���ݷ� : " + GameManager.Instance.PlayerInfo._damage.ToString();
    }
    private void ConnectFordDamage() 
    {
        Text_FordDmg.text = "���� ���ݷ� : " + GameManager.Instance.Ford.fordDmg.ToString();
    }
    private void ConnectLevelPoint()
    { 
        Text_LvPoint.text = GameManager.Instance.PlayerInfo._levelPoint.ToString();
    }

    public void OnClick_UpScaleHpBtn()
    {
        GameManager.Instance.PlayerInfo.UpScaleHp();
    }
    public void OnClick_UpScalePlayerDamage()
    { 
        GameManager.Instance.PlayerInfo.UpScaleDmg();
    }
    public void OnClick_UpScaleFordDamage()
    {
        GameManager.Instance.Ford.UpScaleDmg();
    }
}
