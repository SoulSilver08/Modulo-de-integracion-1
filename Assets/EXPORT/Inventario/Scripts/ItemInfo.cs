using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Datos Item")]
public class ItemInfo : ScriptableObject
{
    [Header("Elementos graficos")]
    public Texture2D imagenObjeto;
    public MeshFilter modeloObjeto;
    public Material[] materialesObjeto;

    [Header("Valores del Objeto")]
    public string nombre;
    public int id;
    public int tipo;
    public int cantidad;
    [TextAreaAttribute(6, 20)]
        public string Descripcion;
}
