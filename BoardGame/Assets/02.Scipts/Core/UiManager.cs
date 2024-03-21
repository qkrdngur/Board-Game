using DG.Tweening;
using UnityEngine;
using BoardGame.Util;
using UnityEngine.UI;

public class UiManager : MonoSingleton<UiManager>
{
    DiceGague dice;

    [HideInInspector] public float grade;
    [HideInInspector] public bool isSpin = false;

    [SerializeField] private RectTransform buildUi;
    [SerializeField] private GameObject[] towerImg;

    public Transform dicePoint;
    private int towerNum;

    private void Start()
    {
        dice = GetComponent<DiceGague>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ShowUI();
            UndoImg();
        }
    }

    public void UndoImg()
    {
        int objNum = (int)GameManager.Instance.buildCount[GameManager.Instance.curBlock[GameManager.Instance.pTurn]];

        for (int i = 0; i < objNum; i++)
        {
            if (towerImg[i].activeSelf)
                towerImg[i].gameObject.SetActive(false);
        }
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

    public void OnBtn()
    {
        grade = dice.CaculateGrade();

        isSpin = true;
    }

    public void OnBtn(int value)
    {
        towerNum = value;

        GameManager.Instance.tower = (CurTower)towerNum;
    }
}
