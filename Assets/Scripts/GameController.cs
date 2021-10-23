using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private SoldiersSquad _squad;
    [SerializeField] private LevelEndBattle _lastBattle;
    [SerializeField] private TextMeshProUGUI _winText;

    private void Awake()
    {
        _squad.onSquadDestroyed += RestartGame;
        _lastBattle.onWin += ShowWinText;
    }

    private void ShowWinText()
    {
        _winText.gameObject.SetActive(true);
        StartCoroutine(Restart());
    }

    private void RestartGame()
    {
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }
}
