using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuizQuestion
{
    public string question;
    public string[] answers;
    public int correctAnswerIndex;
}

public class VietnameseQuizData : MonoBehaviour
{
    public static VietnameseQuizData Instance;

    private List<QuizQuestion> allQuestions = new List<QuizQuestion>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeQuestions();
    }

    void InitializeQuestions()
    {
        // Lich su co ban
        allQuestions.Add(new QuizQuestion
        {
            question = "Vi vua nao lap ra nha Ly?",
            answers = new string[] { "Ly Cong Uan", "Ly Thai To", "Ly Thanh Tong", "Ly Nhan Tong" },
            correctAnswerIndex = 1
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Thanh pho nao la thu do cua Viet Nam?",
            answers = new string[] { "Ha Noi", "TP. Ho Chi Minh", "Da Nang", "Hue" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Chien thang nao danh dau su sup do cua thuc dan Phap o Viet Nam?",
            answers = new string[] { "Dien Bien Phu", "Bach Dang", "Chi Lang", "Dong Da" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Ngay Quoc khanh (ngay doc lap) cua Viet Nam la ngay nao?",
            answers = new string[] { "2/9/1945", "30/4/1975", "1/1/1954", "19/8/1945" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Ai la anh hung dan toc lanh dao khang chien chong quan Nguyen Mong?",
            answers = new string[] { "Tran Hung Dao", "Ngo Quyen", "Le Loi", "Quang Trung" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Chien thang Bach Dang nam 938 do ai chi huy?",
            answers = new string[] { "Ngo Quyen", "Tran Hung Dao", "Le Loi", "Ly Thuong Kiet" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Ten goi cu cua Viet Nam la gi?",
            answers = new string[] { "An Nam", "Dai Viet", "Giao Chi", "Ca ba dap an tren" },
            correctAnswerIndex = 3
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Vi vua nao lap ra nha Tran?",
            answers = new string[] { "Tran Thai Tong", "Tran Nhan Tong", "Tran Hung Dao", "Tran Thanh Tong" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Cuoc khoi nghia Lam Son do ai lanh dao?",
            answers = new string[] { "Le Loi", "Nguyen Trai", "Le Lai", "Le Thai To" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Ngoi chua nao noi tieng o Ninh Binh?",
            answers = new string[] { "Chua Bai Dinh", "Chua Huong", "Chua Mot Cot", "Chua Thien Mu" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Mon an truyen thong cua Viet Nam la gi?",
            answers = new string[] { "Pho", "Banh mi", "Bun cha", "Tat ca deu dung" },
            correctAnswerIndex = 3
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Ao dai la trang phuc truyen thong cua nuoc nao?",
            answers = new string[] { "Viet Nam", "Trung Quoc", "Nhat Ban", "Han Quoc" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Ten that cua Chu tich Ho Chi Minh la gi?",
            answers = new string[] { "Nguyen Ai Quoc", "Nguyen Sinh Cung", "Ho Chi Minh", "Tat ca deu dung" },
            correctAnswerIndex = 3
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Vinh Ha Long thuoc tinh nao?",
            answers = new string[] { "Quang Ninh", "Hai Phong", "Ninh Binh", "Thanh Hoa" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Tet Nguyen Dan la tet gi?",
            answers = new string[] { "Tet Co truyen", "Tet Trung Thu", "Tet Doan Ngo", "Tet Han Thuc" },
            correctAnswerIndex = 0
        });
    }

    public QuizQuestion GetRandomQuestion()
    {
        if (allQuestions.Count == 0)
        {
            Debug.LogError("No questions available!");
            return null;
        }

        int randomIndex = Random.Range(0, allQuestions.Count);
        return allQuestions[randomIndex];
    }
}
