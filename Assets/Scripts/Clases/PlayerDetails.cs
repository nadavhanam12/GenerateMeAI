using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDetails
{
    public int PlayerId { get; set; }
    public string PlayerName { get; set; }
    public Texture PlayerIcon { get; set; }

    public PlayerDetails(int id, string name, Texture icon)
    {
        PlayerId = id;
        PlayerName = name;
        PlayerIcon = icon;
    }

}
