using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController: MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    [SerializeField]
    private Player _player;
    [SerializeField]
    private iLevel _level;
    [SerializeField]
    private GameplayUIManager gameplayUIManager;
    [SerializeField]
    private GameOverUIManager gameOverUIManager;

    private WaveController _currentWave;

    public Action onNoWave;
    public Action onGameOver;

    private int _score = 0;

    public int Score => _score;

    public void Awake()
    {
        Instance = this;

#if UNITY_EDITOR
        //if (GameManager.PreviousScene != GameScene.MENU)
        //{
        //    GameManager.SwitchToScene(GameScene.MENU);
        //    return;
        //}
        //else
#endif

        _level = GameManager.Instance.Level;
        gameplayUIManager.UpdatePoints(_score);

        _player.onLifeUpdate += OnLifeChange;
        _player.onShieldUpdate += OnShieldUpdate;
        _player.onNewWeapon += OnWeaponUpdate;
        _player.onWeaponShoot += OnWeaponShoot;
        _player.onDeath += GameOver;

        CallNextWave();
    }

    public void AddScore(int score)
    {
        _score += score;
        gameplayUIManager.UpdatePoints(_score);
    }

    public void RestartLevel()
    {
        GameManager.SwitchToScene(GameScene.GAME);
        GameManager.SwitchPause(false);
    }

    private void OnLifeChange(int life)
    {
        gameplayUIManager.UpdateLifeCount(life);
    }

    private void OnShieldUpdate(int shield)
    {
        gameplayUIManager.UpdateShieldCount(shield);
    }

    private void OnWeaponUpdate(Sprite sprite, string maxAmmunition)
    {
        gameplayUIManager.UpdateAmmunitionCount(maxAmmunition);
        gameplayUIManager.UpdateShotIcon(sprite);
    }

    private void OnWeaponShoot(string currentAmmo)
    {
        gameplayUIManager.UpdateAmmunitionCount(currentAmmo);
    }

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
        gameOverUIManager.Show();
    } 

    private void ClearEvents() {
        onNoWave = null;
        onGameOver = null;
        _player.onDeath -= GameOver;
    }

}