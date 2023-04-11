using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateController;

public abstract class AbstractStageChangeListener : MonoBehaviour
{
    public abstract void Init(MatchConfiguration matchConfiguration);

    protected virtual void OnEnable()
    {
        StateController.OnStateChange += StateChange;
    }


    protected virtual void OnDisable()
    {
        StateController.OnStateChange -= StateChange;
    }


    public abstract void StateChange(MatchState state);


}
