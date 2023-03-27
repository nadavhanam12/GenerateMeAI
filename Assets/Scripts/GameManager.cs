using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{

    private GameCanvas m_gameCanvas;
    private CamerasManager m_camerasManager;
    StateController m_stateController;
    [Inject]
    public void Construct(GameCanvas gameCanvas, CamerasManager camerasManager, StateController stateController)
    {
        m_stateController = stateController;
        m_gameCanvas = gameCanvas;
        m_camerasManager = camerasManager;
    }
    void Start()
    {
        m_stateController.Init();
        m_camerasManager.Init();
        m_gameCanvas.init(this);

    }







}
