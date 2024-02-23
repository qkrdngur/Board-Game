using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;

public class GameManager : MonoSingleton<GameManager>
{
    public GameState State;

    [HideInInspector] public List<Transform> blockPos;
    public Dictionary<string, GameObject> player = new Dictionary<string, GameObject>();

    public readonly int GRADE = 12;
    public int jumpCount;

    private readonly List<IGameComponent> _components = new();

    private void Awake()
    {
        _components.Add(new SpawnBlock(this));
        _components.Add(new PlayerMovement(this));
        _components.Add(new Dice(this));
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
        if (State == GameState.Main)
            foreach (var component in _components)
                component.OnRoutine();

    }
}
