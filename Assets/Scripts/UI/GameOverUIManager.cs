using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverUIManager : MonoBehaviour
{
    [SerializeField]
    private UIDocument _document;

    [Space]
    [SerializeField] private string scoreTag;
    [SerializeField] private string buttonRetryTag;
    [SerializeField] private string buttonExitTag;

    private Label _scoreLabel;

    void Awake()
    {
        _document.gameObject.SetActive(true);
        VisualElement root = _document.rootVisualElement;

        _scoreLabel = root.Q<Label>(scoreTag);
        
        Button retryBtn = root.Q<Button>(buttonRetryTag);
        Button exitBtn = root.Q<Button>(buttonExitTag);

        retryBtn.clicked += Restart;
        exitBtn.clicked += GoToMenu;

        _document.SetActive(false);
    }

    public void Show()
    {
        _document.SetActive(true);
        _scoreLabel.text = LevelController.Instance.Score.ToString();
    }

    private void Restart()
    {
        LevelController.Instance.RestartLevel();
    }

    private void GoToMenu()
    {
        GameManager.SwitchPause(false);
        GameManager.SwitchToScene(GameScene.MENU);
    }
}
