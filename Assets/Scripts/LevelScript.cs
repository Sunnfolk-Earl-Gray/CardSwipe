using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelScript", menuName = "Scriptable Objects/LevelScript")]
[System.Serializable]
public class LevelScript : ScriptableObject
{
    [SerializeField] private List<GameObject> Enemies;
    [SerializeField] [Tooltip("These coordinates will correspond to the enemy in the Enemies array with the same index")]private List<Vector3> EnemySpawns;

    private void SpawnEnemies()
    {
        
        if (Enemies.Count != EnemySpawns.Count) throw new System.Exception("Enemies count mismatch");
        var i = 0;
        foreach (GameObject enemy in Enemies)
        {
            Instantiate(enemy, EnemySpawns[i], Quaternion.identity);
            i++;
        }
    }
}
