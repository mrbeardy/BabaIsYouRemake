using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Moveable))]
public class Baba : MonoBehaviour
{
    public float moveSpeed = 25f;

    private PlayerInput m_PlayerInput;
    private Moveable m_Moveable;

    private void Awake()
    {
        m_PlayerInput = new PlayerInput();
        m_Moveable = GetComponent<Moveable>();
    }

    private void Update()
    {
        if (m_Moveable.IsOnTargetPosition())
        {
            HandleMoveInput();
        }
    }

    private void HandleMoveInput()
    {
        Vector2 moveValue = m_PlayerInput.Gameplay.Move.ReadValue<Vector2>();

        // FIXME: Condense down by doing vector2 to vector2 math
        if (moveValue.x != 0)
        {
            m_Moveable.AttemptMove(transform.right * moveValue.x, moveSpeed);
        }
        else if (moveValue.y != 0)
        {
            m_Moveable.AttemptMove(transform.up * moveValue.y, moveSpeed);
        }
    }

    private void OnEnable()
    {
        m_PlayerInput.Enable();
    }

    private void OnDisable()
    {
        m_PlayerInput.Disable();
    }
}
