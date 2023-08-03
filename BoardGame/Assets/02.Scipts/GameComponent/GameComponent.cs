using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;

public class GameComponent : IGameComponent
{

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.Init:
                Initialize();
                break;
            case GameState.Standby:
                OnStandby();
                break;
            case GameState.Main:
                OnRunning();
                break;
            case GameState.Over:
                OnOver();
                break;
        }
    }


    protected virtual void Initialize()
    {

    }

    protected virtual void OnStandby()
    {

    }

    protected virtual void OnRunning()
    {

    }

    protected virtual void OnOver()
    {

    }

    public void OnDisable()
    {

    }
}
