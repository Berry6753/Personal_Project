using System;
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

    [Header("벽콜라이더")]
    [SerializeField] private BoxCollider wall;
    public BoxCollider Wall { get { return wall; } }

    private MonsterController monster;

    private bool isGameOnTime;

    private void Awake()
    {
        isGameOnTime = true;
        Cursor.lockState = CursorLockMode.Locked;
        wickline.disable += HandlerWicklineDisable;
        alpha.disable += HandlerAlphaDisable;
        omega.disable += HandlerOmegaDisable;
        playerInfo.onDie += HandlerPlayerDie;
    }
    private void HandlerAlphaDisable(AlphaOmegaController controller)
    {
        Invoke(nameof(SetActiveAlpha), 25f);
    }
    private void HandlerOmegaDisable(AlphaOmegaController controller)
    {
        Invoke(nameof(SetActiveOmega), 25f);
    }
    private void HandlerWicklineDisable(WicklineController controller)
    {
        Invoke(nameof(SetActiveWickline), 25f);
    }
    private void HandlerPlayerDie(PlayerInfo info)
    {
        Time.timeScale = 0.2f;
        Invoke(nameof(ResetPlayer), 2f);
    }
    public void HandlerMonsterDisable(MonsterController monster)
    {
        StartCoroutine(SetActiveMonster(monster, 10f));
    }

    private void SetActiveAlpha()
    { 
        alpha.gameObject.SetActive(true);
    }
    private void SetActiveOmega()
    {
        omega.gameObject.SetActive(true);
    }
    private void SetActiveWickline()
    {
        wickline.gameObject.SetActive(true);
    }
    private IEnumerator SetActiveMonster(MonsterController monster, float time)
    {
        yield return new WaitForSeconds(time);
        monster.gameObject.SetActive(true);
    }
    private void ResetPlayer()
    {
        Time.timeScale = 1.0f;
        PlayUIManager.Instance.DisablePlayerDie();
        player.gameObject.SetActive(false);
        player.gameObject.SetActive(true);
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

    public void SetIsTriggerWall()
    {
        wall.isTrigger = true;
        ParticleManager.Instance.OnStopWallParticle();
    }
}
