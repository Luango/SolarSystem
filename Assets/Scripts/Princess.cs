using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Princess : MonoBehaviour {
    public Transform PrincessSpawnPoint;
    private Transform ParentTransform;

    private void Awake()
    {
        ParentTransform = transform.parent;
    }

    public void Reset()
    {
        transform.parent = ParentTransform;
        transform.position = PrincessSpawnPoint.position;
        transform.rotation = Quaternion.identity;
        transform.GetComponent<PolygonCollider2D>().enabled = true;
    }
}
