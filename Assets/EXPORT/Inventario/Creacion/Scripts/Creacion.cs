using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Creacion : MonoBehaviour
{
    [Header("Referencias a los espacios")]
    [SerializeField] InvEspacio espacioA;
    [SerializeField] InvEspacio espacioB;
    [SerializeField] InvEspacio espacioC;
    [SerializeField] InvEspacio espacioD;
    [SerializeField] InvEspacio espacioE;
    [SerializeField] InvEspacio espacioF;
    [SerializeField] InvEspacio espacioResultado;

    [Header("Referencia  a lista de Items")]
    [SerializeField] ClaseListaPrefabs listaPrefabs;
    [SerializeField] GameObject objetoUI;
    [SerializeField] Inventario inventarioUI;

    [Header("Referencia al Excel(.csv)")]
    [SerializeField] TextAsset combinaciones;

    private GameObject descripcion;

    private void OnEnable()
    {
        inventarioUI.gameObject.SetActive(true);
        descripcion = inventarioUI.GetComponentsInChildren<Transform>()[1].gameObject;
        descripcion.SetActive(false);
    }

    private void OnDisable()
    {
        inventarioUI.gameObject.SetActive(false);
        descripcion.SetActive(true);
    }

    public void CreacionObjeto()
    {
        Debug.Log("CreacionObjeto()");
        int objetosACombinar = 0;
        List<int> objetosPorCombinar = new();
        foreach (InvEspacio espacio in GetComponentsInChildren<InvEspacio>())   //Revisa que espacios tienen un item dentro y los guarda en una lista
        {
            if (espacio.itemDentro)
            {
                objetosPorCombinar.Add(espacio.itemDentro.id);
                objetosACombinar += 1;
            }
        }

        int cuenta = 1;
        string[] datosCombinaciones = combinaciones.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);     //Almacena la informacion del Excel en un array para su rapido acceso
        for (int i = 1; i < datosCombinaciones.Length; i++)     //Recorrido del array con la informacion del excel
        {
            if (i / 9 == cuenta)    //cuando el recorrido pase por la primera columna del excel
            {
                cuenta += 1;
                if (Int32.Parse(datosCombinaciones[i]) == objetosACombinar)     //Si el valor es igual al numero de objeto a combinar
                {
                    int resultado = VerificacionCombinacion(i, datosCombinaciones, objetosPorCombinar);
                    Debug.Log("Resultado: " + resultado);
                    if (resultado > 0) 
                    {
                        AgregarYRemover(resultado);
                        return;
                    }
                }
            }
        }
    }

    public int VerificacionCombinacion(int numerolista, string[] datos, List<int> opc)
    {
        Debug.Log("Verificacion");
        int cuenta = 0;
        foreach (int id in opc) 
        {
            for (int i = 1; i < opc.Count + 1; i++) 
            {
                if (id == Int32.Parse(datos[numerolista + i]))
                {
                    cuenta += 1;
                    break;
                }
            }
            if (cuenta == 0) return 0;
        }

        if (cuenta == opc.Count)
            return Int32.Parse(datos[numerolista + 7]);
        return 0;
    }

    public void AgregarYRemover(int resultado) 
    {
        foreach (InvEspacio espacio in GetComponentsInChildren<InvEspacio>())
        {
            if (espacio.itemDentro)
            {
                espacio.itemDentro.cantidad -= 1;
                espacio.itemDentro.ActualizarNumero();
            }
        }

        foreach (ItemInfo objeto in listaPrefabs.listaDeScriptableObjects) 
        {
            if (objeto.id == resultado) 
            {
                objetoUI.GetComponent<RawImage>().texture = objeto.imagenObjeto;
                GameObject p = Instantiate(objetoUI);
                //p.GetComponent<RawImage>().texture = info.imagenObjeto;
                p.name = objeto.nombre;
                p.GetComponent<ItemUI>().id = objeto.id;
                p.GetComponent<ItemUI>().tipo = objeto.tipo;
                p.GetComponent<ItemUI>().cantidad = objeto.cantidad;
                p.GetComponent<ItemUI>().data = objeto;
                espacioResultado.AsignarItem(p.GetComponent<ItemUI>());
                p.GetComponent<RectTransform>().ForceUpdateRectTransforms();
            }

            
        }
    }
}
