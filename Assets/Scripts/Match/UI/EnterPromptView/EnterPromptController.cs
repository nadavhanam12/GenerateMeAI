using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static StateController;

public class EnterPromptController : AbstractStageChangeListener
{
    [SerializeField] private GuessImageModelFactory m_guessImageModelFactory;
    [SerializeField] private GenerateImageController m_generateImageController;
    [SerializeField] private TMP_InputField m_promptInput;
    [SerializeField] private Button m_submitPromptButton;
    [SerializeField] private RawImage m_image;
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private TMP_Dropdown m_themeDropdown;


    public override void Init(MatchConfiguration matchConfiguration)
    {
        ToggleInteractables(true);
        m_canvas.enabled = false;
        gameObject.SetActive(true);
        m_image.texture = null;
        m_promptInput.text = "";
        m_guessImageModelFactory.Init(m_themeDropdown, m_image, m_promptInput);
    }

    void Show()
    {
        m_canvas.enabled = true;
    }

    void Hide()
    {
        m_canvas.enabled = false;
    }

    public override void StateChange(MatchState state)
    {
        if (state == MatchState.ChoosingPrompt)
            Show();
        else if (state == MatchState.WaitingForImages)
        {
            ToggleInteractables(false);
            m_generateImageController.GenerateImage();
        }
        else if (state == MatchState.MatchStarting)
        {
            Hide();
        }
    }

    public void SubmitPrompt()
    {
        m_generateImageController.GenerateImage();
    }

    private void ToggleInteractables(bool isOn)
    {
        m_promptInput.interactable = !isOn;
        m_themeDropdown.interactable = !isOn;
        m_submitPromptButton.gameObject.SetActive(!isOn);
    }
}
