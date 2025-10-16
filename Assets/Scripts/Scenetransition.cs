using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Scenetransition : MonoBehaviour
{
    [SerializeField] private string GameScene;
    private InputSystem_Actions _actions;

    private void Start()
    {
        _actions = new InputSystem_Actions();
        _actions.Enable();
    }
    private void Update()
    {
        if (_actions.Player.Jump.WasPressedThisFrame()) GoToGameScene();
    }
    public void GoToGameScene()
    {
        SceneManager.LoadScene(GameScene);
    }

    private void OnDestroy()
    {
        _actions.Disable();
    }
}
