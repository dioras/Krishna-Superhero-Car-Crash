using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendCollider : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMeshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        Mesh bakeMesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(bakeMesh);
        var collider = GetComponent<MeshCollider>();
        collider.sharedMesh = bakeMesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
