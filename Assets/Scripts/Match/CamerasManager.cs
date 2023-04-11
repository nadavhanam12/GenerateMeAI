using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateController;

public class CamerasManager : AbstractStageChangeListener
{

    [SerializeField] Camera m_gameCamera;
    [SerializeField] Camera m_guessCamera;
    [Range(0, 1)][SerializeField] float m_mainCameraSize = 0.5f;
    public override void Init(MatchConfiguration matchConfiguration)
    {
        m_gameCamera.rect = new Rect(0, 0, 1, 1);
        m_guessCamera.rect = new Rect(0, 0, 1, 0);
    }

    public override void StateChange(MatchState state)
    {
        //Debug.Log("State changed " + state.ToString());
        if (state == MatchState.Match)
            SplitScreen();
        else if (state == MatchState.MatchFinish)
            UnifyScreen();
    }


    public void SplitScreen()
    {
        Rect gameRect = m_gameCamera.rect;
        Rect guessRect = m_guessCamera.rect;
        LTDescr c = LeanTween.value(m_gameCamera.gameObject, 1, m_mainCameraSize, 1)
        .setOnUpdate(
                    (float val) =>
                    {
                        gameRect.height = val;
                        gameRect.y = 1 - val;
                        m_gameCamera.rect = gameRect;

                        guessRect.height = 1 - val;
                        m_guessCamera.rect = guessRect;
                    }
                ).setEase(LeanTweenType.easeOutQuad);
    }
    public void UnifyScreen()
    {
        Rect gameRect = m_gameCamera.rect;
        Rect guessRect = m_guessCamera.rect;

        LeanTween.value(m_gameCamera.gameObject, m_mainCameraSize, 1, 1)
        .setOnUpdate(
                    (float val) =>
                    {
                        gameRect.height = val;
                        gameRect.y = 1 - val;
                        m_gameCamera.rect = gameRect;

                        guessRect.height = 1 - val + 0.01f;
                        m_guessCamera.rect = guessRect;
                    }
                ).setEase(LeanTweenType.easeOutQuad);


    }
}
