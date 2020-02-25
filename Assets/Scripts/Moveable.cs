using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    // TODO: Branch off from this code and create a Pushable property

    public float moveSpeed = 15f;

    private static float CLOSENESS_THRESHOLD = .03f;
    private static Vector3 PIVOT_OFFSET = new Vector3(.5f, -.5f, 0f);

    private Vector3 m_TargetPosition;

    // Start is called before the first frame update
    void Awake()
    {
        m_TargetPosition = transform.position;
    }

    bool IsCloseEnoughToTargetPosition()
    {
        return Vector2.Distance(transform.position, m_TargetPosition) < CLOSENESS_THRESHOLD;
    }

    public bool IsOnTargetPosition()
    {
        return transform.position == m_TargetPosition;
    }

    public bool AttemptMove(Vector2 direction)
    {
        // if we're already moving...
        if (!IsOnTargetPosition()) return false;

        // TODO: Add in throttling/cooldown to avoid instances where button is held down
        //       against a Stoppable and AttemptMove runs more than it needs to.

        Vector3 desiredPosition = transform.position + (Vector3)direction;

        Collider2D[] hits = Physics2D.OverlapBoxAll(desiredPosition + PIVOT_OFFSET, new Vector2(.2f, .2f), 0f);

        foreach (Collider2D hit in hits)
        {
            // TODO: Check if the stoppable is "enabled" or not
            if (hit.GetComponent<Stoppable>() != null)
            {
                Debug.Log($"{name}: hit a Stoppable, stopping.");

                return false;
            }

            Moveable moveable = hit.GetComponent<Moveable>();

            if (moveable != null)
            {
                Debug.Log($"{name}: hit a Moveable ({moveable.name}), telling it to AttemptMove in direction {direction}.");

                if (!moveable.AttemptMove(direction))
                {
                    Debug.Log($"{name}: {moveable.name} couldn't move, stopping.");

                    return false;
                }
            }
        }

        m_TargetPosition = transform.position + (Vector3)direction;

        Debug.Log($"{name}: Moving in direction {direction} at speed {moveSpeed}.");

        return true;
    }

    void Update()
    {
        if (IsOnTargetPosition()) return;

        if (IsCloseEnoughToTargetPosition())
        {
            transform.position = m_TargetPosition;
        }
        else
        {
            // TODO: replace with tween calculation to more accurately control easing formula
            transform.position = Vector2.Lerp(transform.position, m_TargetPosition, Time.deltaTime * moveSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireCube(transform.position, new Vector3(.5f, .5f, .5f));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(m_TargetPosition, new Vector3(.5f, .5f, .5f));

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(m_TargetPosition + PIVOT_OFFSET, .2f);
    }
}
