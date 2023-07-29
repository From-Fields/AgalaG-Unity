using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public static Action<bool> onPause;
    private static bool _paused = false;

    public static void SwitchPause(bool paused) {
        Time.timeScale = (paused) ? 0 : 1;
        onPause?.Invoke(paused);
    }

    public static void SwitchToScene(GameScene menu) => SceneManager.LoadScene((int) menu);

    private void Update() {
        if(InputHandler.Instance.Pause)
            SwitchPause(!_paused);
    }
}

public enum GameScene {
    MENU,
    GAME
}
