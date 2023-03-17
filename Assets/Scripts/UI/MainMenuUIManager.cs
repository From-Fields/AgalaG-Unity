using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuUIManager : MonoBehaviour
{
    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button startButton = root.Q<Button>("Btn_Start");
        Button settingsButton = root.Q<Button>("Btn_Settings");
        Button exitButton = root.Q<Button>("Btn_Exit");

        startButton.clicked += StartGame;
        settingsButton.clicked += GotoSettings;
        exitButton.clicked += ExitGame;
    }

    private void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void GotoSettings()
    {
        // TODO: Create Settings Behaviour
        Debug.Log("Settings Button clicked!");
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        return;
#endif
        Application.Quit();
    }
}
