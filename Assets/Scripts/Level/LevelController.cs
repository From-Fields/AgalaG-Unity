using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController: MonoBehaviour
{
    [SerializeField]
    private Player _player;
    [SerializeField]
    private iLevel _level;

    private WaveController _currentWave;

    public Action onNoWave;
    public Action onGameOver;

    private void CallNextWave() {
        if(_currentWave != null) {
            _currentWave.onWaveDone -= CallNextWave;
            _currentWave = null;
        }

        if(!_level.HasNextWave) {
            EndLevel();
            return;
        }

        _currentWave = _level.GetNextWave();
        _currentWave.onWaveDone += CallNextWave;
        _currentWave.Initialize(_level.Bounds);
    }
    private void EndLevel() {
        onNoWave?.Invoke();
        ClearEvents();
    } 
    private void GameOver() {
        onGameOver?.Invoke();
        ClearEvents();
    } 

    private void ClearEvents() {
        onNoWave = null;
        onGameOver = null;
        _player.onDeath -= GameOver;
    }

    public void Awake() {
        #if UNITY_EDITOR
            if(GameManager.PreviousScene != GameScene.MENU) {
                GameManager.SwitchToScene(GameScene.MENU);
                return;
            }
            else
        #endif

        _level = GameManager.Instance.Level;

        _player.onDeath += GameOver;
        CallNextWave();
    }
}