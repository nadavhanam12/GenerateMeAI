using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    [SerializeField] TMP_Text m_text;
    [SerializeField] RawImage m_iconImage;
    [SerializeField] float m_speed = 1f;
    PlayerDetails m_playerDetails;
    RectTransform rectTransform;
    Vector2[] m_positionsArray;
    int m_curPosition;


    //for testing- should be in IconsMovementManager
    public void Init(PlayerDetails playerDetails, Vector2[] positionsArray)
    {
        m_playerDetails = playerDetails;
        m_curPosition = m_playerDetails.PlayerId - 1;
        m_iconImage.texture = playerDetails.PlayerIcon;
        m_text.text = "0";
        m_positionsArray = positionsArray;
        rectTransform = GetComponent<RectTransform>();
    }


    internal void SetPos(int posIndex)
    {
        m_curPosition = posIndex;
    }

    void Update()
    {
        Vector2 curPos = rectTransform.anchoredPosition;
        Vector2 pos = m_positionsArray[m_curPosition];
        if (curPos != pos)
        {
            if (curPos.y < pos.y)
            {
                curPos.y += m_speed;
            }
            else
            {
                curPos.y -= m_speed;
            }
            rectTransform.anchoredPosition = curPos;
        }
    }
    internal void SetScore(int score)
    {
        m_text.text = score.ToString();
    }
}
