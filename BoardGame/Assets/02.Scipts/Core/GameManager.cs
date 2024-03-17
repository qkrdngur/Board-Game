using BoardGame.Util;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public CurTower tower;
    public PlayTurn pTurn;
    public GameState State;
    public BlockName blockSO;

    [HideInInspector] public List<Transform> blockPos;
    [HideInInspector] public List<PlayTurn> blockRot;

    public List<GameObject> building;
    public Dictionary<int, CurTower> buildCount = new Dictionary<int, CurTower>();
    public Dictionary<PlayTurn, GameObject> player = new Dictionary<PlayTurn, GameObject>();
    public Dictionary<int, PlayTurn> BuildingOwner = new Dictionary<int, PlayTurn>();
    public Dictionary<PlayTurn, int> curBlock = new Dictionary<PlayTurn, int>();
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

    void Update()
    {
        if (State == GameState.Main)
            foreach (var component in _components)
                component.OnRoutine();
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

    public void Build()
    {
        GameObject obj = ObjectPool.instance.GetObject(PoolObjectType.Build1);
        Debug.Log(curBlock[PlayTurn.player]);
        obj.transform.position = blockPos[curBlock[pTurn]].position;

        BuildingOwner[curBlock[pTurn]] = pTurn;

        Transform child = blockPos[curBlock[pTurn]].GetChild(0);
        child.GetComponent<TextMeshPro>().text = "ssss";

        buildCount[curBlock[pTurn]] = CurTower.tower01;
    }
}
