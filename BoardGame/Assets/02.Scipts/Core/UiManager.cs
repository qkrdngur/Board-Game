using DG.Tweening;
using UnityEngine;
using BoardGame.Util;
using UnityEngine.UI;

public class UiManager : MonoSingleton<UiManager>
{
    DiceGague dice;
    GameManager manager;

    [HideInInspector] public float grade;
    [HideInInspector] public bool isSpin = false;

    [SerializeField] private RectTransform buildUi;
    [SerializeField] private GameObject[] towerImg;

    public Transform dicePoint;
    private int towerNum;

    private void Start()
    {
        dice = GetComponent<DiceGague>();
        manager = GameManager.Instance;
    }

    public void UndoImg()
    {
        int objNum =
            (int)manager.buildCount[manager.curBlock[manager.pTurn]];

        for(int i = 0; i < towerImg.Length; i++)
            towerImg[i].gameObject.SetActive(true);

        for (int i = 0; i < objNum - 1; i++)
                towerImg[i].gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        buildUi.transform.DOScale(Vector2.one * 1f, 2f).SetEase(Ease.InOutQuint);
    }

    public void UndoUI()
    {
        buildUi.transform.DOScale(Vector2.zero, 1.5f).SetEase(Ease.InOutQuint)
            .OnComplete(() => GameManager.Instance.UpdateState(GameState.Build));
    }

    #region Button
    public void OnBtn()
    {
        grade = dice.CaculateGrade();

        isSpin = true;
    }

    public void CalcTowerNum(int value) // °Ç¹° °¹¼ö
    {
        towerNum = value;

        manager.tower = (CurTower)towerNum;
        print(manager.tower);
    }

    public void TakeOverTower()
    {

    }
    #endregion
}
