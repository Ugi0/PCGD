using UnityEngine;

[ExecuteInEditMode]
public class CustomSlider : MonoBehaviour
{
    [SerializeField] GameObject FillRect;
    [SerializeField] GameObject PreviousRectangle;
    private static int SLIDER_LENGTH = 159;

    void Start()
    {
        SetCurrent(0);
        SetPrevious(0);
        HidePrevious();
    }

    public void SetCurrent(float value) {
        RectTransform fillRectTransform = FillRect.GetComponent<RectTransform>();
        fillRectTransform.offsetMax = new Vector2(100 - SLIDER_LENGTH * (1-value), 0);
    }

    public void SetPrevious(float value) {
        PreviousRectangle.SetActive(true);
        RectTransform fillRectTransform = PreviousRectangle.GetComponent<RectTransform>();
        Vector3 pos = fillRectTransform.localPosition;
        pos.x = 77 * ( (value - 0.5f) * 2 );
        pos.z = -1;
        fillRectTransform.localPosition = pos;
    }
    public void HidePrevious() {
        PreviousRectangle.SetActive(false);
    }
}
