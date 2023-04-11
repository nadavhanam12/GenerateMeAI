using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessPromptController : MonoBehaviour
{
    GuessImageController m_guessImageController;
    [SerializeField] GuessLettersFactory m_guessLettersFactory;
    [SerializeField] int m_hiddenIndexesCount = 5;
    string m_prompt;
    public void Init(GuessImageController guessImageController)
    {
        m_guessImageController = guessImageController;
    }

    public void SetPrompt(string prompt)
    {
        m_prompt = prompt;
        List<int> hiddenIndexes = GenerateHiddenIndexes(m_prompt);
        m_guessLettersFactory.GenerateLetters(this, m_prompt, hiddenIndexes);
    }

    internal void DisableInput()
    {
        //throw new NotImplementedException();
    }

    public void AddPoints()
    {
        m_guessImageController.AddPoint();
    }

    private List<int> GenerateHiddenIndexes(string prompt)
    {
        List<int> hiddenIndexes = new List<int>();
        int curIndex;
        while (hiddenIndexes.Count != m_hiddenIndexesCount)
        {
            curIndex = UnityEngine.Random.Range(0, prompt.Length);
            if (prompt[curIndex] == ' ')
                continue;
            if (hiddenIndexes.Contains(curIndex))
                continue;
            hiddenIndexes.Add(curIndex);
        }
        return hiddenIndexes;
    }

}
