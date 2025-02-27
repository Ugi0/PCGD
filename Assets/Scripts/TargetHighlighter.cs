using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(BoxCollider2D))]
public class TargetHighlighter : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private BoxCollider2D boxCollider;
    private GameObject arrow;
    private SpriteRenderer arrowRenderer;
    public Sprite arrowSprite; // Assign this in the Inspector
    private float bobbingSpeed = 2f;
    private float bobbingHeight = 0.2f;
    private float arrowScale = 0.8f;
    private Vector3 arrowStartLocalPosition;
    private float arrowOffset = 2.5f;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        SetupLineRenderer();
        CreateArrow();

    }

    void SetupLineRenderer()
    {
        lineRenderer.positionCount = 5; // 4 corners + closing point
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.startWidth = 0.08f;
        lineRenderer.endWidth = 0.08f;
        lineRenderer.sortingOrder = 1;

        UpdateHighlight();
    }

    void UpdateHighlight()
    {
        Vector2 size = boxCollider.size;
        Vector2 offset = boxCollider.offset;
        Transform t = boxCollider.transform;

        Vector3[] corners = new Vector3[5]
        {
            t.TransformPoint(offset + new Vector2(-size.x / 2, -size.y / 2)), // Bottom Left
            t.TransformPoint(offset + new Vector2(size.x / 2, -size.y / 2)),  // Bottom Right
            t.TransformPoint(offset + new Vector2(size.x / 2, size.y / 2)),   // Top Right
            t.TransformPoint(offset + new Vector2(-size.x / 2, size.y / 2)),  // Top Left
            t.TransformPoint(offset + new Vector2(-size.x / 2, -size.y / 2))  // Close loop
        };

        lineRenderer.SetPositions(corners);
    }

    void CreateArrow()
    {
        arrow = new GameObject("Arrow");
        arrow.transform.SetParent(transform);
        arrow.transform.localScale = new Vector3(arrowScale, -arrowScale, 1.0f);
        arrowRenderer = arrow.AddComponent<SpriteRenderer>();
        arrowRenderer.sprite = arrowSprite;
        arrowRenderer.color = Color.yellow;
        arrowRenderer.sortingOrder = 1;

        Vector3 colliderTop = new Vector3(boxCollider.offset.x, boxCollider.offset.y + boxCollider.size.y / 2, 0);
        arrowStartLocalPosition = colliderTop + new Vector3(0, arrowOffset, 0);
        arrow.transform.localPosition = arrowStartLocalPosition;

        UpdateVisibility();
    }

    void UpdateVisibility()
    {
        if (arrow != null)
        {
            arrow.SetActive(GameStateManager.instance.score == 0);
        }
        if(lineRenderer != null)
        {
            lineRenderer.enabled = GameStateManager.instance.score < 3;
        }
    }

    void Update()
    {
        if (arrow != null)
        {
            float bobbingOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
            arrow.transform.localPosition = arrowStartLocalPosition + new Vector3(0, bobbingOffset, 0);
        }
    }

    void OnValidate()
    {
        if (lineRenderer != null && boxCollider != null)
        {
            UpdateHighlight();
        }
    }
}
