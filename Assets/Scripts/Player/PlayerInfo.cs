using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public Action<PlayerInfo> onDie;

    public float _maxHp;
    public float _hp;
    public float _maxExp;
    private float _minExp;
    public float _exp;
    public float _damage;
    public float _skillDamage;

    public int _level;
    public int _levelPoint;

    private void Awake()
    {
        _maxHp = 300.0f;       
        _maxExp = 100.0f;
        _minExp = 0f;
        _exp = _minExp;
        _damage = 50.0f;
        _skillDamage = _damage * 1.5f;
        _level = 1; 
        _levelPoint = 0;
    }

    private void OnEnable()
    {
        _hp = _maxHp;
    }

    private void Update()
    {
        if (_exp >= _maxExp)
        {
            LevelUp();
            UpScaleExp();
        }
        if (_hp <= 0)
        {
            if (GameManager.Instance.Player._isDie) return;

            Die();
        }
    }

    private void LevelUp()
    {
        _level++;
        _levelPoint += 5;
        _hp = _maxHp;
        _exp -= _maxExp;
    }

    private void UpScaleExp()
    {
        _maxExp += 20f;
    }
    public void UpScaleHp()
    {
        if (_levelPoint <= 0) return;

        _levelPoint--;
        _maxHp += 20f;
        _hp += 20f;
    }
    public void UpScaleDmg()
    {
        if (_levelPoint <= 0) return;

        _levelPoint--;
        _damage += 10f;
        _skillDamage = _damage * 1.5f;
    }
    public void GetExp(float get)
    {
        _exp += get;
    }
    public void Hurt(float dmg)
    {
        if (GameManager.Instance.Player._gaurd != 0) return;

        _hp -= dmg;

        if (_hp / _maxHp < 0.3f)
        {
            PlayUIManager.Instance.PlayerHurt();

            if (_hp <= 0) return;

            PlayUIManager.Instance.DisableHurt();
        }
    }
    private void Die()
    {
        onDie?.Invoke(this);
        PlayUIManager.Instance.PlayerDie();
    }
}
