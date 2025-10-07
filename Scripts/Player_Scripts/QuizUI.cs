using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizUI : MonoBehaviour
{
    public static QuizUI Instance;

    [Header("UI References")]
    public GameObject quizPanel;
    public Text questionText;
    public Button[] answerButtons;
    public Text resultText;
    public GameObject resultPanel;

    private QuizQuestion currentQuestion;
    private bool hasAnswered = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (quizPanel != null)
        {
            quizPanel.SetActive(false);
        }

        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }

    public void ShowQuiz()
    {
        if (quizPanel == null || VietnameseQuizData.Instance == null)
        {
            Debug.LogError("Quiz Panel or Quiz Data is NULL!");
            return;
        }

        currentQuestion = VietnameseQuizData.Instance.GetRandomQuestion();

        if (currentQuestion == null)
        {
            Debug.LogError("No question received!");
            return;
        }

        hasAnswered = false;

        questionText.text = currentQuestion.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.answers.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                Text buttonText = answerButtons[i].GetComponentInChildren<Text>();
                buttonText.text = currentQuestion.answers[i];

                int answerIndex = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(answerIndex));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }

        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }

        quizPanel.SetActive(true);
    }

    void OnAnswerSelected(int answerIndex)
    {
        if (hasAnswered) return;

        hasAnswered = true;

        bool isCorrect = answerIndex == currentQuestion.correctAnswerIndex;

        if (isCorrect)
        {
            resultText.text = "Dung roi! +1 Upgrade";
            resultText.color = Color.green;

            StartCoroutine(ShowResultThenUpgrade());
        }
        else
        {
            resultText.text = "Sai roi! Khong duoc nang cap";
            resultText.color = Color.red;

            StartCoroutine(ShowResultThenContinue());
        }

        resultPanel.SetActive(true);
    }

    IEnumerator ShowResultThenUpgrade()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        quizPanel.SetActive(false);

        if (LevelUpUI.Instance != null)
        {
            LevelUpUI.Instance.ShowUpgradeOptions();
        }
    }

    IEnumerator ShowResultThenContinue()
    {
        yield return new WaitForSecondsRealtime(2f);

        quizPanel.SetActive(false);
        Time.timeScale = 1;
    }
}