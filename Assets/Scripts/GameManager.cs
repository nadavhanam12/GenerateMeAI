using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Camera m_gameCamera;
    [SerializeField] Camera m_guessCamera;

    void Start()
    {
        m_guessCamera.rect = new Rect(0, 0, 1, 0);
        m_gameCamera.rect = new Rect(0, 0, 1, 1);
        Invoke("SplitScreen", 1);
        Invoke("UnifyScreen", 3);

    }



    private void SplitScreen()
    {
        Rect gameRect = new Rect(0, 0, 1, 1);
        Rect guessRect = new Rect(0, 0, 1, 0);
        LTDescr c = LeanTween.value(m_gameCamera.gameObject, 1, 0.6f, 1).setOnUpdate(
                    (float val) =>
                    {
                        gameRect.height = val;
                        gameRect.y = 1 - val;
                        m_gameCamera.rect = gameRect;
                    }
                ).setEase(LeanTweenType.easeOutQuad);

        LeanTween.value(m_guessCamera.gameObject, 0, 0.4f, 1).setOnUpdate(
                    (float val) =>
                    {
                        guessRect.height = val;
                        m_guessCamera.rect = guessRect;
                    }
                ).setEase(LeanTweenType.easeOutQuad);

        m_guessCamera.CalculateFrustumCorners();
    }
    private void UnifyScreen()
    {
        Rect gameRect = new Rect(0, 0.4f, 0, 0.6f);
        Rect guessRect = new Rect(0, 0, 1, 0.4f);

        LeanTween.value(m_gameCamera.gameObject, 0.6f, 1, 1).setOnUpdate(
                    (float val) =>
                    {
                        gameRect.height = val;
                        gameRect.y = 1 - val;
                        m_gameCamera.rect = gameRect;
                    }
                ).setEase(LeanTweenType.easeOutQuad);

        LeanTween.value(m_guessCamera.gameObject, 0.4f, 0.01f, 1).setOnUpdate(
                    (float val) =>
                    {
                        guessRect.height = val;
                        m_guessCamera.rect = guessRect;
                    }
                ).setEase(LeanTweenType.easeOutQuad);
    }



}
