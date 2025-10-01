using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
   [SerializeField] private List<RectTransform> _coinPoses;
   [SerializeField] private RectTransform _currentPos;
   [SerializeField] private GameObject _coin;
   
   private InputSystem_Actions _actions;
   [SerializeField] private float _moveDelay;

   private void Awake()
   {
      _actions = new InputSystem_Actions();
      _actions.Enable();
   }

   private void Update()
   {
      if (_moveDelay <= 0)
      {
         _moveDelay = 0.2f;
         if (_actions.Player.Move.ReadValue<Vector2>().x > 0)
         {
            if (_coinPoses.IndexOf(_currentPos) == _coinPoses.Count - 1) _currentPos = _coinPoses[0];
            else _currentPos = _coinPoses[_coinPoses.IndexOf(_currentPos) + 1];
            _coin.transform.position = _currentPos.position;
         }
         else if (_actions.Player.Move.ReadValue<Vector2>().x < 0)
         {
            if (_coinPoses.IndexOf(_currentPos) == 0) _currentPos = _coinPoses[2];
            else _currentPos = _coinPoses[_coinPoses.IndexOf(_currentPos) - 1];
            _coin.transform.position = _currentPos.position;
         }
      }
      _moveDelay -= Time.deltaTime;
   }

   private void OnDisable()
   {
      _actions.Disable();
   }
}
