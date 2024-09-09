using BoardGame.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public readonly int GRADE = 12;

    public BlockName blockSO;
    public PlayerSO playerSO;

    [SerializeField] private List<Material> materials;

    public List<Transform> blockPos;

    public PlayTurn pTurn                                { get; private set; }
    public GameState State                               { get; private set; }
    public CurTower tower                                { get; set; }
    public CurTower removeTower                          { get; set; }

    public Dictionary<PlayTurn, GameObject> player       { get; set; } = new();
    public Dictionary<PlayTurn, int> curBlock            { get; set; } = new();
    public Dictionary<PlayTurn, int> money               { get; set; } = new();
    public Dictionary<PlayTurn, bool> diePlayer          { get; set; } = new();
    public Dictionary<int, PlayMoney> buildingOwner      { get; set; } = new();
    public Dictionary<int, CurTower> buildCount          { get; set; } = new();
    public Dictionary<int, int> buildingPrice            { get; set; } = new();
    public Dictionary<int, int> built                    { get; set; } = new();
    public Dictionary<int, List<GameObject>> curTower    { get; set; } = new();

    public int jumpCount                                 { get; set; }
    public bool isChangeColor                            { get; set; }

    private readonly List<IGameComponent> _components = new();
    private bool _buyBuilding = false;

    void FirstPlayer()
    {

        //���⿡ ó�� ���� �� �÷��̾� ���ϴ°� �ϱ�(�̴���ϸ� �ȵ�)
        int firstPlayer = UnityEngine.Random.Range(0, Enum.GetValues(typeof(PlayTurn)).Length);
        pTurn = (PlayTurn)firstPlayer;
    }

    void Awake()
    {
        _components.Add(new SpawnBlock(this));
        _components.Add(new PlayerMovement(this));
        _components.Add(new Dice(this));
        _components.Add(new Select(this));
        _components.Add(new Build(this));
        _components.Add(new GameTurn(this));
        _components.Add(new ResetValue(this));
    }

    void Start()
    {
        UpdateState(GameState.Init);
    }

    void Update()
    {
        if (State == GameState.Main) 
            _components.ForEach(component => component.OnRoutine());
    }

    public void UpdateState(GameState state)
    {
        State = state;
        _components.ForEach(component => component.UpdateState(state));
    }

    public void NextTurn()
    {
        UiManager.Instance.EnableOutLine(pTurn);
        int next = (int)++pTurn % 4;
        
        pTurn = (PlayTurn)next;

        for(int i = 0; i < Enum.GetValues(typeof(PlayTurn)).Length; i++)
        {
            if (diePlayer[pTurn]) pTurn = (PlayTurn)((next + i) % 4);
            else break;
        }
    }

    #region Build
    public void BuildFunc()
    {
        buildCount[curBlock[pTurn]] = tower;

        CalcPrice();
        BuildTower();
        BuyBuilding();
    }

    private void BuyBuilding()
    {
        if (_buyBuilding)
        {
            buildingOwner[curBlock[pTurn]] = (PlayMoney)pTurn;

            _buyBuilding = false; // �ǹ��� �������� �ʾ��� ��
        }

        isChangeColor = false;
    }

    private void BuildTower()
    {
        for (int i = 0; i < (int)tower; i++)
        {
            if (i < built[curBlock[pTurn]])
            {
                if (isChangeColor)
                    curTower[curBlock[pTurn]][i].GetComponent<MeshRenderer>().material = materials[(int)pTurn];

                continue;
            }

            GameObject obj = ObjectPool.instance.GetObject(PoolObjectType.build1 + i);
            curTower[curBlock[pTurn]].Add(obj);

            obj.GetComponent<MeshRenderer>().material = materials[(int)pTurn];
            obj.transform.position = BuildingPos(i).position;
        }

        isChangeColor = true;
        built[curBlock[pTurn]] = (int)buildCount[curBlock[pTurn]];
    }

    private Transform BuildingPos(int idx)
    {
        Transform child = blockPos[curBlock[pTurn]].GetChild(1);
        return child.GetChild(idx);
    }
    #endregion

    #region Price
    public void CalcPrice()
    {
        foreach (PlayTurn play in Enum.GetValues(typeof(PlayTurn)))
        {
            PlayTurn curTurn = play;

            int price = buildingPrice[curBlock[pTurn]];
            int payPrice = price * (int)tower;

            if (payPrice == 0 || (pTurn == curTurn)) continue;

            if (buildingOwner[curBlock[pTurn]] == PlayMoney.None)
            {
                _buyBuilding = true;
                money[pTurn] -= payPrice;

                break;
            }
            else
            {
                DicMoney(curTurn, payPrice);
            }
        }
    }

    public void DicMoney(PlayTurn turn, int price)
    {
        if (buildingOwner[curBlock[pTurn]] != (PlayMoney)turn) return;

        if (turn != pTurn && built[curBlock[pTurn]] != 0)
        {
            print(buildingOwner[curBlock[pTurn]]);
            _buyBuilding = true; // �ǹ��� �����Ͽ��� ���� owner�� �ǰ�
            money[turn] += price;
        }
        //���Ⱑ �̻��մϴ�. Ȯ���� �ؾ��� ���� 2��� �ȳ��� �̰Ÿ� �ذ��ϸ���� ��

        if (UiManager.Instance.isBuyBuilding) // ���߿� ui�� ������ ����Ʈ�ϰ� �̰����� �ϱ�
        {
            price *= 2;
            ChangeColor();
        }

        money[pTurn] -= price;
    }

    private void ChangeColor()
    {
        for (int i = 0; i < (int)tower; i++)
        {
            curTower[curBlock[pTurn]][i].GetComponent<MeshRenderer>().material = materials[(int)pTurn];
        }
    }
    #endregion
}
