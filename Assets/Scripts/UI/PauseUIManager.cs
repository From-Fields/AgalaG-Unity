using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseUIManager : MonoBehaviour
{
    [SerializeField]
    private UIDocument _document;
    [SerializeField]
    private string _btnResumeName, _btnRetryName, _btnQuitName;

    private void Awake()
    {
        _document.gameObject.SetActive(true);
        VisualElement root = _document.rootVisualElement;

        Button resumeButton = root.Q<Button>(_btnResumeName);
        Button retryButton = root.Q<Button>(_btnRetryName);
        Button quitButton = root.Q<Button>(_btnQuitName);

        resumeButton.clicked += Resume;
        retryButton.clicked += Retry;
        quitButton.clicked += GoToMenu;

        GameManager.onPause += this.SetActive;
        _document.SetActive(false);
    }

    private void Resume() => GameManager.SwitchPause(false);
    private void Retry()
    {
        // TODO: Create Settings Behaviour
        Debug.Log("Retry Button clicked!");
    }
    private void GoToMenu() {
        GameManager.SwitchPause(false);
        GameManager.SwitchToScene(GameScene.MENU);
    } 
    private void SetActive(bool active) => _document.SetActive(active);

    private void OnDestroy() {
        GameManager.onPause -= this.SetActive;
    }
}
