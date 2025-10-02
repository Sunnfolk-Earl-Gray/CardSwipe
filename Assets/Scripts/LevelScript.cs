using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelScript", menuName = "Scriptable Objects/LevelScript")]
[System.Serializable]
public class LevelScript : ScriptableObject
{
    [SerializeField] private List<GameObject> Enemies;
    public List<GameObject> spawnedEnemies;
    [SerializeField] [Tooltip("These coordinates will correspond to the enemy in the Enemies array with the same index")]private List<Vector3> EnemySpawns;
    [SerializeField] private GameObject LevelSprite;
    private GameObject LevelInstance;

    private GameObject player
    {
        get { return GameObject.FindWithTag("Player"); }
    }

    public void SpawnEnemies()
    {
        spawnedEnemies.Clear();
        LevelInstance = Instantiate(LevelSprite, new Vector3(0,0,0), LevelSprite.transform.rotation);
        
        if (Enemies.Count != EnemySpawns.Count) throw new System.Exception("Enemies count mismatch");
        for  (int i = 0; i < Enemies.Count; i++)
        {
            var enemy = Instantiate(Enemies[i], EnemySpawns[i], Quaternion.identity);
            spawnedEnemies.Add(enemy);
            Debug.Log("Spawned Enemy");
            enemy.GetComponent<TESTEnemy>().enabled = false;
        }
        
    }

    public void ActivateLevel()
    {
        player.transform.position = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position;
        player.GetComponent<BoxCollider2D>().enabled = true;
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        foreach (var enemy in spawnedEnemies)
        {
            enemy.GetComponent<TESTEnemy>().enabled = true;
        }
    }

    public void UnLoad()
    {
        
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<BoxCollider2D>().enabled = false;
        foreach (GameObject enemy in spawnedEnemies) Destroy(enemy);
        Destroy(LevelInstance);
    }
}
