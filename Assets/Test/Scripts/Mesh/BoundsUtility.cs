using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsUtility : MonoBehaviour {

    private void OnDrawGizmos()
    {
        var bounds = GetBounds(gameObject);
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

    Bounds GetBounds(GameObject go)
    {
        var mfs = go.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds();
        if (mfs != null)
        {
            foreach (var mf in mfs)
            {
                bounds.Encapsulate(mf.bounds);
            }
        }
        return bounds;
    }
}
