using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelScript", menuName = "Scriptable Objects/LevelScript")]
[System.Serializable]
public class LevelScript : ScriptableObject
{
    [SerializeField] private List<GameObject> Enemies;
    [SerializeField] private List<GameObject> _spawnedEnemies;
    [SerializeField] [Tooltip("These coordinates will correspond to the enemy in the Enemies array with the same index")]private List<Vector3> EnemySpawns;
    [SerializeField] private GameObject LevelSprite;
    private GameObject LevelInstance;

    public void SpawnEnemies()
    {
        _spawnedEnemies.Clear();
        LevelInstance = Instantiate(LevelSprite, new Vector3(0,0,0), LevelSprite.transform.rotation);
        
        if (Enemies.Count != EnemySpawns.Count) throw new System.Exception("Enemies count mismatch");
        for  (int i = 0; i < Enemies.Count; i++)
        {
            var enemy = Instantiate(Enemies[i], EnemySpawns[i], Quaternion.identity);
            _spawnedEnemies.Add(enemy);
            Debug.Log("Spawned Enemy");
            _spawnedEnemies[i].GetComponent<EnemyChase>().enabled = false;
            Debug.Log("Deactivated");
        }
    }

    public void ActivateLevel()
    {
        foreach (var enemy in _spawnedEnemies)
        {
            enemy.GetComponent<EnemyChase>().enabled = true;
        }
    }

    public void UnLoad()
    {
        foreach (GameObject enemy in _spawnedEnemies) Destroy(enemy);
        Destroy(LevelInstance);
    }
}
