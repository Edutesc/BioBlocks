using UnityEngine;

[System.Serializable]
public class Player
{
    public string name { get; set; }
    public string id { get; set; }
    public int score { get; set; }

    // Parameterless constructor (useful for serialization)
    public Player() {}

    // Constructor with parameters
    public Player(string name, string id, int score)
    {
        this.name = name;
        this.id = id;
        this.score = score;
    }
}