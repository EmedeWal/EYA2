
namespace EmeWillem
{
    using UnityEngine.UI;
    using UnityEngine;

    public class Line : MonoBehaviour
    {
        public Perk StartPerk { get; private set; }
        public Perk EndPerk { get; private set; }

        private Image _lineImage;

        public void DrawLine(Perk startPerk, Perk endPerk, Transform parent, Color color)
        {
            _lineImage = GetComponent<Image>();

            transform.SetParent(parent, false);

            StartPerk = startPerk;
            EndPerk = endPerk;

            Vector3 startPos = StartPerk.GetComponent<RectTransform>().position;
            Vector3 endPos = EndPerk.GetComponent<RectTransform>().position;

            Vector3 midPoint = (startPos + endPos) / 2;
            Vector3 lineDirection = (endPos - startPos).normalized;

            float distance = Vector3.Distance(startPos, endPos);
            _lineImage.rectTransform.sizeDelta = new Vector2(distance, 10);
            _lineImage.rectTransform.position = midPoint;
            _lineImage.rectTransform.rotation = Quaternion.FromToRotation(Vector3.right, lineDirection);

            SetColor(color);
        }

        public void SetColor(Color color)
        {
            _lineImage.color = color;
        }
    }

}