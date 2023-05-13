using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public int PlayerId { get; private set; }
    public int Points { get; private set; }

    private string m_playerName;
    [SerializeField] RawImage m_icon;
    [SerializeField] TMP_Text m_nameText;

    [SerializeField] float m_initSpeed = 1f;
    [SerializeField] private CharacterAnimsController m_animationsController;
    int m_speedMultiplier = 1;
    private Queue<Vector2> m_destinations;
    private RectTransform m_rectTransform;

    internal void Init(
        PlayerDetails playerDetails, Vector2 initPos)
    {
        Points = 0;
        PlayerId = playerDetails.PlayerId;
        m_playerName = playerDetails.PlayerName;
        m_nameText.text = m_playerName;
        m_icon.texture = playerDetails.PlayerIcon;
        m_destinations = new Queue<Vector2>();
        m_rectTransform = GetComponent<RectTransform>();
        m_rectTransform.anchoredPosition = initPos;

    }

    internal void AddPoints(int points)
    {
        Points++;
        m_speedMultiplier++;
    }

    internal void ToggleVisible(bool toShow)
    {
        gameObject.SetActive(toShow);
    }

    internal void EnqueueNewDestination(Vector2 newDestination)
    {
        m_destinations.Enqueue(newDestination);
    }
    void Update()
    {
        if (m_destinations == null)
            return;
        if (m_destinations.Count == 0)
        {
            m_speedMultiplier = 1;
            m_animationsController.PlayIdle();
            return;
        }
        Vector2 newDestination = m_destinations.Peek();
        Vector2 curPos = m_rectTransform.localPosition;
        Vector2 difference = newDestination - curPos;
        //print(difference);
        if ((Math.Abs(difference.x) < m_initSpeed)
            && (Math.Abs(difference.y) < m_initSpeed))
        {
            m_destinations.Dequeue();
        }
        else if (difference.y >= m_initSpeed)
        {
            curPos.y += m_initSpeed * m_speedMultiplier;
            if (curPos.y > newDestination.y)
                curPos.y = newDestination.y;
            m_rectTransform.localPosition = curPos;
            m_animationsController.PlayMoveUp();
        }
        else if (difference.x >= m_initSpeed)
        {
            curPos.x += m_initSpeed * m_speedMultiplier;
            if (curPos.x > newDestination.x)
                curPos.x = newDestination.x;
            m_rectTransform.localPosition = curPos;
            m_animationsController.PlayMoveSide();
        }
        else if (difference.y < 0)
        {
            curPos.y -= m_initSpeed * m_speedMultiplier;
            if (curPos.y < newDestination.y)
                curPos.y = newDestination.y;
            m_rectTransform.localPosition = curPos;
            m_animationsController.PlayMoveDown();
        }

    }
}
