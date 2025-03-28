using UnityEngine;
using System;
using System.Collections.Generic;
using Firebase.Firestore;

[System.Serializable]
[FirestoreData]
public class UserData
{
    [FirestoreProperty]
    public string UserId { get; set; }

    [FirestoreProperty]
    public string NickName { get; set; }

    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public string Email { get; set; }

    [FirestoreProperty]
    public string ProfileImageUrl { get; set; }

    [FirestoreProperty]
    public int Score { get; set; }

    [FirestoreProperty]
    public int WeekScore { get; set; }

    [FirestoreProperty]
    public int Progress { get; set; }

    [FirestoreProperty]
    public Timestamp CreatedTime { get; set; }

    [FirestoreProperty]
    public Dictionary<string, List<int>> AnsweredQuestions { get; set; }

    [FirestoreProperty]
    public bool IsUserRegistered { get; set; }

    public UserData()
    {
        AnsweredQuestions = new Dictionary<string, List<int>>();
        IsUserRegistered = false;
    }

    public UserData(string userId, string nickName, string name, string email,
               string profileImageUrl = null, int score = 0, int weekScore = 0, int progress = 0,
               bool isRegistered = false)
    {
        UserId = userId;
        NickName = nickName;
        Name = name;
        Email = email;
        ProfileImageUrl = profileImageUrl;
        Score = score;
        WeekScore = weekScore;
        Progress = progress;
        CreatedTime = Timestamp.FromDateTime(DateTime.UtcNow);
        IsUserRegistered = isRegistered;
        AnsweredQuestions = new Dictionary<string, List<int>>();
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            { "AnsweredQuestions", AnsweredQuestions },
            { "UserId", UserId },
            { "NickName", NickName },
            { "Name", Name },
            { "Email", Email },
            { "ProfileImageUrl", ProfileImageUrl },
            { "Score", Score },
            { "WeekScore", WeekScore },
            { "Progress", Progress },
            { "CreatedTime", CreatedTime },
            { "IsUserRegistered", IsUserRegistered }
        };
    }

    public void SetUserRegistered(bool registered)
    {
        IsUserRegistered = registered;
    }

    public DateTime GetCreatedDateTime()
    {
        return CreatedTime.ToDateTime();
    }

    public string GetFormattedCreatedTime()
    {
        return GetCreatedDateTime().ToLocalTime().ToString("dd/MM/yyyy");
    }

    public string GetFormattedCreatedTimeWithHour()
    {
        return GetCreatedDateTime().ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss");
    }

}

public static class UserDataStore
{
    private static UserData _currentUserData;
    public static event Action<UserData> OnUserDataChanged;

    public static UserData CurrentUserData
    {
        get => _currentUserData;
        set
        {
            _currentUserData = value;
            OnUserDataChanged?.Invoke(_currentUserData);
            Debug.Log($"UserDataStore atualizado para usuário: {_currentUserData?.UserId}, Score: {_currentUserData?.Score}, WeekScore: {_currentUserData?.WeekScore}");
        }
    }

    public static void UpdateScore(int newScore)
    {
        if (_currentUserData != null)
        {
            _currentUserData.Score = newScore;
            OnUserDataChanged?.Invoke(_currentUserData);
            Debug.Log($"Score atualizado para: {newScore}");
        }
    }

    public static void UpdateWeekScore(int newWeekScore)
    {
        if (_currentUserData != null)
        {
            _currentUserData.WeekScore = newWeekScore;
            OnUserDataChanged?.Invoke(_currentUserData);
            Debug.Log($"WeekScore atualizado para: {newWeekScore}");
        }
    }

    public static void AddScore(int additionalScore)
    {
        if (_currentUserData != null)
        {
            _currentUserData.Score += additionalScore;
            _currentUserData.WeekScore += additionalScore; // Also update week score when adding points
            OnUserDataChanged?.Invoke(_currentUserData);
            Debug.Log($"Score incrementado em {additionalScore}. Novo Score: {_currentUserData.Score}, Novo WeekScore: {_currentUserData.WeekScore}");
        }
    }

    public static void UpdateUserData(UserData userData)
    {
        if (userData != null)
        {
            _currentUserData = userData;
            OnUserDataChanged?.Invoke(_currentUserData);
            Debug.Log($"UserDataStore atualizado para usuário: {_currentUserData?.UserId}, Score: {_currentUserData?.Score}, WeekScore: {_currentUserData?.WeekScore}");
        }
    }

}
