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

    public int _level;
    private int _skillPoint;

    private void Awake()
    {
        _hp = _maxHp;
        _exp = _minExp;
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
            Die();
        }
    }

    private void LevelUp()
    {
        _level++;
        _skillPoint += 5;
        _hp = _maxHp;
        _exp = _minExp;
    }

    private void UpScaleExp()
    {
        _maxExp *= 1.2f;
    }
    public void UpScaleHp()
    {
        _skillPoint--;
        _maxHp += 20f;
    }
    public void UpScaleDmg()
    {
        _skillPoint--;
        _damage += 20f;
    }
    public void GetExp(float get)
    {
        _exp += get;
    }
    public void Hurt(float dmg)
    {
        if (GameManager.Instance.Player._gaurd != 0) return;

        _hp -= dmg;
    }
    private void Die()
    {
        onDie?.Invoke(this);
    }
}
