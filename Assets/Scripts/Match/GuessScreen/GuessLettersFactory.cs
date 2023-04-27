using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessLettersFactory : MonoBehaviour
{
    [SerializeField] Canvas m_canvas;
    [SerializeField] GuessPromptController m_controller;
    [SerializeField] GuessLettersPool m_pool;
    [SerializeField] RectTransform m_promptTransform;
    [SerializeField] RectTransform m_dragableLettersTransform;
    [SerializeField] Vector2 m_cellSize;
    private List<MissingLetter> m_missingLetterList;
    private List<char> m_dragableChars;

    public List<MissingLetter> GenerateLetters(
        GuessPromptController Controller,
        string prompt,
        List<int> hiddenLetters)
    {
        m_missingLetterList = new List<MissingLetter>();
        m_dragableChars = new List<char>();

        UpdateCellSize(prompt.Length);
        Letter newLetter;
        //iterate every letter in prompt
        for (int i = 0; i < prompt.Length; i++)
        {
            //if letter should be hidden
            if (hiddenLetters.Contains(i))
                newLetter = GenerateMissingLetter(prompt[i]);
            else if (prompt[i] == ' ')
                newLetter = GenerateSpaceLetter(prompt[i]);
            else
                newLetter = GenerateLetter(prompt[i]);

            newLetter.transform.SetSiblingIndex(i);
        }

        GenearetDragablesChars();
        return m_missingLetterList;

    }

    private void UpdateCellSize(int promptLength)
    {
        GridLayoutGroup gridLayoutGroup = m_promptTransform.GetComponent<GridLayoutGroup>();
        //update prompt cell size
        float width = m_promptTransform.rect.width - gridLayoutGroup.padding.left - gridLayoutGroup.padding.right;
        float cellWidth = width / (promptLength + 2) * 2;
        Vector2 newSize = m_cellSize;
        m_promptTransform.GetComponent<GridLayoutGroup>().cellSize = newSize;

        //update dragable letters cell size
        m_dragableLettersTransform.GetComponent<GridLayoutGroup>().cellSize = newSize;

    }


    private Letter GenerateLetter(char c)
    {
        Letter letter = m_pool.GetLetter();
        // letter.transform.SetParent(m_promptTransform);
        // letter.transform.localScale = Vector3.one;
        letter.Init(c);
        return letter;

    }

    private Letter GenerateSpaceLetter(char c)
    {
        Letter letter = m_pool.GetSpaceLetter();
        // letter.transform.SetParent(m_promptTransform);
        // letter.transform.localScale = Vector3.one;
        letter.Init(c);
        return letter;
    }
    private Letter GenerateMissingLetter(char c)
    {
        MissingLetter missingLetter = m_pool.GetMissingLetter();
        // missingLetter.transform.SetParent(m_promptTransform);
        // missingLetter.transform.localScale = Vector3.one;
        missingLetter.Init(c);

        m_missingLetterList.Add(missingLetter);
        m_dragableChars.Add(c);
        return missingLetter;
    }
    private void GenearetDragablesChars()
    {
        if (m_dragableChars == null) return;
        Shuffle(m_dragableChars);
        foreach (char c in m_dragableChars)
            GenerateDragableLetter(c);
    }
    private void GenerateDragableLetter(char c)
    {
        DragableLetter dragableLetter = m_pool.GetDragableLetter();
        // dragableLetter.transform.SetParent(m_dragableLettersTransform);
        // dragableLetter.transform.localScale = Vector3.one;
        dragableLetter.Init(m_controller, m_canvas, c);
    }
    public void Shuffle(List<char> list)
    {
        System.Random random = new System.Random();
        // Loop through the list and swap each item with a random item
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = random.Next(i, list.Count);
            char temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
