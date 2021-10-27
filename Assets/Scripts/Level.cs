using UnityEngine;

public class Level : MonoBehaviour
{
    public SoldiersSquad Squad => _squad;
    public LevelEndBattle LastBattle => _lastBattle;

    [SerializeField] private SoldiersSquad _squad;
    [SerializeField] private LevelEndBattle _lastBattle;
}