namespace RehvidGames.UI.Player
{
    using System.Collections;
    using TMPro;
    using UnityEngine;

    public class PlayerSoul: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private float _animationDuration = 2f;

        public void OnUpdateText(int souls)
        {
            int currentValue = int.TryParse(_textMeshPro.text, out int parsedValue) ? parsedValue : 0;
            StartCoroutine(AnimateTextCoroutine(currentValue, souls));
        }

        private IEnumerator AnimateTextCoroutine(int startValue, int endValue)
        {
            float elapsedTime = 0f;

            while (elapsedTime < _animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _animationDuration;
                int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, t));

                _textMeshPro.text = currentValue.ToString();
                yield return null;
            }
            _textMeshPro.text = endValue.ToString();
        }
    }
}