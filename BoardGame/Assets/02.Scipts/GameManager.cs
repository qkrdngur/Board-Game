using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    GameState State;
    private readonly List<IGameComponent> _components = new();

    private void Awake()
    {
        instance = this;

        _components.Add(new SpawnBlock(this));
    }

    void Start()
    {
        UpdateState(GameState.Init);
    }

    public void UpdateState(GameState state)
    {
        State = state;

        foreach (var component in _components)
            component.UpdateState(state);

        if (state == GameState.Init)
            UpdateState(GameState.Standby);
    }

    void Update()
    {
        Debug.Log(State);
    }
}
