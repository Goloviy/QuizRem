using System.Collections.Generic;
using UnityEngine;

public class Question
{
    public int id;

    public string questionText;

    public List<AnswerData> answers = new List<AnswerData>();
    public List<int> removedAnswers = new List<int>();

    public int questionDifficulty;

    public bool sortAnswers;
    public int correctAnswerIndex;
    public KnowledgeCategory category;
    public Sprite categoryIcon;
}