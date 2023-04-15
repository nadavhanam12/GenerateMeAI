using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuessImageController : MonoBehaviour
{
    PlayerDetails m_playerDetails;
    GuessImageModel m_guessImageModel;
    [SerializeField] private TMP_Text m_playerName;
    [SerializeField] private RawImage m_playerIcon;
    [SerializeField] private RawImage m_playerImage;
    [SerializeField] private GuessPromptController m_guessPromptController;
    private GuessManager m_manager;


    internal void Init(GuessManager manager, PlayerDetails playerDetails)
    {
        m_manager = manager;
        m_playerDetails = playerDetails;
        m_playerName.text = m_playerDetails.PlayerName;
        m_playerIcon.texture = m_playerDetails.PlayerIcon;
        m_guessPromptController.Init(this);
    }
    internal int GetPlayerId()
    {
        return m_playerDetails.PlayerId;
    }
    internal void SetGuessImageModel(GuessImageModel guessImageModel)
    {
        m_guessImageModel = guessImageModel;
        m_playerImage.texture = m_guessImageModel.Image;
        m_guessPromptController.SetPrompt(guessImageModel.PromptText);
    }

    public void AddPoint()
    {
        m_manager.AddPoints();
    }


}
