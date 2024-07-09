using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DEVInteractionCollider : MonoBehaviour
{
    [SerializeField] Inventario inventarioPlayer;
    [SerializeField] ItemInfo itemData;
    [SerializeField] ItemUI objeto;
    [SerializeField] List<GameObject> interactuable;
    [SerializeField] GameObject fueraDeRango;
    public bool interactuando = false;

    [SerializeField] Control camara;
    [SerializeField] bool enfocado = false;
    // Start is called before the first frame update
   private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<cofreController>() || other.GetComponent<Item3D>())
        {
            interactuable.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<cofreController>() || other.GetComponent<Item3D>())
        {
            fueraDeRango = other.gameObject;
            foreach (GameObject o in interactuable) 
            {
                if (o == fueraDeRango)
                    interactuable.Remove(o);
            }
            //interactuable = null;
        }
    }

    public void Desactivar()
    {
        Destroy(objeto.GetComponent<Rigidbody>());
        if (objeto.GetComponent<MeshRenderer>())
            objeto.GetComponent<MeshRenderer>().enabled = false;
        objeto.transform.SetParent(inventarioPlayer.transform);
        Destroy(objeto.GetComponent<BoxCollider>());
    }

    public void Activar() 
    {
        objeto.AddComponent<Rigidbody2D>().isKinematic = true;
        objeto.AddComponent<BoxCollider2D>();
        objeto.GetComponent<ItemUI>().enabled = true;
        objeto.GetComponent<RawImage>().enabled = true;
        objeto.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        objeto.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
        inventarioPlayer.AddItem(objeto.GetComponent<ItemUI>());
    }

    public void OnInteract() 
    {
        if (interactuable.Count > 0)
        {
            if (interactuable[interactuable.Count - 1].GetComponent<Item3D>())
            {
                ItemInfo info = interactuable[interactuable.Count - 1].GetComponent<Item3D>().referencia;
                objeto.GetComponent<RawImage>().texture = info.imagenObjeto;
                GameObject p = Instantiate(objeto.gameObject);
                //p.GetComponent<RawImage>().texture = info.imagenObjeto;
                p.name = info.nombre;
                p.GetComponent<ItemUI>().id = info.id;
                p.GetComponent<ItemUI>().tipo = info.tipo;
                p.GetComponent<ItemUI>().cantidad = info.cantidad;
                p.GetComponent<ItemUI>().data = info;
                inventarioPlayer.AddItem(p.GetComponent<ItemUI>());
                Destroy(interactuable[interactuable.Count - 1].gameObject);
            }

            if (interactuable[^1].GetComponent<cofreController>()) interactuable[interactuable.Count - 1].GetComponent<cofreController>().Abrir();
        }
    }
}
