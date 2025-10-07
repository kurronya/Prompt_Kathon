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
        // Lịch sử cơ bản
        allQuestions.Add(new QuizQuestion
        {
            question = "Vị vua nào lập ra nhà Lý?",
            answers = new string[] { "Lý Công Uẩn", "Lý Thái Tổ", "Lý Thánh Tông", "Lý Nhân Tông" },
            correctAnswerIndex = 1
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Thành phố nào là thủ đô của Việt Nam?",
            answers = new string[] { "Hà Nội", "TP. Hồ Chí Minh", "Đà Nẵng", "Huế" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Chiến thắng nào đánh dấu sự sụp đổ của thực dân Pháp ở Việt Nam?",
            answers = new string[] { "Điện Biên Phủ", "Bạch Đằng", "Chi Lăng", "Đống Đa" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Ngày Quốc khánh (ngày độc lập) của Việt Nam là ngày nào?",
            answers = new string[] { "2/9/1945", "30/4/1975", "1/1/1954", "19/8/1945" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Ai là anh hùng dân tộc lãnh đạo kháng chiến chống quân Nguyên Mông?",
            answers = new string[] { "Trần Hưng Đạo", "Ngô Quyền", "Lê Lợi", "Quang Trung" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Chiến thắng Bạch Đằng năm 938 do ai chỉ huy?",
            answers = new string[] { "Ngô Quyền", "Trần Hưng Đạo", "Lê Lợi", "Lý Thường Kiệt" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Tên gọi cũ của Việt Nam là gì?",
            answers = new string[] { "An Nam", "Đại Việt", "Giao Chỉ", "Cả ba đáp án trên" },
            correctAnswerIndex = 3
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Vị vua nào lập ra nhà Trần?",
            answers = new string[] { "Trần Thái Tông", "Trần Nhân Tông", "Trần Hưng Đạo", "Trần Thánh Tông" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Cuộc khởi nghĩa Lam Sơn do ai lãnh đạo?",
            answers = new string[] { "Lê Lợi", "Nguyễn Trãi", "Lê Lai", "Lê Thái Tổ" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Ngôi chùa nào nổi tiếng ở Ninh Bình?",
            answers = new string[] { "Chùa Bái Đính", "Chùa Hương", "Chùa Một Cột", "Chùa Thiên Mụ" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Món ăn truyền thống của Việt Nam là gì?",
            answers = new string[] { "Phở", "Bánh mì", "Bún chả", "Tất cả đều đúng" },
            correctAnswerIndex = 3
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Áo dài là trang phục truyền thống của nước nào?",
            answers = new string[] { "Việt Nam", "Trung Quốc", "Nhật Bản", "Hàn Quốc" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Tên thật của Chủ tịch Hồ Chí Minh là gì?",
            answers = new string[] { "Nguyễn Ái Quốc", "Nguyễn Sinh Cung", "Hồ Chí Minh", "Tất cả đều đúng" },
            correctAnswerIndex = 3
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Vịnh Hạ Long thuộc tỉnh nào?",
            answers = new string[] { "Quảng Ninh", "Hải Phòng", "Ninh Bình", "Thanh Hóa" },
            correctAnswerIndex = 0
        });

        allQuestions.Add(new QuizQuestion
        {
            question = "Tết Nguyên Đán là tết gì?",
            answers = new string[] { "Tết Cổ truyền", "Tết Trung Thu", "Tết Đoan Ngọ", "Tết Hàn Thực" },
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
