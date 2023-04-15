using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessPromptController : MonoBehaviour
{
    GuessImageController m_guessImageController;
    [SerializeField] GuessLettersFactory m_guessLettersFactory;
    [SerializeField] int m_hiddenIndexesCount = 5;
    [SerializeField] float m_releaseRnage = 10f;
    List<MissingLetter> m_missingLetters;
    string m_prompt;
    public void Init(GuessImageController guessImageController)
    {
        m_guessImageController = guessImageController;
    }

    public void SetPrompt(string prompt)
    {
        m_prompt = prompt;
        List<int> hiddenIndexes = GenerateHiddenIndexes(m_prompt);
        m_missingLetters = m_guessLettersFactory.GenerateLetters(this, m_prompt, hiddenIndexes);
    }

    public void AddPoints()
    {
        m_guessImageController.AddPoint();
    }

    internal void OnLetterRelease(DragableLetter dragableLetter)
    {
        bool isReleaseOnLetter = false;
        //print(releasePos);
        foreach (MissingLetter missingLetter in m_missingLetters)
        {
            if (IsInRange(missingLetter, dragableLetter))
            {
                isReleaseOnLetter = true;
                if (missingLetter.GetLetter() == dragableLetter.GetLetter())
                {
                    //letters match
                    missingLetter.Reveal();
                    dragableLetter.Destroy();
                    AddPoints();
                }
                else
                {
                    //letters dont match
                    missingLetter.FailTry();
                    dragableLetter.InitPosition();
                }
                break;
            }
        }

        if (!isReleaseOnLetter)
            dragableLetter.InitPosition();
    }

    private bool IsInRange(MissingLetter missingLetter, DragableLetter dragableLetter)
    {
        Vector2 missingLetterPos = missingLetter.transform.position;
        Vector2 dragableLetterPos = dragableLetter.transform.position;
        float distance = Vector2.Distance(missingLetterPos, dragableLetterPos);
        if (distance < m_releaseRnage)
            return true;
        return false;
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
