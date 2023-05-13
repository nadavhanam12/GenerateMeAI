using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessPromptController : MonoBehaviour
{
    GuessImageController m_guessImageController;
    [SerializeField] GuessLettersFactory m_guessLettersFactory;
    private int m_hiddenCharactersCount;
    [SerializeField] float m_releaseRange = 10f;
    List<MissingLetter> m_missingLetters;
    string m_prompt;
    bool m_enableInput;
    [SerializeField] Image m_turnIndicator;
    public void Init(GuessImageController guessImageController, int hiddenCharactersCount)
    {
        m_hiddenCharactersCount = hiddenCharactersCount;
        m_guessImageController = guessImageController;
        m_enableInput = true;
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
        m_enableInput = isOn;
        m_turnIndicator.gameObject.SetActive(isOn);
    }

    internal void OnLetterRelease(DragableLetter dragableLetter)
    {
        bool isReleaseOnLetter = false;
        if (m_enableInput)
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


        int hiddenCharsCount = m_hiddenCharactersCount;
        if (hiddenCharsCount > nonSpaceIndexes.Count)
            hiddenCharsCount = nonSpaceIndexes.Count;

        for (int i = 0; i < hiddenCharsCount; i++)
        {
            int randomIndex = nonSpaceIndexes[rand.Next(nonSpaceIndexes.Count)];
            nonSpaceIndexes.Remove(randomIndex);
            randomIndexes.Add(randomIndex);
        }

        return randomIndexes;
    }
}
