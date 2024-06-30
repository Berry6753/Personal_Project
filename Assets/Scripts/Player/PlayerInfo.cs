using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public float _maxHp;
    public float _hp;
    public float _maxExp;
    private float _minExp;
    public float _exp;
    public float _damage;

    private int _level;
    private int _levelPoint;

    private void Awake()
    {
        _hp = _maxExp;
        _exp = _minExp;
    }

    private void Update()
    {
        if (_exp >= _maxExp)
        {
            LevelUp();
            UpScaleExp();
        }
    }

    private void LevelUp()
    {
        _level++;
        _levelPoint += 5;
        _hp = _maxHp;
        _exp = _minExp;
    }

    private void UpScaleExp()
    {
        _maxExp *= 1.2f;
    }
    public void UpScaleHp()
    {
        _maxHp += 20f;
    }
    public void UpScaleDmg()
    {
        _damage += 20f;
    }
}
