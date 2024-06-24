using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerMove player;
    public PlayerMove Player { get { return player; } }
    [SerializeField] private FordController ford;
    public FordController Ford { get { return ford; } }

    private bool isGameOnTime = true;

    public void ChangeState()
    {
        player.ChangeStateAfterMove();
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
}
