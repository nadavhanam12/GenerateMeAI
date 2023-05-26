using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using static StateController;

public class GenerateImageController : AbstractStageChangeListener
{
    public static event Action<int, GuessImageModel> OnPlayerSubmitImage;
    [SerializeField] private GuessImageModelFactory m_guessImageModelFactory;
    [SerializeField] private WebService m_webService;
    [SerializeField] private TMP_Text m_processText;
    [SerializeField] private RawImage m_image;
    [SerializeField] private Button m_readyButton;
    [SerializeField] private Texture m_loadingTexture;
    [SerializeField] private VideoPlayer m_videoPlayer;



    bool m_isGenerating;
    bool m_isImageReady;
    bool m_inWaitingState;


    int m_playerId;
    string m_userPrompt;
    public override void Init(MatchConfiguration matchConfiguration)
    {
        m_playerId = matchConfiguration.playerId;
        m_isGenerating = false;
        m_isImageReady = false;
        m_inWaitingState = false;
        m_processText.gameObject.SetActive(false);
        m_readyButton.gameObject.SetActive(false);
    }

    public override void StateChange(StateController.MatchState state)
    {
        if (state == MatchState.WaitingForImages)
        {
            m_inWaitingState = true;
            if (m_isImageReady)
                m_readyButton.interactable = true;

        }
        else if (state == MatchState.MatchStarting)
        {
            if (m_isGenerating || !m_isImageReady)
            {
                m_webService.CancelProcess();
                //submit random images
                print("submit random images");
                OnPlayerSubmitImage?.Invoke(1, m_guessImageModelFactory.Generate());
                OnPlayerSubmitImage?.Invoke(2, m_guessImageModelFactory.Generate());
            }
            else
            {
                SubmitImage();
            }
        }
    }

    public void GenerateImage()
    {
        if (m_isGenerating || m_isImageReady) return;
        m_isGenerating = true;
        ToggleLoadingTexture(true);
        m_readyButton.gameObject.SetActive(true);
        m_readyButton.interactable = false;

        m_userPrompt = m_guessImageModelFactory.GetUserPrompt();
        string themePrompt = m_guessImageModelFactory.GetThemePrompt();
        m_webService.GenerateImage(themePrompt, m_userPrompt);
    }

    private void ToggleLoadingTexture(bool isOn)
    {
        if (isOn)
        {
            m_image.texture = m_loadingTexture;
            m_videoPlayer.Play();
        }
        else
        {
            //m_image.texture = null;
            m_videoPlayer.Stop();
        }
    }

    public void SubmitImage()
    {
        if (!m_isImageReady) return;
        if (!m_readyButton.interactable) return;
        m_readyButton.interactable = false;
        GuessImageModel guessImageModel = new GuessImageModel(m_userPrompt, m_image.texture);

        OnPlayerSubmitImage?.Invoke(m_playerId, guessImageModel);
        //for testing
        OnPlayerSubmitImage?.Invoke(2, m_guessImageModelFactory.Generate());
    }

    internal void UpdateNewImage(Texture2D texture)
    {
        m_isGenerating = false;
        m_isImageReady = true;
        m_processText.gameObject.SetActive(false);
        if (m_inWaitingState)
            m_readyButton.interactable = true;
        ToggleLoadingTexture(false);
        m_image.texture = texture;
    }

    internal void UpdateImageProcessText(string message)
    {
        //m_image.texture = null;
        m_processText.gameObject.SetActive(true);
        m_processText.text = message;
    }
}
