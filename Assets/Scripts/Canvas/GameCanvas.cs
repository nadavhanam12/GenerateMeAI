using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] EnterPromptCanvas m_enterPromptCanvas;
    [SerializeField] GameplayCanvas m_gameplayCanvas;
    [SerializeField] private PlayerData m_playerData;
    private GameManager m_gameManager;
    private StateController m_stateController;

    [Inject]
    public void Construct(StateController stateController)
    {
        m_stateController = stateController;
    }
    public void init(GameManager gameManager)
    {
        m_gameManager = gameManager;
        m_enterPromptCanvas.Activate();
        m_gameplayCanvas.Disable();

        m_enterPromptCanvas.Init(this);
        m_gameplayCanvas.Init(this);



    }


    public void PlayerDataSubmit(string name, string prompt, Texture2D generatedPicture)
    {
        //m_playerData.SetPlayerData(name, prompt, generatedPicture);
        m_enterPromptCanvas.Disable();
        m_gameplayCanvas.Activate();
        m_stateController.PlayerSubmitedData();
    }





}
