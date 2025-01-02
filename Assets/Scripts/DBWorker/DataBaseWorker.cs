using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataBaseWorker 
{
    private const int CATEGORY = 0;
    private const int QUESTION_ID = 1;
    private const int ANSWERS_NEED_SORTING = 2;
    private const int RIGHT_ANSWER_INDEX = 3;
    private const int QUESTION_TEXT = 4;
    private const int FIRST_ANSWER_INDEX = 6;
    private const int DIFFICULTY = 14;

    private Dictionary<int, List<Question>> questions = new Dictionary<int, List<Question>>();
    private Dictionary<int, Question> questionsByID = new Dictionary<int, Question>();
    private Dictionary<int, Question> fastQuestions = new Dictionary<int, Question>();
    private Dictionary<int, List<Question>> tutorialQuestions = new Dictionary<int, List<Question>>();
    private Dictionary<int, Question> tutorialQuestionsById = new Dictionary<int, Question>();

    private readonly string questionList;
    private readonly string fastGamequestionList;
    private readonly string tutorialQuestionList;
    
    private List<int> idQuestions;
    private bool isParsingComplete;

    private Action onDatabaseReady;

    private Dictionary<KnowledgeCategory, Sprite> categoryIcons;
    
    public DataBaseWorker(string _questionList, string _fastGameQuestions,string _tutorialQuestions, Action _callback, List<KnowledgeCategoryIcon> _categoryIcons)
    {
        //categoryIcons = new Dictionary<KnowledgeCategory, Sprite>(_categoryIcons.Count);
        
        // for (int i = 0; i < _categoryIcons.Count; i++)
        // {
        //     categoryIcons.Add(_categoryIcons[i].category, _categoryIcons[i].icon);
        // }
        
        questionList = _questionList;
        fastGamequestionList = _fastGameQuestions;
        tutorialQuestionList = _tutorialQuestions;
        
        onDatabaseReady = _callback;
        
        CreateQuestionsAsync();
    }
    
     public void RemoveAnsweredQuestions(List<int> _questions)
    {
        for (var i = 0; i < _questions.Count; i++)
        {
            RemoveAnsweredQuestions(_questions[i]);
        }
    }

    public void RemoveAnsweredQuestions(int _questionID)
    {
        if (questionsByID.ContainsKey(_questionID))
        {
            RemoveAnsweredQuestions(questionsByID[_questionID]);
        }
    }
    
    public void RemoveAnsweredQuestions(List<Question> _questions)
    {
        for (var i = 0; i < _questions.Count; i++)
        {
            RemoveAnsweredQuestions(_questions[i]);
        }
    }

    public void RemoveAnsweredQuestions(Question _questions)
    {
        if (questions[_questions.questionDifficulty].Contains(_questions))
        {
            questions[_questions.questionDifficulty].Remove(_questions);
        }
    }

    public void RemoveFastQuestion(List<int> _idList)
    {
        for (var i = 0; i < _idList.Count; i++)
        {
            RemoveFastQuestion(_idList[i]);
        }
    }

    public void RemoveFastQuestion(int _id)
    {
        if(fastQuestions.ContainsKey(_id))
        {
            fastQuestions.Remove(_id);
        }
    }

    public Question GetFastQuestion()
    {
        var questions = fastQuestions.Values.ToList();
        return questions[Random.Range(0, questions.Count)];
    }

    /// <summary>
    /// Creates "List<Question>" certain difficult.
    /// </summary>
    /// <param name="_difficultyLevel"></Difficulty level of the question is from 1 to 5>
    /// <param name="_count"></number of questions to be returned>
    /// <param name="_idQuetions"></id Question>
    /// <returns></returns>
    public List<Question> GetQuestions(int _difficultyLevel,QuestionType _questionType, int _count = 0)
    {
        if (!isParsingComplete)
        {
            Debug.LogError("Database still in progress.");
            return new List<Question>();
        }

        switch (_questionType)
        {
            case QuestionType.QuestionMain:
            {
                _difficultyLevel = Mathf.Clamp(_difficultyLevel, 0, 5);
                _count = Mathf.Clamp(_count, 0, questions[_difficultyLevel].Count);        
                //List<Question> questionsLevel = new List<Question>();

                var questionList = new List<Question>(questions[_difficultyLevel]);
                var result = new List<Question>(_count);
                questionList.Shuffle();
                for (int i = 0; i < _count; i++)
                {
                    var index = UnityEngine.Random.Range(0, questionList.Count);
                    result.Add(questionList[index]);
                    questionList.RemoveAt(index);
                }
                return result;
            }
            //TODO: needed for tutorial.
            case QuestionType.QuestionTutorial:
            {
                _difficultyLevel = Mathf.Clamp(_difficultyLevel, 0, 5);
                _count = Mathf.Clamp(_count, 0, tutorialQuestions[_difficultyLevel].Count);        
                //List<Question> questionsLevel = new List<Question>();

                var questionList = new List<Question>(tutorialQuestions[_difficultyLevel]);
                var result = new List<Question>(_count);
                //questionList.Shuffle();
                for (int i = 0; i < _count; i++)
                {
                    //var index = UnityEngine.Random.Range(0, questionList.Count);
                    result.Add(questionList[0]);
                    if (tutorialQuestions[_difficultyLevel].Contains(questionList[0]))
                    {
                        tutorialQuestions[_difficultyLevel].Remove(questionList[0]);
                    }
                    questionList.RemoveAt(0);
                }
                return result;
            }
            default:
                return new List<Question>();;
        }
    }
    /// <summary>
    /// Asynchronous start "GreateQuestions".
    /// </summary>
    private async void CreateQuestionsAsync()
    {
        await Task.Run(CreateQuestions);
        onDatabaseReady?.Invoke();
    }

    /// <summary>
    /// Fills "Dictionary<int, List<Question>>" and creates "List<Question>" five difficulties.
    /// </summary>
    private void CreateQuestions()
    {
        var lines = new List<string>(questionList.Split('\n'));

        for (int i = 0; i < lines.Count; i++)
        {
            var newQuestion = ParseQuestionString(lines[i]);
            var difficulty = newQuestion.questionDifficulty;

            if (questions.ContainsKey(difficulty))
            {
                questions[difficulty].Add(newQuestion);
            }
            else
            {
                questions.Add(difficulty, new List<Question>());
                questions[difficulty].Add(newQuestion);
            }
            
            questionsByID.Add(newQuestion.id, newQuestion);
        }

        lines = new List<string>(fastGamequestionList.Split('\n'));
        
        for (int i = 0; i < lines.Count; i++)
        {
            var newQuestion = ParseQuestionString(lines[i]);
            
            fastQuestions.Add(newQuestion.id, newQuestion);
        }

        //TODO: needed for tutorial.
        lines = new List<string>(tutorialQuestionList.Split('\n'));
        
        for (int i = 0; i < lines.Count; i++)
        {
            var newQuestion = ParseQuestionString(lines[i]);
            var difficulty = newQuestion.questionDifficulty;

            if (tutorialQuestions.ContainsKey(difficulty))
            {
                tutorialQuestions[difficulty].Add(newQuestion);
            }
            else
            {
                tutorialQuestions.Add(difficulty, new List<Question>());
                tutorialQuestions[difficulty].Add(newQuestion);
            }
            tutorialQuestionsById.Add(newQuestion.id, newQuestion);
        }
        
        isParsingComplete = true;
    }

    private Question ParseQuestionString(string _str)
    {
        try
        {
            var newQuestion = new Question();

            string[] questionInfo = _str.Split(',');

            int.TryParse(questionInfo[CATEGORY], out var categoryIndex);
            newQuestion.category = (KnowledgeCategory) categoryIndex;

            int.TryParse(questionInfo[QUESTION_ID], out var questionID);
            newQuestion.id = questionID;

            int.TryParse(questionInfo[ANSWERS_NEED_SORTING], out var sorting);
            newQuestion.sortAnswers = sorting == 1;

            //TODO : Useless index because of shuffling
            int.TryParse(questionInfo[RIGHT_ANSWER_INDEX], out var answerIndex);
            newQuestion.correctAnswerIndex = answerIndex - 1;

            newQuestion.questionText = questionInfo[QUESTION_TEXT].Replace(';', ',').Replace('|', '\"');
            newQuestion.answers = new List<AnswerData>(4)
            {
                new AnswerData()
                {
                    answerLabel = questionInfo[FIRST_ANSWER_INDEX].Replace(';', ',').Replace('|', '\"'),
                    answerGlobalID = questionID * 4 + 1, isRightOne = answerIndex == 1
                },
                new AnswerData()
                {
                    answerLabel = questionInfo[FIRST_ANSWER_INDEX + 2].Replace(';', ',').Replace('|', '\"'),
                    answerGlobalID = questionID * 4 + 2, isRightOne = answerIndex == 2
                },
                new AnswerData()
                {
                    answerLabel = questionInfo[FIRST_ANSWER_INDEX + 4].Replace(';', ',').Replace('|', '\"'),
                    answerGlobalID = questionID * 4 + 3, isRightOne = answerIndex == 3
                },
                new AnswerData()
                {
                    answerLabel = questionInfo[FIRST_ANSWER_INDEX + 6].Replace(';', ',').Replace('|', '\"'),
                    answerGlobalID = questionID * 4 + 4, isRightOne = answerIndex == 4
                }
            };
            var rightAnswer = newQuestion.answers[newQuestion.correctAnswerIndex];

            if (newQuestion.sortAnswers)
            {
                bool incorrectAnswers = false;
                var results = new List<Tuple<int, int>>(4);
                for (int j = 0; j < newQuestion.answers.Count; j++)
                {
                    if (!int.TryParse(newQuestion.answers[j].answerLabel, out var result))
                    {
                        incorrectAnswers = true;
                        break;
                    }

                    var indexValue = new Tuple<int, int>(result, j);
                    results.Add(indexValue);
                }

                if (!incorrectAnswers)
                {
                    results = results.OrderBy(x => x.Item1).ToList();
                    var sortedList = new List<AnswerData>(4);

                    for (int j = 0; j < results.Count; j++)
                    {
                        sortedList.Add(newQuestion.answers[results[j].Item2]);
                    }

                    newQuestion.answers = sortedList;
                }
            }
            else
            {
                newQuestion.answers.Shuffle();
            }

            newQuestion.correctAnswerIndex = newQuestion.answers.IndexOf(rightAnswer);

            int.TryParse(questionInfo[DIFFICULTY], out var difficulty);
            newQuestion.questionDifficulty = difficulty;

            //newQuestion.categoryIcon = categoryIcons[newQuestion.category];

            return newQuestion;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            throw;
        }
    }
    
  
 
    
}
