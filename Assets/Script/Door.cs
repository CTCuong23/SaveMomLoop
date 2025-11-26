using UnityEngine;
using UnityEngine.AI;

// KẾ THỪA TỪ INTERACTABLE (Thay vì MonoBehaviour)
public class Door : Interactable
{
    private NavMeshObstacle obstacle;
    private BoxCollider boxCollider;
    private bool isOpen = false;

    void Start()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        boxCollider = GetComponent<BoxCollider>();
        ApplyDoorState();
    }

    // GHI ĐÈ HÀM CỦA CHA
    public override void OnInteract()
    {
        // Logic riêng của cái cửa: Đảo ngược trạng thái
        isOpen = !isOpen;
        ApplyDoorState();
    }

    void ApplyDoorState()
    {
        // (Giữ nguyên logic đóng mở hôm qua)
        if (isOpen)
        {
            obstacle.enabled = false;
            boxCollider.isTrigger = true;
            Debug.Log("Cửa MỞ");
        }
        else
        {
            obstacle.enabled = true;
            obstacle.carving = true;
            boxCollider.isTrigger = false;
            Debug.Log("Cửa ĐÓNG");
        }
    }

    public override void Break()
    {
        Debug.Log("Cửa đã bị phá hủy!");

        // 1. Tắt hình ảnh (Làm cửa tàng hình)
        GetComponent<MeshRenderer>().enabled = false;

        // 2. Tắt va chạm (Để đi xuyên qua)
        GetComponent<BoxCollider>().enabled = false;

        // 3. Tắt chặn đường AI
        GetComponent<NavMeshObstacle>().enabled = false;

        // (Sau này thêm dòng này: PlaySound("Tiếng_Cửa_Vỡ"); )
    }
}