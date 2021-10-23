using TMPro;
using UnityEngine;

public class SoldiersIncreaserMultiplying : SoldiersIncreaser
 {
     [SerializeField] private int _multiplyingValue;
     
     protected override void IncreaseSquad(SoldiersSquad squad)
     {
         
         squad.AddSoldiers(squad.SquadCount * _multiplyingValue - squad.SquadCount);
     }
 
     protected override void SetText(TextMeshProUGUI text)
     {
         text.text = $"x{_multiplyingValue}";
     }
 }