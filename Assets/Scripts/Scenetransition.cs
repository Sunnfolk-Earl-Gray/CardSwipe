using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenetransition : MonoBehaviour
{
    [SerializeField] private string GameScene;
    
    public void GoToGameScene()
    {
        SceneManager.LoadScene(GameScene);
    }
}
