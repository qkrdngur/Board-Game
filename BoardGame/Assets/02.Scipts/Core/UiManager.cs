using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;

public class UiManager : MonoSingleton<UiManager>
{
    DiceGague dice;

    [HideInInspector] public float grade;
    [HideInInspector] public bool isSpin = false;

    public Transform dicePoint;

    private void Start()
    {
        dice = GetComponent<DiceGague>();
    }

    private void Update()
    {

    }

    public void OnBtn()
    {
        grade = dice.CaculateGrade();

        isSpin = true;
    }
}
