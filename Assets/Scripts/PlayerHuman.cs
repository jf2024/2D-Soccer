using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHuman : Player
{
    protected override void Move()
    {
        Vector2 movement = _playerInput.actions[$"Player{(playerID == PlayerID.Player1 ? "1" : "2")}Move"].ReadValue<Vector2>();
        _rigidbody.velocity = new Vector2(movement.x * _speed, _rigidbody.velocity.y);
    }

    protected override void Jump()
    {
        if (isGrounded &&  _playerInput.actions[$"Player{(playerID == PlayerID.Player1 ? "1" : "2")}Jump"].ReadValue<float>() > 0)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

/*    protected override void Awake()
    {
        base.Awake();
        Debug.Log("Stats: " + GameManager.Instance.p1stats.wins);
    }*/
}
