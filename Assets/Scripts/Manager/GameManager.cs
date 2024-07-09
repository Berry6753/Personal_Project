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

    [Header("알파")]
    [SerializeField] private AlphaOmegaController alpha;
    public AlphaOmegaController Alpha { get { return alpha; } }

    [Header("오메가")]
    [SerializeField] private AlphaOmegaController omega;
    public AlphaOmegaController Omega { get { return omega; } }

    private bool isGameOnTime;

    private void Awake()
    {
        isGameOnTime = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

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
            Cursor.lockState = CursorLockMode.None;
            isGameOnTime = false;
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
            isGameOnTime = true;
            Cursor.lockState = CursorLockMode.Locked;
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
    public void AlphaAttack()
    {
        alpha.OnAttackThink();
    }
    public void OmegaAttack()
    {
        omega.OnAttackThink();
    }

    public void PlayerHurt(float dmg)
    {
        playerInfo.Hurt(dmg);
    }
    public void WicklineHurt(float dmg)
    {
        wickline.Hurt(dmg);
    }
    public void AlphaHurt(float dmg)
    {
        alpha.Hurt(dmg);
    }
    public void OmegaHurt(float dmg)
    {
        omega.Hurt(dmg);
    }
}
