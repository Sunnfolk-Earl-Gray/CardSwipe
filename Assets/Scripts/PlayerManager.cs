using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int _cards = 3;
    [SerializeField] private int _score;

    public void TakeDamage()
    {
        _cards = 0;
        
    }
}
