using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Letter : MonoBehaviour
{

    protected char m_letter;
    [SerializeField] protected TMP_Text m_letterText;

    public virtual void Init(char c)
    {
        gameObject.SetActive(true);

        if (c == ' ') return;
        m_letter = c;
        m_letterText.text = m_letter.ToString();
    }

    public virtual char GetLetter()
    {
        return m_letter;
    }


}
