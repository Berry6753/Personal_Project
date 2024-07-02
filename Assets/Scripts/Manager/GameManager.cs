using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("플레이어 정보")]
    [SerializeField] private PlayerInfo playerInfo;
    public PlayerInfo PlayerInfo { get { return playerInfo; } }

    [Header("플레이어 움직임 정보")]
    [SerializeField] private PlayerMove player;
    public PlayerMove Player { get { return player; } }

    [Header("포드 정보")]
    [SerializeField] private FordController ford;
    public FordController Ford { get { return ford; } }

    [Header("위클라인")]
    [SerializeField] private WicklineController wickline;
    public WicklineController Wickline { get { return wickline; } }

    [Header("알파 오메가")]
    [SerializeField] private AlphaOmegaController ao;
    public AlphaOmegaController AO { get { return ao; } }

    private bool isGameOnTime = true;

    public void ChangeState()
    {
        if (player == null) return;

        player.ChangeStateAfterMove();
    }

    public void ShootBullet()
    {
        if( ford == null) return;

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
    public int CurrentLevel()
    {
        return playerInfo._level;
    }

    public void PlayerGetExp(float get)
    {
        if (playerInfo == null) return;
        playerInfo.GetExp(get);
    }

    public void WicklineAttack()
    { 
        wickline.OnAttackThink();
    }
    public void AOAttack()
    {
        ao.OnAttackThink();
    }

    public void PlayerHurt(float dmg)
    {
        playerInfo.Hurt(dmg);
    }
    public void WicklineHurt(float dmg)
    {
        wickline.Hurt(dmg);
    }
    public void AOHurt(float dmg)
    {
        ao.Hurt(dmg);
    }
}
