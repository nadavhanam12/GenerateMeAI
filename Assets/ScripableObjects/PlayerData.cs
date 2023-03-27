using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private string Name;
    [SerializeField] private string Prompt;
    [SerializeField] private Texture2D GeneratedPicture;
    [SerializeField] private int Points;
    [SerializeField] private Color Color;


    public void SetPlayerData(string name, string prompt, Texture2D generatedPicture)
    {
        Name = name;
        Prompt = prompt;
        GeneratedPicture = generatedPicture;
        Points = 0;
    }

    public void AddPoints(int points)
    {
        Points += points;
    }


}
