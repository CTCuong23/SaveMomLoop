using UnityEngine;

// Class này không gắn trực tiếp vào vật nào, mà để cho các vật khác kế thừa
public abstract class Interactable : MonoBehaviour
{
    // Hàm ảo (Virtual): Cho phép các con ghi đè (Override) để làm việc riêng
    public virtual void OnInteract()
    {
        // Mặc định không làm gì cả, chỉ báo log
        Debug.Log("Chạm vào: " + gameObject.name);
    }

    // Hàm bị phá (Enemy tấn công)
    public virtual void Break()
    {
        Debug.Log(gameObject.name + " ĐÃ BỊ PHÁ HỦY!");
        // Mặc định: Tự hủy object luôn
        // (Nhưng các con có thể ghi đè để làm kiểu khác)
        gameObject.SetActive(false);
    }
}