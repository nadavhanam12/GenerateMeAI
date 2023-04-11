using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchInitializer : MonoBehaviour
{
    [SerializeField] List<AbstractStageChangeListener> m_controllersToInitialize;
    [SerializeField] StateController m_stateController;

    internal void Init(MatchConfiguration matchConfiguration)
    {
        foreach (AbstractStageChangeListener controller in m_controllersToInitialize)
            controller.Init(matchConfiguration);
    }

    internal void StartMatch()
    {
        m_stateController.StartMatch();
    }
}
