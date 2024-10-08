using BoardGame.Util;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoSingleton<UiManager>
{
    DiceGague dice;
    GameManager manager;

    [SerializeField] private GameObject[] towerImg;
    [SerializeField] private GameObject playerInfoUI;
    [SerializeField] private GameObject chooseBtn;
    [SerializeField] private GameObject cancelBtn;
    [SerializeField] private GameObject payBtn;
    [SerializeField] private GameObject DiceGage;
    [SerializeField] private RectTransform buildUi;
    [SerializeField] private TextMeshProUGUI btnNameText;

    [Space(20)]
    public Transform dicePoint;
    public List<RectTransform> playerUI;

    public bool isBuyBuilding               { get; set; } = false;
    public bool isSpin                      { get; set; } = false;
    public float grade                      { get; private set; }

    private CurTower saveTower = CurTower.none;
    private GameState gameState = GameState.Build;

    private int towerNum;
    private string[] btnName = { "Cancel", "Pay" };

    private void Awake()
    {
        manager = GameManager.Instance;
        dice = GetComponent<DiceGague>();
    }

    private void BuyBuilding(bool value)
    {
        isBuyBuilding = false;

        if (value)
        {
            UndoUI();
        }
        else
        {
            for (int i = 0; i < towerImg.Length; i++)
                towerImg[i].gameObject.SetActive(false);

            cancelBtn.GetComponent<Button>().onClick.AddListener(UndoCancelBtn);
            payBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                payBtn.SetActive(false);
                manager.DicMoney(manager.pTurn,
                    manager.buildingPrice[manager.curBlock[manager.pTurn]] * (int)manager.tower);
            });

            manager.BuildFunc();

            payBtn.SetActive(true);
        }
    }
    private void UndoCancelBtn()
    {
        UndoUI();
        payBtn.SetActive(false);

        cancelBtn.GetComponent<Button>().onClick.RemoveListener(UndoCancelBtn);
    }
    private void SelectBuild()
    {
        int buildMoney = manager.buildingPrice[manager.curBlock[manager.pTurn]];
        int money = manager.money[manager.pTurn];

        for (int i = 1; i <= towerImg.Length; i++)
        {
            towerImg[i - 1].gameObject.SetActive(money >= buildMoney * i);
        }
    }

    public void PlayerUISetUp(List<Sprite> img, List<string> name, List<int> money)
    {
        int idx = 0;

        foreach (RectTransform player in playerUI)
        {
            player.transform.GetChild(0).GetComponent<Image>().sprite = img[idx];
            player.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = name[idx];
            player.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = money[idx].ToString();

            idx++;
        }
    }
    public void UndoImg() //타워 건설 버튼
    {
        int objNum = (int)manager.buildCount[manager.curBlock[manager.pTurn]];

        for (int i = 0; i < towerImg.Length; i++)
            towerImg[i].gameObject.SetActive(true);

        for (int i = 0; i < objNum; i++)
            towerImg[i].gameObject.SetActive(false);
    }
    public void AiPlayerRoutine()
    {
        DiceActive(false);
        StartCoroutine(WaitingDice());

        IEnumerator WaitingDice()
        {
            yield return new WaitForSeconds(1f);

            grade = dice.CaculateGrade();
            isSpin = true;
        }
    }
    public void DiceActive(bool value) => DiceGage.SetActive(value);
    public void playerUIActive(bool value) => playerInfoUI.gameObject.SetActive(value);
    public void ChooseActive(bool value, bool active)
    {
        if (manager.isChangeColor || manager.buildingOwner[manager.curBlock[manager.pTurn]] == PlayMoney.None) SelectBuild();

        string name = value ? btnName[0] : btnName[1];
        if (active) name = btnName[0];

        cancelBtn.SetActive(true);//여기 파산하게 될때 pay가 두개가 되는데 그걸 바꾸거나 건물을 자기꺼로 만드는걸 없애거나 하삼
        cancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = name;
    }


    #region UIDotWeen
    public void ShowUI()
    {
        buildUi.transform.DOScale(Vector2.one * 1f, 2f).SetEase(Ease.InOutQuint);

        saveTower = manager.buildCount[manager.curBlock[manager.pTurn]];
    }

    public void UndoUI()
    {
        chooseBtn.SetActive(false);

        buildUi.transform.DOScale(Vector2.zero, 1.5f).SetEase(Ease.InOutQuint)
            .OnComplete(() => GameManager.Instance.UpdateState(GameState.Build));
    }

    public void EnableOutLine(PlayTurn turn)
    {
        playerUI[(int)turn % 4].GetComponent<Outline>().enabled = false;
        playerUI[((int)turn + 1) % 4].GetComponent<Outline>().enabled = true;
    }
    #endregion

    #region Button

    public void StartBtn(int num)
    {
        GameState state = (GameState)num;
        GameManager.Instance.UpdateState(state);//Setting으로 바꾸기
    }

    public void OnBtn()
    {
        grade = dice.CaculateGrade();

        isSpin = true;
    }

    public void CalcTowerNum(int value) // 건물 갯수
    {
        chooseBtn.SetActive(true);

        towerNum = value;
        manager.tower = (CurTower)towerNum;
    }

    public void Cancel()
    {
        manager.tower = saveTower;

        bool isBuild = manager.buildingOwner[manager.curBlock[manager.pTurn]] == PlayMoney.None
            || manager.buildingOwner[manager.curBlock[manager.pTurn]] == PlayMoney.player;
        BuyBuilding(isBuild);
    }
    #endregion
}
