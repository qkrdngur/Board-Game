using DG.Tweening;
using UnityEngine;
using BoardGame.Util;

public class UiManager : MonoSingleton<UiManager>
{
    DiceGague dice;

    [HideInInspector] public float grade;
    [HideInInspector] public bool isSpin = false;

    [SerializeField] private RectTransform buildUi;

    public Transform dicePoint;

    private void Start()
    {
        dice = GetComponent<DiceGague>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
            ShowUI();
    }
    
    public void ShowUI()
    {
        buildUi.transform.DOScale(Vector2.one * 1f, 2f).SetEase(Ease.InOutQuint);
    }

    public void UnShowUI()
    {
        buildUi.transform.DOScale(Vector2.zero, 1.5f).SetEase(Ease.InOutQuint);
    }

    public void OnBtn()
    {
        grade = dice.CaculateGrade();

        isSpin = true;
    }
}
