using BoardGame.Util;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoSingleton<UiManager>
{
    DiceGague dice;
    GameManager manager;

    [SerializeField] private RectTransform buildUi;
    [SerializeField] private GameObject chooseBtn;
    [SerializeField] private GameObject[] towerImg;

    public bool isSpin { get; set; } = false;
    public float grade { get; private set; }

    public List<GameObject> playerUI;
    public Transform dicePoint;
    public GameObject DiceGage;

    private int towerNum;
    private CurTower saveTower = CurTower.none;

    private void Awake()
    {
        manager = GameManager.Instance;
        dice = GetComponent<DiceGague>();
    }

    public void PlayerUISetUp(List<Sprite> img, List<string> name, List<int> money)
    {
        int idx = 0;

        foreach (GameObject player in playerUI)
        {
            player.transform.GetChild(0).GetComponent<Image>().sprite = img[idx];
            player.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = name[idx];
            player.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = money[idx].ToString();

            idx++;
        }
    }

    public void UndoImg() //타워 건설 버튼
    {
        int objNum =
            (int)manager.buildCount[manager.curBlock[manager.pTurn]];

        for (int i = 0; i < towerImg.Length; i++)
            towerImg[i].gameObject.SetActive(true);

        for (int i = 0; i < objNum; i++)
            towerImg[i].gameObject.SetActive(false);
    }

    public void ChooseActive(bool value)
    {
        chooseBtn.SetActive(value);
    }

    #region UIDotWeen
    public void ShowUI()
    {
        buildUi.transform.DOScale(Vector2.one * 1f, 2f).SetEase(Ease.InOutQuint);

        saveTower = manager.tower;
    }

    public void UndoUI()
    {
        buildUi.transform.DOScale(Vector2.zero, 1.5f).SetEase(Ease.InOutQuint)
            .OnComplete(() => GameManager.Instance.UpdateState(GameState.Build));
    }
    #endregion

    #region Button
    public void OnBtn()
    {
        grade = dice.CaculateGrade();

        isSpin = true;
    }

    public void CalcTowerNum(int value) // 건물 갯수
    {
        towerNum = value;

        manager.tower = (CurTower)towerNum;
    }

    public void Cancel()
    {
        manager.tower = saveTower;
        print(manager.tower);   
    }
    #endregion
}
