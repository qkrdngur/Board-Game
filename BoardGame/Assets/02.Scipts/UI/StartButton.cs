using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;

public class StartButton : MonoBehaviour
{
    public void StartBtn()
    {
        GameManager.instance.UpdateState(GameState.Main);
    }
}
