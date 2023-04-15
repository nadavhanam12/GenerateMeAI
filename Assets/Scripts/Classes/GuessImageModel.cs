using System;
using UnityEngine;

[Serializable]
public class GuessImageModel
{
    public string PromptText;
    public Texture Image;

    public GuessImageModel(string promptText, Texture image)
    {
        PromptText = promptText;
        Image = image;
    }

}
