using TMPro;
using UnityEngine;

public class SoldiersIncreaserAdding : SoldiersIncreaser
{
    [SerializeField] private bool _red;
    [SerializeField] private GameObject _redFX, _greenFX;

    [SerializeField] private int _soldiersToIncrease;

    private void Start()
    {
        _redFX.SetActive(_red);
        _greenFX.SetActive(!_red);
    }

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