using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessLettersFactory : MonoBehaviour
{
    [SerializeField] ExistingLetter ExistingLetterPrefab;
    [SerializeField] MissingLetter MissingLetterPrefab;

    public void GenerateLetters(
        GuessPromptController Controller,
        string promp,
        List<int> hiddenLetters)
    {

        print(promp);
        print(hiddenLetters.ToString());


    }


}
