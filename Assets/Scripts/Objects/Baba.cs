using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Moveable))]
public class Baba : MonoBehaviour
{
    private PlayerInput m_PlayerInput;
    private Moveable m_Moveable;

    private void Awake()
    {
        m_PlayerInput = new PlayerInput();
        m_Moveable = GetComponent<Moveable>();

        //m_PlayerInput.Gameplay.Move.performed += HandleMoveInput;
    }

    private void Update()
    {
        if (m_Moveable.IsOnTargetPosition())
        {
            HandleMoveInput();
        }
    }

    //private void HandleMoveInput(InputAction.CallbackContext callback)
    private void HandleMoveInput()
    {
        //Vector2 moveValue = callback.ReadValue<Vector2>();
        Vector2 moveValue = m_PlayerInput.Gameplay.Move.ReadValue<Vector2>();

        // FIXME: Condense down by doing vector2 to vector2 math
        if (moveValue.x != 0)
        {
            m_Moveable.AttemptMove(transform.right * moveValue.x);
        }
        else if (moveValue.y != 0)
        {
            m_Moveable.AttemptMove(transform.up * moveValue.y);
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
