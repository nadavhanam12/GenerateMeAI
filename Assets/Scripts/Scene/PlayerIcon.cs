using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    [SerializeField] TMP_Text m_text;
    [SerializeField] RawImage m_iconImage;

    //for testing- should be in IconsMovementManager
    int score;
    public void Init(PlayerDetails playerDetails)
    {
        m_iconImage.texture = playerDetails.PlayerIcon;
        score = 0;
        m_text.text = score.ToString();
    }
    public void AddScore(int score)
    {
        score += score;
        m_text.text = score.ToString();

    }
}
