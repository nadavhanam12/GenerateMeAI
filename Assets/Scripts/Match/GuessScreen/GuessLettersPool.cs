using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessLettersPool : MonoBehaviour
{
    [SerializeField] Transform m_letters;
    [SerializeField] Transform m_prompt;
    [SerializeField] Letter m_letterPrefab;
    [SerializeField] Letter m_spaceLetterPrefab;
    [SerializeField] MissingLetter m_missingLetterPrefab;
    [SerializeField] DragableLetter m_dragableLetterPrefab;

    Queue<Letter> m_lettersQueue = new Queue<Letter>();
    Queue<MissingLetter> m_missingLettersQueue = new Queue<MissingLetter>();
    Queue<DragableLetter> m_dragableLettersQueue = new Queue<DragableLetter>();
    Queue<Letter> m_spaceLettersQueue = new Queue<Letter>();

    void Start()
    {
        Letter newLetter;
        DragableLetter newDragableLetter;
        MissingLetter newMissingLetter;

        for (int i = 0; i < 25; i++)
        {
            newLetter = Instantiate(m_letterPrefab, m_prompt);
            m_lettersQueue.Enqueue(newLetter);
        }
        for (int i = 0; i < 15; i++)
        {
            newMissingLetter = Instantiate(m_missingLetterPrefab, m_prompt);
            m_missingLettersQueue.Enqueue(newMissingLetter);
            newDragableLetter = Instantiate(m_dragableLetterPrefab, m_letters);
            m_dragableLettersQueue.Enqueue(newDragableLetter);
        }
        for (int i = 0; i < 10; i++)
        {
            newLetter = Instantiate(m_spaceLetterPrefab, m_prompt);
            m_spaceLettersQueue.Enqueue(newLetter);
        }

    }
    public Letter GetLetter()
    {
        if (m_lettersQueue.Count > 0)
            return m_lettersQueue.Dequeue();

        var newLetter = Instantiate(m_letterPrefab, m_prompt);
        return newLetter;
    }
    public MissingLetter GetMissingLetter()
    {
        if (m_missingLettersQueue.Count > 0)
            return m_missingLettersQueue.Dequeue();

        var newLetter = Instantiate(m_missingLetterPrefab, m_prompt);
        return newLetter;
    }
    public DragableLetter GetDragableLetter()
    {
        if (m_dragableLettersQueue.Count > 0)
            return m_dragableLettersQueue.Dequeue();
        var newLetter = Instantiate(m_dragableLetterPrefab, m_letters);
        return newLetter;
    }
    public Letter GetSpaceLetter()
    {
        if (m_spaceLettersQueue.Count > 0)
            return m_spaceLettersQueue.Dequeue();
        var newLetter = Instantiate(m_spaceLetterPrefab, m_prompt);
        return newLetter;
    }

    void OnDestroy()
    {
        CleanPool();
    }
    private void CleanPool()
    {
        foreach (Letter letter in m_lettersQueue)
            Destroy(letter.gameObject);
        m_lettersQueue.Clear();

        foreach (Letter letter in m_missingLettersQueue)
            Destroy(letter.gameObject);
        m_missingLettersQueue.Clear();

        foreach (Letter letter in m_dragableLettersQueue)
            Destroy(letter.gameObject);
        m_dragableLettersQueue.Clear();

        foreach (Letter letter in m_spaceLettersQueue)
            Destroy(letter.gameObject);
        m_spaceLettersQueue.Clear();
    }
}
