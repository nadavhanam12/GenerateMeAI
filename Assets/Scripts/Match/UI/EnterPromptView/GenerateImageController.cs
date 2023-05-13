using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static StateController;

public class GenerateImageController : AbstractStageChangeListener
{
    public static event Action<int, GuessImageModel> OnPlayerSubmitImage;
    [SerializeField] private GuessImageModelFactory m_guessImageModelFactory;
    [SerializeField] private WebService m_webService;
    [SerializeField] private TMP_Text m_processText;
    [SerializeField] private RawImage m_image;
    [SerializeField] private Button m_readyButton;


    bool m_isGenerating;
    int m_playerId;
    string m_userPrompt;
    public override void Init(MatchConfiguration matchConfiguration)
    {
        m_playerId = matchConfiguration.playerId;
        m_isGenerating = false;
        m_processText.gameObject.SetActive(false);
        m_readyButton.gameObject.SetActive(false);
    }

    public override void StateChange(StateController.MatchState state)
    {
        if (state == MatchState.MatchStarting)
        {
            m_webService.CancelProcess();
        }
    }

    public void GenerateImage()
    {
        if (m_isGenerating) return;
        m_isGenerating = true;
        m_readyButton.gameObject.SetActive(true);
        m_readyButton.interactable = false;

        m_userPrompt = m_guessImageModelFactory.GetUserPrompt();
        string themePrompt = m_guessImageModelFactory.GetThemePrompt();
        m_webService.GenerateImage(themePrompt, m_userPrompt);
    }

    public void SubmitImage()
    {
        GuessImageModel guessImageModel;
        guessImageModel = new GuessImageModel(m_userPrompt, m_image.texture);

        OnPlayerSubmitImage?.Invoke(m_playerId, guessImageModel);
        //for testing
        OnPlayerSubmitImage?.Invoke(2, m_guessImageModelFactory.Generate());
        OnPlayerSubmitImage?.Invoke(3, m_guessImageModelFactory.Generate());
        OnPlayerSubmitImage?.Invoke(4, m_guessImageModelFactory.Generate());
    }

    internal void UpdateNewImage(Texture2D texture)
    {
        m_isGenerating = false;
        m_processText.gameObject.SetActive(false);
        m_readyButton.interactable = true;
        m_image.texture = texture;
    }

    internal void UpdateImageProcessText(string message)
    {
        m_image.texture = null;
        m_processText.gameObject.SetActive(true);
        m_processText.text = message;
    }
}
