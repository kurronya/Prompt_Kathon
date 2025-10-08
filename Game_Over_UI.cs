using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // Nút Play Again - Reset lại màn hiện tại
    public void PlayAgain()
    {
        // Lấy index của scene hiện tại và reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Nút Go Home - Về màn Home
    public void GoHome()
    {
        // Load scene Home (đảm bảo scene "Home" đã được add vào Build Settings)
        SceneManager.LoadScene("Home");
    }
}