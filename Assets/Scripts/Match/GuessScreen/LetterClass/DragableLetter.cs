using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableLetter : Letter, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    Canvas m_canvas;
    RectTransform m_rectTransform;
    GuessPromptController m_controller;
    private Vector2 offset;
    private Vector2 m_initPos;

    public void Init(GuessPromptController controller,
    Canvas canvas,
     char c)
    {
        base.Init(c);
        m_rectTransform = gameObject.GetComponent<RectTransform>();
        m_canvas = canvas;
        m_controller = controller;

    }

    internal void Destroy()
    {
        Destroy(gameObject);
    }

    internal void InitPosition()
    {
        m_rectTransform.anchoredPosition = m_initPos;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        m_initPos = m_rectTransform.anchoredPosition;
        Vector2 result;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        ((RectTransform)m_canvas.transform, eventData.position, m_canvas.worldCamera, out result);

        offset = (Vector2)m_rectTransform.localPosition - result;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the UI element based on the pointer's position and the saved offset
        Vector2 result;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                ((RectTransform)m_canvas.transform, eventData.position, m_canvas.worldCamera, out result);
        m_rectTransform.localPosition = result + offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_controller.OnLetterRelease(this);
    }
}
