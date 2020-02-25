using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stoppable : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, new Vector3(1f, 1f, 1f));
    }
}
