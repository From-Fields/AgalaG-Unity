using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public static Action<bool> onPause;
    private static bool _paused = false;
    private static GameScene _currentScene, _previousScene = (GameScene)(-1);
    private iLevel _level;

    public iLevel Level {
        get { return _level; }
        set {
            if(_currentScene != GameScene.GAME)
                _level = value;
        }
    }
    public static GameScene CurrentScene => _currentScene;
    public static GameScene PreviousScene => _previousScene;

    public static void SwitchPause(bool paused) {
        Time.timeScale = (paused) ? 0 : 1;
        onPause?.Invoke(paused);
    }

    public static void SwitchToScene(GameScene scene) {
        _previousScene = _currentScene;
        _currentScene = scene;
        SceneManager.LoadScene((int) scene);
    }
    private void Update() {
        if(InputHandler.Instance.Pause)
            SwitchPause(!_paused);
    }
}

public enum GameScene {
    MENU,
    GAME
}
