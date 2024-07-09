using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Datos Arma")]
public class EspadaInfo : ItemInfo
{
    [Header("Valores de Arma")]
    public int daño;
    public string tipoDeAtaque;
}
