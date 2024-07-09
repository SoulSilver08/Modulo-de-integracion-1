using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item3D : MonoBehaviour
{
    [SerializeField] public ItemInfo referencia;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshFilter>().mesh = referencia.modeloObjeto.sharedMesh;
        gameObject.GetComponent<MeshRenderer>().materials = referencia.materialesObjeto;
        gameObject.AddComponent<MeshCollider>().sharedMesh = referencia.modeloObjeto.sharedMesh;
        gameObject.GetComponent<MeshCollider>().convex = true;
    }
}
