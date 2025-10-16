using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TutorialUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> listObjectOnTop = new();
    [SerializeField] private List<GameObject> listObjectOnLeft = new();
    [SerializeField] private List<GameObject> listObjectOnDown = new();
    [SerializeField] private List<GameObject> listObjectOnRight = new();

    [Header("Focus & Arrow")]
    [SerializeField] private Image m_focusedImage;       // Khung highlight
    [SerializeField] private GameObject m_arrow;         // Mũi tên hướng dẫn
    [SerializeField] private Canvas m_mainCanvas;        // Canvas (Screen Space - Overlay)

    [Header("Setting")]
    [SerializeField] private float m_arrowOffset = 100f; // Khoảng cách giữa arrow và target
    [SerializeField] private float m_focusPadding = 1.3f;// Phóng to thêm khung focus

    private Camera mainCamera;
    private int currentStep = 0;
    private readonly List<GameObject> allTargets = new();

    private enum Axis { Horizontal, Vertical }

    private void Awake()
    {
        mainCamera = Camera.main;
        SortAllLists();
        MergeListsInOrder();
    }

    private void OnEnable()
    {
       Reset();
    }
   
 
    //Goi reset tutorial UI
    private void Reset()
    {
        currentStep = 0;
        ShowNextStep();
    }
    // ==============================
    // === SẮP XẾP DANH SÁCH THEO HƯỚNG
    // ==============================
    private void SortAllLists()
    {
        listObjectOnLeft = SortByScreenPosition(listObjectOnLeft, Axis.Vertical, true);  // Trên → Dưới
        listObjectOnRight = SortByScreenPosition(listObjectOnRight, Axis.Vertical, false); // Dưới → Trên
        listObjectOnTop = SortByScreenPosition(listObjectOnTop, Axis.Horizontal, true);  // Phải → Trái
        listObjectOnDown = SortByScreenPosition(listObjectOnDown, Axis.Horizontal, false); // Trái → Phải
    }

    private List<GameObject> SortByScreenPosition(List<GameObject> list, Axis axis, bool descending)
    {
        if (list == null || list.Count == 0) return list;

        if (axis == Axis.Vertical)
        {
            return descending
                ? list.OrderByDescending(o => GetScreenPosition(o).y).ToList()
                : list.OrderBy(o => GetScreenPosition(o).y).ToList();
        }
        else
        {
            return descending
                ? list.OrderByDescending(o => GetScreenPosition(o).x).ToList()
                : list.OrderBy(o => GetScreenPosition(o).x).ToList();
        }
    }

    private void MergeListsInOrder()
    {
        allTargets.Clear();
        allTargets.AddRange(listObjectOnTop);
        allTargets.AddRange(listObjectOnLeft);
        allTargets.AddRange(listObjectOnRight);
        allTargets.AddRange(listObjectOnDown);
    }

    // ==============================
    // === HIỂN THỊ TỪNG BƯỚC
    // ==============================
    public int ShowNextStep()
    {
        if (currentStep >= allTargets.Count)
        {
            m_arrow.SetActive(false);
            m_focusedImage.gameObject.SetActive(false);
            return -1;
        }

        var target = allTargets[currentStep];
        if (target == null)
        {
            Debug.Log($"UI reset vì object thứ {currentStep} là null tutorial đã kết thúc");
            Reset();
            return -1;
        }

        // --- Arrow ---
        Vector2 screenPos = GetScreenPosition(target);
        Vector2 arrowScreenPos = GetArrowPositionByList(screenPos, target, out float rotationZ);

        // Chuyển screen → local canvas pos
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_mainCanvas.transform as RectTransform,
            arrowScreenPos,
            null,
            out Vector2 localArrowPos
        );

        // Gán arrow
        RectTransform arrowRect = m_arrow.GetComponent<RectTransform>();
        arrowRect.anchoredPosition = localArrowPos;
        arrowRect.localRotation = Quaternion.Euler(0, 0, rotationZ);
        m_arrow.SetActive(true);

        // --- Focus Image ---
        UpdateFocusImage(target);

        Debug.Log($"Step {currentStep}: {target.name} | rot={rotationZ}");
        currentStep++;
        return currentStep - 1;
    }

    // ==============================
    // === FOCUS IMAGE
    // ==============================
    private void UpdateFocusImage(GameObject target)
    {
        Vector2 screenPos = GetScreenPosition(target);
        Vector2 size = GetObjectScreenSize(target); // width, height

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_mainCanvas.transform as RectTransform,
            screenPos,
            null,
            out Vector2 localPos
        );

        RectTransform focusRect = m_focusedImage.GetComponent<RectTransform>();
        focusRect.anchoredPosition = localPos;
        focusRect.sizeDelta = size * m_focusPadding;
        m_focusedImage.gameObject.SetActive(true);
    }

    // ==============================
    // === HỖ TRỢ TÍNH TOÁN
    // ==============================
    private Vector2 GetScreenPosition(GameObject target)
    {
        if (target == null) return Vector2.zero;

        // UI Object
        if (target.TryGetComponent(out RectTransform rectTransform) &&
            target.layer == LayerMask.NameToLayer("UI"))
        {
            return rectTransform.position;
        }

        // 3D Object
        return mainCamera.WorldToScreenPoint(target.transform.position);
    }

    private Vector2 GetObjectScreenSize(GameObject target)
    {
        // --- UI ---
        if (target.TryGetComponent(out RectTransform rect))
        {
            return rect.rect.size;
        }

        // --- 3D Object ---
        Renderer renderer = target.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            Bounds bounds = renderer.bounds;
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;

            Vector3 screenMin = mainCamera.WorldToScreenPoint(min);
            Vector3 screenMax = mainCamera.WorldToScreenPoint(max);

            float width = Mathf.Abs(screenMax.x - screenMin.x);
            float height = Mathf.Abs(screenMax.y - screenMin.y);

            return new Vector2(width, height);
        }

        // fallback
        return new Vector2(100f, 100f);
    }

    private Vector2 GetArrowPositionByList(Vector2 screenPos, GameObject target, out float rotationZ)
    {
        Vector2 size = GetObjectScreenSize(target);
        Vector2 halfSize = size / 2f;

        Vector2 arrowPos = screenPos;
        rotationZ = 0f;

        if (listObjectOnTop.Contains(target))
        {
            arrowPos = new Vector2(screenPos.x, screenPos.y - (halfSize.y + m_arrowOffset));
            rotationZ = 180f;
        }
        else if (listObjectOnDown.Contains(target))
        {
            arrowPos = new Vector2(screenPos.x, screenPos.y + (halfSize.y + m_arrowOffset));
            rotationZ = 0f;
        }
        else if (listObjectOnLeft.Contains(target))
        {
            arrowPos = new Vector2(screenPos.x + (halfSize.x + m_arrowOffset), screenPos.y);
            rotationZ = -90f;
        }
        else if (listObjectOnRight.Contains(target))
        {
            arrowPos = new Vector2(screenPos.x - (halfSize.x + m_arrowOffset), screenPos.y);
            rotationZ = 90f;
        }

        arrowPos.x = Mathf.Clamp(arrowPos.x, 50f, Screen.width - 50f);
        arrowPos.y = Mathf.Clamp(arrowPos.y, 50f, Screen.height - 50f);

        return arrowPos;
    }
}
