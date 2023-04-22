using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissingLetter : Letter
{
    [SerializeField] private float m_revealDuration = 0.5f;
    [SerializeField] private Image m_backgroundImage;
    [SerializeField] private Color m_failColor = Color.red;
    [SerializeField] private Color m_successColor = Color.green;

    [SerializeField] private float m_failTweenDuration = 0.2f;
    [SerializeField] private int m_tweenRepeat = 2;
    public bool IsRevealed { get; private set; }
    public override void Init(char c)
    {
        base.Init(c);
        IsRevealed = false;
        m_letterText.text = "_";

    }
    internal void FailTry()
    {
        //print("Fail Try");
        LeanTween.color(m_backgroundImage.rectTransform, m_failColor, m_failTweenDuration)
        .setLoopPingPong(m_tweenRepeat);

    }
    internal void Reveal()
    {
        IsRevealed = true;
        //disapear
        Color initColor;
        initColor = m_letterText.color;
        initColor.a = 0;
        m_letterText.color = initColor;

        //change text to right char
        m_letterText.text = m_letter.ToString();

        //fade in
        LeanTweenExt.FadeText(m_letterText, 1.0f, m_revealDuration)
        .setEaseOutQuad();
        LeanTween.color(m_backgroundImage.rectTransform, m_successColor, m_failTweenDuration)
       .setLoopPingPong(m_tweenRepeat);
    }
}
