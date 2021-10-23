using TMPro;
using UnityEngine;

public class SoldiersIncreaserDividing : SoldiersIncreaser
{
    [SerializeField] private int _divisionValue;
    
    protected override void IncreaseSquad(SoldiersSquad squad)
    {
        int newCount = squad.SquadCount / _divisionValue;
        int countToRemove = squad.SquadCount - newCount;
        squad.RemoveSoldiers(countToRemove);
    }

    protected override void SetText(TextMeshProUGUI text)
    {
        text.text = $"/{_divisionValue}";
    }
}