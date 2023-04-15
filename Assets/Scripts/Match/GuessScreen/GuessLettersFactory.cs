using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessLettersFactory : MonoBehaviour
{
    [SerializeField] Canvas m_canvas;
    [SerializeField] GuessPromptController m_controller;

    [SerializeField] RectTransform m_promptTransform;
    [SerializeField] RectTransform m_dragableLettersTransform;
    [SerializeField] Letter m_letterPrefab;
    [SerializeField] Letter m_spaceLetterPrefab;
    [SerializeField] MissingLetter m_missingLetterPrefab;
    [SerializeField] DragableLetter m_dragableLetterPrefab;

    private List<MissingLetter> m_missingLetterList;
    private List<char> m_dragableChars;

    public List<MissingLetter> GenerateLetters(
        GuessPromptController Controller,
        string promp,
        List<int> hiddenLetters)
    {
        m_missingLetterList = new List<MissingLetter>();
        m_dragableChars = new List<char>();

        UpdateCellSize(promp.Length);
        //iterate every letter in prompt
        for (int i = 0; i < promp.Length; i++)
        {
            //if letter should be hidden
            if (hiddenLetters.Contains(i))
                GenerateMissingLetter(promp[i]);
            else if (promp[i] == ' ')
                GenerateSpaceLetter(promp[i]);
            else
                GenerateLetter(promp[i]);

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
        Vector2 newSize = new Vector2(cellWidth, 150);
        m_promptTransform.GetComponent<GridLayoutGroup>().cellSize = newSize;

        //update dragable letters cell size
        m_dragableLettersTransform.GetComponent<GridLayoutGroup>().cellSize = newSize;

    }


    private void GenerateLetter(char c)
    {
        Letter letter = Instantiate(m_letterPrefab, m_promptTransform, false);
        letter.transform.localScale = Vector3.one;
        letter.Init(c);
    }

    private void GenerateSpaceLetter(char c)
    {
        Letter letter = Instantiate(m_spaceLetterPrefab, m_promptTransform, false);
        letter.transform.localScale = Vector3.one;
        //letter.Init(c);
    }
    private void GenerateMissingLetter(char c)
    {
        MissingLetter missingLetter = Instantiate(m_missingLetterPrefab, m_promptTransform, false);
        missingLetter.transform.localScale = Vector3.one;
        missingLetter.Init(c);
        m_missingLetterList.Add(missingLetter);
        m_dragableChars.Add(c);
    }
    private void GenearetDragablesChars()
    {
        Shuffle(m_dragableChars);
        foreach (char c in m_dragableChars)
            GenerateDragableLetter(c);
    }
    private void GenerateDragableLetter(char c)
    {
        DragableLetter dragableLetter = Instantiate(m_dragableLetterPrefab, m_dragableLettersTransform, false);
        dragableLetter.transform.localScale = Vector3.one;
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
