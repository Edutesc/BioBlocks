using System;
using System.Collections.Generic;
using Firebase.Firestore;

[System.Serializable]
public class BonusType
{
    public string BonusName { get; set; }
    public int BonusCount { get; set; }
    public bool IsBonusActive { get; set; }
    public long ExpirationTimestamp { get; set; }
    public bool IsPersistent { get; set; }
    public int Multiplier { get; set; } = 1;

    public BonusType(string name, int count, bool isActive, long expiration, bool isPersistent, int multiplier = 1)
    {
        BonusName = name;
        BonusCount = count;
        IsBonusActive = isActive;
        ExpirationTimestamp = expiration;
        IsPersistent = isPersistent;
        Multiplier = multiplier;
    }

    public bool IsExpired()
    {
        if (ExpirationTimestamp <= 0) return false;
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds() >= ExpirationTimestamp;
    }

    public void SetExpirationFromDuration(float durationInSeconds)
    {
        ExpirationTimestamp = DateTimeOffset.UtcNow.AddSeconds(durationInSeconds).ToUnixTimeSeconds();
    }

    public float GetRemainingSeconds()
    {
        if (ExpirationTimestamp <= 0) return 0;
        
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long remainingTime = ExpirationTimestamp - currentTime;
        
        return remainingTime > 0 ? remainingTime : 0;
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            { "BonusName", BonusName },
            { "BonusCount", BonusCount },
            { "IsBonusActive", IsBonusActive },
            { "ExpirationTimestamp", ExpirationTimestamp },
            { "IsPersistent", IsPersistent },
            { "Multiplier", Multiplier }
        };
    }
}

