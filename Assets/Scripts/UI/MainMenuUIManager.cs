using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuUIManager : MonoBehaviour
{
    public AudioSettings audioSettings;

    private GroupBox _mainOptions;
    private GroupBox _settings;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        _mainOptions = root.Q<GroupBox>("MainOptions");
        _settings = root.Q<GroupBox>("Settings");

        _settings.SetEnabled(false);

        Button startButton = root.Q<Button>("Btn_Start");
        Button settingsButton = root.Q<Button>("Btn_Settings");
        Button exitButton = root.Q<Button>("Btn_Exit");

        Toggle toggleFullscreen = root.Q<Toggle>("Toggle_Fullscreen");

        Slider masterVolume = root.Q<Slider>("MasterVolume");
        Slider musicVolume = root.Q<Slider>("MusicVolume");
        Slider SfxVolume = root.Q<Slider>("SfxVolume");

        Button backButton = root.Q<Button>("Btn_Back");

        startButton.clicked += StartGame;
        settingsButton.clicked += GotoSettings;
        exitButton.clicked += ExitGame;

        toggleFullscreen.RegisterValueChangedCallback(SetFullScreen);

        masterVolume.RegisterValueChangedCallback(SetMasterVolume);
        musicVolume.RegisterValueChangedCallback(SetMusicVolume);
        SfxVolume.RegisterValueChangedCallback(SetSFXVolume);

        backButton.clicked += BackToMainOptions;
    }

    #region MainOptions
    private void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void GotoSettings()
    {
        _mainOptions.SetEnabled(false);
        _settings.SetEnabled(true);
        _mainOptions.visible = false;
        _settings.visible = true;
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        return;
#endif
        Application.Quit();
    }
    #endregion

    #region Settings
    private void SetFullScreen(ChangeEvent<bool> fullscreen)
    {
        Debug.Log($"Fullscreen: {fullscreen.newValue}");
        Screen.fullScreen = fullscreen.newValue;
    }

    private void SetMasterVolume(ChangeEvent<float> volume)
    {
        audioSettings.ApplyMasterVolume((int) volume.newValue);
    }

    private void SetMusicVolume(ChangeEvent<float> volume)
    {
        audioSettings.ApplyMusicVolume((int) volume.newValue);
    }

    private void SetSFXVolume(ChangeEvent<float> volume)
    {
        audioSettings.ApplySFXVolume((int) volume.newValue);
    }

    private void BackToMainOptions()
    {
        _mainOptions.SetEnabled(true);
        _settings.SetEnabled(false);
        _mainOptions.visible = true;
        _settings.visible = false;
    }

    #endregion
}
