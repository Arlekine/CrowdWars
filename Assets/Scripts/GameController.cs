using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController _inst;
    
    
    [SerializeField] private Button _startButton;
    [SerializeField] private ResultPanel _resultPanel;
    [SerializeField] private List<Level> _levels = new List<Level>();

    [Header("Win texts")] 
    [SerializeField] private string _middleTextWin;
    [SerializeField] private string _headerTextWin;
    [SerializeField] private string _buttonTextWin;
    
    [Header("Loose texts")] 
    [SerializeField] private string _middleTextLoose;
    [SerializeField] private string _headerTextLoose;
    [SerializeField] private string _buttonTextLoose;

    private int _currentLevelIndex = 0;
    private Level _instantiatedLevel;
    
    private void Awake () 
    {
        if (_inst == null) 
        {
            _inst = this;

            DontDestroyOnLoad(gameObject);
            
            InitGame();
        } 
        else if(_inst != this)
        {
            Destroy(gameObject);
        }
    }

    public void InitGame()
    {
        _startButton.gameObject.SetActive(true);
        _instantiatedLevel = Instantiate(_levels[_currentLevelIndex]);
        
        _instantiatedLevel.Squad.onSquadDestroyed += ShowLoosePanel;
        _instantiatedLevel.Squad.enabled = false;
        _instantiatedLevel.LastBattle.onWin += ShowWinPanel;
    }

    public void StartGame()
    {
        _instantiatedLevel.Squad.enabled = true;
    }

    public void ShowWinPanel()
    {
        _resultPanel.Show(_middleTextWin, _headerTextWin, _buttonTextWin, NextLevel);
    }
    
    public void ShowLoosePanel()
    {
        _resultPanel.Show(_middleTextLoose, _headerTextLoose, _buttonTextLoose, RestartLevel);
    }

    public void RestartLevel()
    {
        StartCoroutine(Restart());
    }
    
    public void NextLevel()
    {
        _currentLevelIndex++;

        if (_currentLevelIndex >= _levels.Count)
            _currentLevelIndex = 0;
        
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        _instantiatedLevel.Squad.onSquadDestroyed -= ShowLoosePanel;
        _instantiatedLevel.LastBattle.onWin -= ShowWinPanel;
        SceneManager.LoadScene(0);
        SceneManager.sceneLoaded += SceneLoaded;

        yield return null;
        
        _resultPanel.Hide();
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        
        InitGame();
    }
}