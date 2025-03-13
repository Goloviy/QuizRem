using TMPro;
using UnityEngine;

namespace UIControllers
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI secDozens,secUnits;

        public void UpdateTime(long _time)
        {
            var secondsDozens = Mathf.FloorToInt(_time / 10000);
            var secondsUnits = (_time - secondsDozens * 10000) / 1000;

            secDozens.text = secondsDozens.ToString();
            secUnits.text = Mathf.FloorToInt(secondsUnits).ToString();
        }
    }
}
