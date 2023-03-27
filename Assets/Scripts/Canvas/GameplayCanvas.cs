using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCanvas : MonoBehaviour
{

    private GameCanvas m_gameCanvas;




    public void Init(GameCanvas gameCanvas)
    {
        m_gameCanvas = gameCanvas;
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
