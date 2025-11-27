using UnityEngine;

// Class này không gắn trực tiếp vào vật nào, mà để cho các vật khác kế thừa
public abstract class Interactable : MonoBehaviour
{
    // Thêm biến này để quản lý màu sắc
    protected Renderer objRenderer;
    protected bool isBroken = false; // Trạng thái đã vỡ chưa

    // Thêm hàm Start ảo để các con có thể dùng hoặc ghi đè
    protected virtual void Start()
    {
        objRenderer = GetComponent<Renderer>();
    }

    // Hàm ảo (Virtual): Cho phép các con ghi đè (Override) để làm việc riêng
    public virtual void OnInteract()
    {
        // Mặc định không làm gì cả, chỉ báo log
        Debug.Log("Chạm vào: " + gameObject.name);
    }

    // Hàm bị phá (Enemy tấn công)
    public virtual void Break()
    {
        if (isBroken) return; // Nếu vỡ rồi thì thôi
        isBroken = true;

        Debug.Log(gameObject.name + " ĐÃ BỊ PHÁ HỦY!");

        // THAY ĐỔI: Đổi màu sang xám đen thay vì tắt object
        if (objRenderer != null)
        {
            objRenderer.material.color = Color.gray;
        }
    }
}