using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterPromptCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private RawImage m_image;
    [SerializeField] private Texture2D m_exampleImage;
    private GameCanvas m_gameCanvas;

    public void Init(GameCanvas gameCanvas)
    {
        m_gameCanvas = gameCanvas;
        Submit();
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }


    public void Submit()
    {
        string inputText = m_text.text;
        m_image.texture = m_exampleImage;
        m_gameCanvas.PlayerDataSubmit("Player Name Example", inputText, m_exampleImage);
    }

}
