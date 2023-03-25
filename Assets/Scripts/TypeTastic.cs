using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypeTastic : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] possibleAnswers;
    }

    public List<Question> questions = new List<Question>();
    public TextMeshProUGUI questionText;
    public TMP_InputField answerInput;
    public Button submitButton;
    public TextMeshProUGUI resultText;

    private Question currentQuestion;
    private int playerScore;
    private int totalQuestions;
    

    void Start()
    {
        submitButton.onClick.AddListener(CheckAnswer);
        totalQuestions = questions.Count;
        LoadQuestion();
    }

    void LoadQuestion()
    {
        resultText.gameObject.SetActive(false);

        if (questions.Count > 0)
        {
            int randomIndex = Random.Range(0, questions.Count);
            currentQuestion = questions[randomIndex];
            questions.RemoveAt(randomIndex);

            questionText.text = currentQuestion.questionText;
            answerInput.text = "";
        }
        else
        {
            questionText.text = $"You scored {playerScore} of {totalQuestions}";
            answerInput.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);
        }
    }

    void CheckAnswer()
    {
        string playerAnswer = answerInput.text.Trim().ToLower();
        string[] correctAnswers = currentQuestion.possibleAnswers;

        bool isCorrect = false;

        foreach (string correctAnswer in correctAnswers)
        {
            string lowerCaseCorrectAnswer = correctAnswer.ToLower();
            string[] correctWords = lowerCaseCorrectAnswer.Split(' ');

            foreach (string word in correctWords)
            {
                if (playerAnswer.Contains(word))
                {
                    isCorrect = true;
                    break;
                }
            }

            if (isCorrect)
            {
                break;
            }
        }

        if (isCorrect)
        {
            playerScore++;
            resultText.text = "Correct!";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = "Incorrect!";
            resultText.color = Color.red;
        }

        resultText.gameObject.SetActive(true);
        StartCoroutine(NextQuestion());
    }



    IEnumerator NextQuestion()
    {
        yield return new WaitForSeconds(2);
        LoadQuestion();
    }
}
