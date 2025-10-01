using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<LevelScript> _levels;
    [SerializeField] private LevelScript _currentLevel;
    
    [SerializeField] private bool _fading;
    [SerializeField] private RawImage _fade;
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
        StartCoroutine(fade(2));
    }

    private IEnumerator fade(float time)
    {
        if (!_fading)
        {
            _fading = true;
            while (_fade.color.a <= 1 && _fade.color.a >= 0)
            {
                _fade.color += new Color(0, 0, 0, 0.01f * Mathf.Sign(time));
                yield return new WaitForSeconds(time / 100);
            }
            _currentLevel.UnLoad();
            _currentLevel = _levels[_levels.IndexOf(_currentLevel)+1];
            _currentLevel.SpawnEnemies();
            var player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position;
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
        foreach (var enemy in _currentLevel.spawnedEnemies)
        {
            if (enemy == null) _currentLevel.spawnedEnemies.Remove(enemy);
        }

        if (_levels.IndexOf(_currentLevel) == _levels.Count - 1 || GameObject.FindGameObjectWithTag("Player") == null)
        {
            LeaderboardScript.instance.saveScore();
            SceneManager.LoadScene(1);
        }
        if (_currentLevel.spawnedEnemies.Count == 0 && _levels.IndexOf(_currentLevel) != _levels.Count - 1) LoadNextLevel();
    }
}
