using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvEspacio : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemUI itemDentro; //Referencia del item que se encuentra en el espacio
    public bool Resaltado = false;

    //Al pasar el puntero por encima ilumina el espacio
    public void OnPointerEnter(PointerEventData eventData)
    {
        Resaltado = true;
        Highlighted(Resaltado);
    }

    //Al quitar el puntero del espacio este deja de iluminarse
    public void OnPointerExit(PointerEventData eventData)
    {
        Resaltado = false;
        Highlighted(Resaltado);
    }

    public void Highlighted(bool resaltado) 
    {
        if (resaltado) 
        {
            if (GetComponentInParent<Inventario>())
            {
                Inventario inv = GetComponentInParent<Inventario>();
                if (inv.iluminarEspacio)
                    gameObject.GetComponentsInChildren<RawImage>()[1].color = new Color(0, 1, 0, 0.5f);
            }
            return;
        }
        gameObject.GetComponentsInChildren<RawImage>()[1].color = new Color(0, 1, 0, 0);
    }

    //Al pasar un item encima del espacio muestra el espacio al layer de enfrente
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ItemUI>() && !GetComponentInParent<ScrollRect>())
            transform.SetSiblingIndex(24);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.GetComponent<ItemUI>() && collision.CompareTag("estatico")) itemDentro = collision.GetComponent<ItemUI>();
    }

    public void AsignarItem(ItemUI objeto)
    {
        itemDentro = objeto;
        objeto.transform.SetParent(transform);
        objeto.gameObject.GetComponent<RectTransform>().localPosition = Vector2.zero;
        objeto.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        objeto.gameObject.GetComponent<RectTransform>().localScale = new Vector3(.7f, .7f, .7f);
        objeto.espacio = this;
        if(GetComponentInParent<Inventario>()) GetComponentInParent<Inventario>().ActualizarLista();
    }

    public void RemoverItem()
    {
        itemDentro = null;
        if (GetComponentInParent<ScrollRect>()) GetComponentInParent<Inventario>().AjustarInventario();
    }
}
