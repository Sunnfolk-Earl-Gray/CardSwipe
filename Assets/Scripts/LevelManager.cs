using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<LevelScript> _levels;
    [SerializeField] private LevelScript _currentLevel;
    
    [SerializeField] private bool _fading;
    [SerializeField] private RawImage _fade;

    private void Awake()
    {
        _currentLevel = _levels[0];
        _currentLevel.SpawnEnemies();
    }

    public void LoadNextLevel()
    {
        _currentLevel = _levels[_levels.IndexOf(_currentLevel)+1];
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
            _currentLevel.SpawnEnemies();
            _fade.color = new Color(0,0,0, 0.5f + (Mathf.Sign(time)/2));
            while (_fade.color.a <= 1 && _fade.color.a >= 0)
            {
                _fade.color += new Color(0, 0, 0, 0.01f * Mathf.Sign(-time));
                yield return new WaitForSeconds(time / 100);
            }
            _fading = false;
        }
    }
}
