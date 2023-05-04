using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessPromptController : MonoBehaviour
{
    GuessImageController m_guessImageController;
    [SerializeField] GuessLettersFactory m_guessLettersFactory;
    [SerializeField][Range(0, 1)] float m_hiddenIndexesPrecentage = 0.5f;
    [SerializeField][Range(0, 1)] int m_hiddenIndexesMinimum = 5;

    [SerializeField] float m_releaseRange = 10f;
    List<MissingLetter> m_missingLetters;
    string m_prompt;
    bool enableInput;
    [SerializeField] Image m_turnIndicator;
    public void Init(GuessImageController guessImageController)
    {
        m_guessImageController = guessImageController;
        enableInput = true;
        m_turnIndicator.gameObject.SetActive(false);
    }

    public void SetPrompt(string prompt)
    {
        m_prompt = prompt;
        List<int> hiddenIndexes = GetRandomNonSpaceIndexes(m_prompt);
        m_missingLetters = m_guessLettersFactory.GenerateLetters(this, m_prompt, hiddenIndexes);
    }

    public void AddPoints()
    {
        m_guessImageController.AddPoint();
    }
    internal void ToggleInput(bool isOn)
    {
        enableInput = isOn;
        m_turnIndicator.gameObject.SetActive(isOn);
    }

    internal void OnLetterRelease(DragableLetter dragableLetter)
    {
        bool isReleaseOnLetter = false;
        //print(releasePos);
        if (enableInput)
            foreach (MissingLetter missingLetter in m_missingLetters)
            {
                if (IsInRange(missingLetter, dragableLetter))
                {
                    if (missingLetter.GetLetter() == dragableLetter.GetLetter())
                    {
                        //letters match
                        missingLetter.Reveal();
                        dragableLetter.Destroy();
                        AddPoints();
                        isReleaseOnLetter = true;
                        break;
                    }
                    else
                    {
                        //letters dont match
                        missingLetter.FailTry();
                    }

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
        if (distance < m_releaseRange)
            return true;
        return false;
    }

    List<int> GetRandomNonSpaceIndexes(string prompt)
    {
        List<int> nonSpaceIndexes = new List<int>();
        for (int i = 0; i < prompt.Length; i++)
        {
            if (prompt[i] != ' ')
            {
                nonSpaceIndexes.Add(i);
            }
        }

        List<int> randomIndexes = new List<int>();
        System.Random rand = new System.Random();

        int count = (int)(nonSpaceIndexes.Count * m_hiddenIndexesPrecentage);
        if (count < m_hiddenIndexesMinimum)
            count = m_hiddenIndexesMinimum;

        for (int i = 0; i < count; i++)
        {
            int randomIndex = nonSpaceIndexes[rand.Next(nonSpaceIndexes.Count)];
            nonSpaceIndexes.Remove(randomIndex);
            randomIndexes.Add(randomIndex);
        }

        return randomIndexes;
    }
}
