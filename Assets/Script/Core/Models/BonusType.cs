using System;
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
    
    [FirestoreProperty]
    public long ExpirationTimestamp { get; set; }
    
    [FirestoreProperty]
    public bool IsPersistent { get; set; }

    public BonusType() { }

    public BonusType(string bonusName, int bonusCount = 0, bool isBonusActive = false, long expirationTimestamp = 0, bool isPersistent = false)
    {
        BonusName = bonusName;
        BonusCount = bonusCount;
        IsBonusActive = isBonusActive;
        ExpirationTimestamp = expirationTimestamp;
        IsPersistent = isPersistent;
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            { "BonusName", BonusName },
            { "BonusCount", BonusCount },
            { "IsBonusActive", IsBonusActive },
            { "ExpirationTimestamp", ExpirationTimestamp },
            { "IsPersistent", IsPersistent }
        };
    }
    
    public bool IsExpired()
    {
        if (ExpirationTimestamp <= 0)
            return false; 
            
        long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        return currentTimestamp >= ExpirationTimestamp;
    }
    
    public long GetRemainingSeconds()
    {
        if (ExpirationTimestamp <= 0)
            return 0;
            
        long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        return Math.Max(0, ExpirationTimestamp - currentTimestamp);
    }
    
    public void SetExpirationFromDuration(float durationInSeconds)
    {
        ExpirationTimestamp = DateTimeOffset.UtcNow.AddSeconds(durationInSeconds).ToUnixTimeSeconds();
    }
}

