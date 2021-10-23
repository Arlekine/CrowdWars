using TMPro;
using UnityEngine;

public class SoldiersIncreaserAdding : SoldiersIncreaser
{
    [SerializeField] private int _soldiersToIncrease;
    
    protected override void IncreaseSquad(SoldiersSquad squad)
    {
        if (_soldiersToIncrease > 0)
            squad.AddSoldiers(_soldiersToIncrease);
        else
            squad.RemoveSoldiers(Mathf.Abs(_soldiersToIncrease));
    }

    protected override void SetText(TextMeshProUGUI text)
    {
        string sign = _soldiersToIncrease > 0 ? "+" : "";
        text.text = $"{sign}{_soldiersToIncrease}";
    }
}