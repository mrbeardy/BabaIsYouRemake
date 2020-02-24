using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask LinkTileLayer = -1;

    [Header("Colors")]
    public float activeAlpha = 1f;
    public float inactiveAlpha = .4f;

    private TMP_Text m_TextComponent;
    private bool m_IsActive = false;

    // Start is called before the first frame update
    void Awake()
    {
        m_TextComponent = GetComponentInChildren<TMP_Text>();

        SetActiveState(IsNearLinkTile());
    }

    bool IsLinkTile()
    {
        return LinkTileLayer == (LinkTileLayer | (1 << gameObject.layer));
    }

    bool IsNearLinkTile()
    {
        // TODO: Extract out to inherited class and remove all of these checks entirely
        if (IsLinkTile()) return true;

        // TODO: Don't hardcode pivot offset
        //return Physics2D.OverlapBox(transform.position + new Vector3(0.5f, -0.5f, 0f), new Vector2(1.1f, 1.1f), 0f, LinkTileLayer);
        return Physics2D.OverlapCircle(transform.position + new Vector3(0.5f, -0.5f, 0f), .6f, LinkTileLayer);
    }

    void SetActiveState(bool newActive)
    {
        m_IsActive = newActive;

        Color color = m_TextComponent.color;

        color.a = newActive ? activeAlpha : inactiveAlpha;

        m_TextComponent.color = color;
    }

    private void Update()
    {
        // TODO: Add OnMove event to Moveable and trigger this when that is invoked.
        SetActiveState(IsNearLinkTile());
    }

    void OnDrawGizmos()
    {
        if (IsLinkTile()) return;

        Color color = Color.gray;

        if (IsNearLinkTile())
        {
            color = Color.green;
        }

        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0.5f, -0.5f, 0f), .6f);
    }
}
