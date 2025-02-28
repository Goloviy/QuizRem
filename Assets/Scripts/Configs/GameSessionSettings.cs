using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Game Session Settings", menuName ="Gameplay/GameSessionSettings")]
public class GameSessionSettings : BaseConfig
{
    public GameSettingsData data;
}

[Serializable]
public struct GameSettingsData
{
    public float mainTimerTime;   
    public float answerDelayTime;
    public float showAnswersDelay;
    public List<QuestionByDifficulty> sessionQuestions;
}

[Serializable]
public class QuestionByDifficulty
{
    public int difficulty;
    public int count;
}
