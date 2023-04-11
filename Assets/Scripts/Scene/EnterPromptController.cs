using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static StateController;

public class EnterPromptController : AbstractStageChangeListener
{
    [SerializeField] private TMP_InputField m_promptInput;
    [SerializeField] private Button m_button;
    [SerializeField] private RawImage m_image;
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private TMP_Text m_waitingText;
    [SerializeField] private List<GuessImageModel> m_testingImageModels;


    int m_playerId;
    public static event Action<int, GuessImageModel> OnPlayerSubmitImage;
    public override void Init(MatchConfiguration matchConfiguration)
    {
        m_playerId = matchConfiguration.playerId;
        ToggleWaitText(false);
        m_canvas.enabled = false;
        gameObject.SetActive(true);
        Invoke("Submit", 2f);
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
        else if (state == MatchState.MatchStarting)
            Hide();
    }
    public void Submit()
    {
        ToggleWaitText(true);
        string inputText = m_promptInput.text;
        GuessImageModel guessImageModel = new GuessImageModel(inputText, m_image.texture);
        OnPlayerSubmitImage?.Invoke(m_playerId, guessImageModel);


        //for testing
        List<int> randomInts = GenerateRandomInts();
        OnPlayerSubmitImage?.Invoke(2, m_testingImageModels[randomInts[0]]);
        OnPlayerSubmitImage?.Invoke(3, m_testingImageModels[randomInts[1]]);
        OnPlayerSubmitImage?.Invoke(4, m_testingImageModels[randomInts[2]]);

    }

    private List<int> GenerateRandomInts()
    {
        int max = m_testingImageModels.Count;
        int randomNum;
        if (max < 3) return null;
        List<int> list = new List<int>();
        while (list.Count < 3)
        {
            randomNum = UnityEngine.Random.Range(0, max);
            if (!list.Contains(randomNum))
                list.Add(randomNum);
        }
        return list;
    }

    private void ToggleWaitText(bool isOn)
    {
        m_promptInput.interactable = !isOn;
        m_button.gameObject.SetActive(!isOn);
        m_waitingText.gameObject.SetActive(isOn);
    }
}
