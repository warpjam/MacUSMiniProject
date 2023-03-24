using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] answers;
        public int correctAnswerIndex;
    }

    public List<Question> questions = new List<Question>();
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI scoreText;
    public Button restartButton;
    public Button loadMiniGameButton;


    private Question currentQuestion;
    private bool questionActive;
    private int playerScore;
    private int totalQuestions;

    void Start()
    {
        totalQuestions = questions.Count;
        LoadQuestion();
    }

    void LoadQuestion()
    {
        ResetButtonColors();
        resultText.gameObject.SetActive(false);
        if (questions.Count > 0)
        {
            int randomIndex = Random.Range(0, questions.Count);
            currentQuestion = questions[randomIndex];
            questions.RemoveAt(randomIndex);

            questionText.text = currentQuestion.questionText;
            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answers[i];
                answerButtons[i].onClick.RemoveAllListeners();
                int currentIndex = i;
                answerButtons[i].onClick.AddListener(() => CheckAnswer(currentIndex));
            }

            questionActive = true;
        }
        else
        {
            questionText.text = "Quiz completed!";
            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void CheckAnswer(int selectedIndex)
    {
        if (questionActive)
        {
            questionActive = false;

            resultText.gameObject.SetActive(true);

            if (selectedIndex == currentQuestion.correctAnswerIndex)
            {
                answerButtons[selectedIndex].GetComponent<Image>().color = Color.green;
                resultText.text = "Correct!";
                resultText.color = Color.green;
                playerScore++;
            }
            else
            {
                answerButtons[selectedIndex].GetComponent<Image>().color = Color.red;
                resultText.text = "Incorrect!";
                resultText.color = Color.red;
            }

            StartCoroutine(NextQuestion());
        }
    }
    

    IEnumerator NextQuestion()
    {
        yield return new WaitForSeconds(1);

        if (questions.Count > 0)
        {
            LoadQuestion();
        }
        else
        {
            // All questions have been answered, show the final score
            scoreText.gameObject.SetActive(true);
            scoreText.text = $"You scored {playerScore} of {totalQuestions}";

            // Activate the buttons
            restartButton.gameObject.SetActive(true);
            loadMiniGameButton.gameObject.SetActive(true);
        }
    }



    
    private void ResetButtonColors()
    {
        foreach (Button button in answerButtons)
        {
            button.GetComponent<Image>().color = Color.white;
        }
    }

    public void RestartQuiz()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void LoadMiniGame()
    {
        SceneManager.LoadScene(1); 
    }

}
