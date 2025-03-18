using System.Collections.Generic;
using Firebase.Firestore;

[System.Serializable]
[FirestoreData]
public class BonusType
{
    [FirestoreProperty]
    public string BonusName { get; set; }

    [FirestoreProperty]
    public int BonusCount { get; set; }

    [FirestoreProperty]
    public bool IsBonusActive { get; set; }

    public BonusType() { }

    public BonusType(string bonusName, int bonusCount = 0, bool isBonusActive = false)
    {
        BonusName = bonusName;
        BonusCount = bonusCount;
        IsBonusActive = isBonusActive;
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
    {
        { "BonusName", BonusName },
        { "BonusCount", BonusCount },
        { "IsBonusActive", IsBonusActive }
    };
    }

}

