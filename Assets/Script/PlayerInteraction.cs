using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Cài đặt")]
    [SerializeField] float interactRange = 10f; // Tầm với xa bao nhiêu
    [SerializeField] LayerMask interactLayer;   // Chỉ tương tác với lớp này (đỡ click nhầm sàn)

    void Update()
    {
        // Nếu bấm chuột trái (0)
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Đã bấm chuột!");
            CheckInteraction();
        }
    }

    void CheckInteraction()
    {
        // Tạo một tia chiếu từ Camera qua vị trí con trỏ chuột
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // VẼ TIA LASER MÀU ĐỎ TRONG SCENE ĐỂ SOI (Hiện trong 2 giây)
        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red, 2f);

        // Bắn tia!
        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            Debug.Log("Bắn trúng: " + hit.collider.name); // Báo tên vật bị bắn trúng

            Interactable item = hit.collider.GetComponent<Interactable>();
            if (item != null)
            {
                item.OnInteract();
            }
            else
            {
                Debug.Log("Vật này không có script Interactable!");
            }
        }
        else
        {
            Debug.Log("Bắn trượt!");
        }
    }
}