using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimsController : MonoBehaviour
{
    [SerializeField] Animator m_animatorController;
    private enum CharacterAnimState
    { IdleUp, IdleSide, IdleDown, MoveUp, MoveSide, MoveDown };
    private CharacterAnimState m_curAnimState = CharacterAnimState.IdleUp;
    internal void PlayIdle()
    {
        switch (m_curAnimState)
        {
            case CharacterAnimState.MoveUp:
                SetAnimState(CharacterAnimState.IdleUp);
                break;
            case CharacterAnimState.MoveSide:
                SetAnimState(CharacterAnimState.IdleSide);
                break;
            case CharacterAnimState.MoveDown:
                SetAnimState(CharacterAnimState.IdleDown);
                break;
        }
    }

    internal void PlayMoveDown()
    {
        SetAnimState(CharacterAnimState.MoveDown);
    }

    internal void PlayMoveSide()
    {
        SetAnimState(CharacterAnimState.MoveSide);
    }

    internal void PlayMoveUp()
    {
        SetAnimState(CharacterAnimState.MoveUp);
    }

    void SetAnimState(CharacterAnimState state)
    {
        if (m_curAnimState == state)
            return;
        m_curAnimState = state;
        m_animatorController.SetTrigger(state.ToString());
    }
}
