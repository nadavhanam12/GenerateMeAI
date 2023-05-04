using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeImages", menuName = "ScriptableObjects/ThemeImages", order = 3)]
public class ThemeImages : ScriptableObject
{
    public string ThemePromp;
    public List<GuessImageModel> Images;
}