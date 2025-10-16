using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<LevelScript> _levels;
    public LevelScript _currentLevel;
    
    [SerializeField] private bool _fading;
    [SerializeField] private RawImage _fade;
    [SerializeField] private Texture2D _winScreen;
    [SerializeField] private Texture2D _loseScreen;
    private bool _gameOver;
    public static LevelManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
        
        _currentLevel = _levels[0];
        _currentLevel.SpawnEnemies();
        _currentLevel.ActivateLevel();
    }

    public void LoadNextLevel()
    {
        if ((_levels.IndexOf(_currentLevel) == _levels.Count -1 || GameObject.FindGameObjectWithTag("Player") == null) && !_gameOver)
        {
            if (_currentLevel.spawnedEnemies.Count == 0 && _levels.IndexOf(_currentLevel) == _levels.Count -1)
            {
                _gameOver = true;
                LeaderboardScript.instance.saveScore();
                StartCoroutine(WinScreen(2, _winScreen));
            }
            else
            {
                _gameOver = true;
                LeaderboardScript.instance.saveScore();
                StartCoroutine(WinScreen(2, _loseScreen));
            }
        }
        else
        {
            StartCoroutine(fade(2));
        }
    }

    private IEnumerator WinScreen(float duration, Texture2D texture)
    {
        if (!_fading)
        {
            _fade.texture = texture;
            _fading = true;
            _fade.color = new Color(255,255,255, 0);
            while (_fade.color.a <= 1 && _fade.color.a >= 0)
            {
                _fade.color += new Color(0, 0, 0, 0.01f * Mathf.Sign(duration));
                yield return new WaitForSeconds(duration / 100);
            }
           yield return new WaitForSeconds(duration*3);
            SceneManager.LoadScene(0);
        }
    }
    
    private IEnumerator fade(float time)
    {
        if (!_fading)
        {
            _fade.color = new Color(0,0,0,0);
            _fading = true;
            while (_fade.color.a <= 1 && _fade.color.a >= 0)
            {
                _fade.color += new Color(0, 0, 0, 0.01f * Mathf.Sign(time));
                yield return new WaitForSeconds(time / 100);
            }
            _currentLevel.UnLoad();
            _currentLevel = _levels[_levels.IndexOf(_currentLevel)+1];
            _currentLevel.SpawnEnemies();
            _fade.color = new Color(0,0,0, 0.5f + (Mathf.Sign(time)/2));
            while (_fade.color.a <= 1 && _fade.color.a >= 0)
            {
                _fade.color += new Color(0, 0, 0, 0.01f * Mathf.Sign(-time));
                yield return new WaitForSeconds(time / 100);
            }
            _currentLevel.ActivateLevel();
            _fading = false;
        }
    }

    private void Update()
    {
        
        
        if (_currentLevel.spawnedEnemies.Count == 0 || GameObject.FindGameObjectWithTag("Player") == null) LoadNextLevel();
    }
}
