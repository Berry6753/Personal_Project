using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("�÷��̾� ����")]
    [SerializeField] private PlayerInfo playerInfo;
    public PlayerInfo PlayerInfo { get { return playerInfo; } }

    [Header("�÷��̾� ������ ����")]
    [SerializeField] private PlayerMove player;
    public PlayerMove Player { get { return player; } }

    [Header("���� ����")]
    [SerializeField] private FordController ford;
    public FordController Ford { get { return ford; } }

    private bool isGameOnTime = true;

    public void ChangeState()
    {
        player.ChangeStateAfterMove();
    }

    public void ShootBullet()
    {
        ford.OnAttack();
    }

    public void PauseGame()
    {
        if (isGameOnTime)
        {
            isGameOnTime = false;
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
            isGameOnTime = true;
        }
    }

    public float CurruntHp()
    {
        return playerInfo._hp / playerInfo._maxHp;
    }
    public float CurruntExp()
    {
        return playerInfo._exp / playerInfo._maxExp;
    }
}
