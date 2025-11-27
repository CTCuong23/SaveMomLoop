using System.Collections; // Cần cái này để dùng Coroutine
using TMPro;
using UnityEngine;
using UnityEngine.AI; // Để tắt AI của Enemy
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton

    [Header("Game Loop Settings")]
    public float survivalTime = 60f; // Mục tiêu: Sống sót 60 giây
    private float currentTimer;

    [Header("Game State")]
    public bool isGameOver = false;
    public bool isVictory = false;

    [Header("UI References")]
    public TMP_Text timerText; // Dùng cái này nếu xài TextMeshPro
    public TMP_Text victoryMessage; // Dòng chữ thông báo kết quả (nếu có)

    

    public GameObject gameOverPanel; // Panel đen thùi lùi khi thua
    public GameObject victoryPanel;  // Panel chiến thắng

    private void Awake()
    {
        // Setup Singleton
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
        // Bắt đầu đếm ngược từ 60s
        currentTimer = survivalTime;
        isGameOver = false;
        isVictory = false;

        // Ẩn các panel đi lúc đầu
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (victoryPanel) victoryPanel.SetActive(false);

       
    }

    void Update()
    {
        // Nếu game đã kết thúc (thắng hoặc thua) thì dừng lại
        if (isGameOver || isVictory) return;

        // Đếm ngược thời gian
        currentTimer -= Time.deltaTime;

        // --- CẬP NHẬT UI ĐỒNG HỒ ---
        if (timerText != null)
        {
            // Làm tròn số giây
            float seconds = Mathf.CeilToInt(currentTimer);
            // Định dạng text kiểu "00:59"
            timerText.text = string.Format("00:{0:00}", seconds);

            // Hiệu ứng: Khi còn dưới 10s thì chữ chuyển màu ĐỎ
            if (currentTimer <= 10) timerText.color = Color.red;
            else timerText.color = Color.white;
        }

        // KIỂM TRA ĐIỀU KIỆN THẮNG
        if (currentTimer <= 0)
        {
            TriggerVictory();
        }
    }

    // Hàm hỗ trợ: Rút ngắn thời gian (Dùng cho cái Điện thoại)
    public void ReduceTime(float amount)
    {
        currentTimer -= amount;
        Debug.Log("Đã gọi điện! Thời gian giảm đi " + amount + "s");
    }

    public void GameOver()
    {
        if (isGameOver || isVictory) return;
        isGameOver = true;

        Debug.Log("❌ MẸ ĐÃ MẤT! VÒNG LẶP KHỞI ĐỘNG LẠI...");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        Invoke("RestartLevel", 3f); // Chờ 3s để người chơi ngẫm nghĩ
    }

    
    // --- XỬ LÝ THẮNG (BỐ VỀ) ---
    public void TriggerVictory()
    {
        if (isGameOver || isVictory) return;
        isVictory = true;

        if(victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        Debug.Log("✅ BỐ ĐÃ VỀ! BẠN ĐÃ CỨU ĐƯỢC MẸ!");

        // Dừng mọi hoạt động của Enemy (Gọi hàm StopEnemy nếu cần)
        // Hiển thị màn hình chiến thắng
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}