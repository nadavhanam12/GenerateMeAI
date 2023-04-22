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
    [SerializeField] private TMP_Dropdown m_themeDropdown;

    [SerializeField] private ThemeImages m_christmasTheme;
    [SerializeField] private ThemeImages m_vanGoghTheme;


    bool m_submitted;
    int m_playerId;
    public static event Action<int, GuessImageModel> OnPlayerSubmitImage;
    public override void Init(MatchConfiguration matchConfiguration)
    {
        m_playerId = matchConfiguration.playerId;
        ToggleWaitText(false);
        m_canvas.enabled = false;
        gameObject.SetActive(true);
        m_submitted = false;
        m_promptInput.interactable = false;
        //Invoke("Submit", 1f);
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
        {
            if (!m_submitted)
                Submit();
            Hide();
        }
    }
    public void Submit()
    {
        m_submitted = true;
        ToggleWaitText(true);
        string inputText = m_promptInput.text;
        GuessImageModel guessImageModel = new GuessImageModel(inputText, m_image.texture);
        OnPlayerSubmitImage?.Invoke(m_playerId, guessImageModel);

        //for testing
        if (m_themeDropdown.value == 0)
        {
            List<int> randomInts = GenerateRandomInts(m_christmasTheme.Images);
            OnPlayerSubmitImage?.Invoke(2, m_christmasTheme.Images[randomInts[0]]);
            OnPlayerSubmitImage?.Invoke(3, m_christmasTheme.Images[randomInts[1]]);
            OnPlayerSubmitImage?.Invoke(4, m_christmasTheme.Images[randomInts[2]]);
        }
        else
        {
            List<int> randomInts = GenerateRandomInts(m_vanGoghTheme.Images);
            OnPlayerSubmitImage?.Invoke(2, m_vanGoghTheme.Images[randomInts[0]]);
            OnPlayerSubmitImage?.Invoke(3, m_vanGoghTheme.Images[randomInts[1]]);
            OnPlayerSubmitImage?.Invoke(4, m_vanGoghTheme.Images[randomInts[2]]);
        }


    }

    private List<int> GenerateRandomInts(List<GuessImageModel> m_testingImages)
    {
        int max = m_testingImages.Count;
        int randomNum;
        if (max < 3) return null;
        List<int> list = new List<int>();
        while (list.Count < 3)
        {
            randomNum = UnityEngine.Random.Range(1, max);
            if (!list.Contains(randomNum))
                list.Add(randomNum);
        }
        return list;
    }

    private void ToggleWaitText(bool isOn)
    {
        //m_promptInput.interactable = !isOn;
        m_button.gameObject.SetActive(!isOn);
        m_waitingText.gameObject.SetActive(isOn);
    }
    public void OnThemesChanged()
    {
        if (m_themeDropdown.value == 0)
        {
            m_image.texture = m_christmasTheme.Images[0].Image;
        }
        else
        {
            m_image.texture = m_vanGoghTheme.Images[0].Image;
        }
    }
}
