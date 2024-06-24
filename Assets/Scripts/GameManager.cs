using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerMove player;
    public PlayerMove Player {  get { return player; } }

    public void ChangeState()
    {
        player.ChangeStateAfterMove();
    }
}
