using BoardGame.Util;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void StartBtn()
    {
        GameManager.Instance.UpdateState(GameState.Main);
    }
}
