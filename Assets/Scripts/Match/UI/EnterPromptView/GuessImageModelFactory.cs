using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuessImageModelFactory : MonoBehaviour
{

    [SerializeField] private ThemeImages m_christmasTheme;
    [SerializeField] private ThemeImages m_vanGoghTheme;
    private TMP_Dropdown m_themeDropdown;
    private ThemeImages m_chosenTheme;
    RawImage m_image;
    private TMP_InputField m_promptInput;
    public void Init(
        TMP_Dropdown themeDropdown,
         RawImage image,
          TMP_InputField promptInput)
    {
        m_themeDropdown = themeDropdown;
        m_image = image;
        m_promptInput = promptInput;
        m_chosenTheme = m_christmasTheme;
    }
    public GuessImageModel Generate()
    {
        int rnd = UnityEngine.Random.Range(0, m_chosenTheme.Images.Count);
        return m_chosenTheme.Images[rnd];
    }

    public void OnThemesChanged()
    {
        if (m_themeDropdown.value == 0)
            m_chosenTheme = m_christmasTheme;
        else
            m_chosenTheme = m_vanGoghTheme;

        GuessImageModel guessImageModel = Generate();
        m_image.texture = guessImageModel.Image;
        m_promptInput.text = guessImageModel.PromptText;
    }
    public string GetThemePrompt()
    {
        return m_chosenTheme.ThemePrompt;
    }
    public string GetUserPrompt()
    {
        return m_promptInput.text;
    }
}
