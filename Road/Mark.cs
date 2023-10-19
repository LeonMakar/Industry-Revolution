using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mark : MonoBehaviour
{
     public Mark NextMark;

    private void OnDrawGizmosSelected()
    {
        if (NextMark != null)
        {
            Gizmos.DrawLine(transform.position, NextMark.transform.position);
            Gizmos.DrawCube(transform.position, new Vector3(0.1f, 0.1f, 0.1f));
        }
    }

    public void FindClosetMark(List<Mark> entryPoints)
    {
        var mark = entryPoints.
            OrderBy(t => Vector3.Distance(t.transform.position, transform.position))
            .First(t => t);
        NextMark = mark;
    }
}
