using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;

public class GameManager : MonoSingleton<GameManager>
{
    public PlayTurn pTurn;
    public GameState State;
    public BlockName blockSO;

    [HideInInspector] public List<Transform> blockPos;
    [HideInInspector] public List<PlayTurn> blockRot;

    public Dictionary<PlayTurn, GameObject> player = new Dictionary<PlayTurn, GameObject>();
    public Dictionary<PlayTurn, int> money = new Dictionary<PlayTurn, int>();

    private readonly List<IGameComponent> _components = new();

    public readonly int GRADE = 12;
    public int jumpCount;

    private void Awake()
    {
        _components.Add(new SpawnBlock(this));
        _components.Add(new PlayerMovement(this));
        _components.Add(new Dice(this));
        _components.Add(new Build(this));
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
    }

    public void NextTurn(PlayTurn turn)
    {
        int next = (int)++turn % 4;

        pTurn = (PlayTurn)next;
    }

    void Update()
    {
        if (State == GameState.Main)
            foreach (var component in _components)
                component.OnRoutine();
    }
}
