using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    public GameObject player;
    public Sprite changeSpriteTo;

    private static float CLOSENESS_THRESHOLD = .03f;
    private static Vector3 PIVOT_OFFSET = new Vector3(.5f, -.5f, 0f);

    private Vector3 m_TargetPosition;
    private float m_MoveSpeed = 20f;

    private bool m_HasMoved = false;

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

    public void AttemptMove(Vector2 direction, float speed)
    {
        m_HasMoved = true;

        Debug.Log($"{name}: attempting to move in direction {direction} at speed {speed}");

        // TODO: Add fallback for default speed
        m_MoveSpeed = speed;
        m_TargetPosition = transform.position + (Vector3) direction;

        // TODO: Look for other Moveables, and attempt to move them also
        Collider2D[] hits = Physics2D.OverlapBoxAll(m_TargetPosition + PIVOT_OFFSET, new Vector2(.2f, .2f), 0f);

        foreach (Collider2D hit in hits)
        {
            Moveable moveable = hit.GetComponent<Moveable>();

            if (moveable != null)
            {
                moveable.AttemptMove(direction, speed);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCloseEnoughToTargetPosition())
        {
            transform.position = m_TargetPosition;

            if (player != null && transform.position.y == 3)
            {
                player.GetComponent<SpriteRenderer>().sprite = changeSpriteTo;
            }
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, m_TargetPosition, Time.deltaTime * m_MoveSpeed);
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
