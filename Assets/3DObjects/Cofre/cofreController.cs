using System.Collections.Generic;
using UnityEngine;

public class cofreController : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] Inventario menuInventario; //Referecnia al script inventario del cofre
    [SerializeField] Inventario playerInventario;    //Referencia script inventario del jugador
    [SerializeField] Canvas canvas;
    [HideInInspector]
    public bool yaAbierto = false;

    [Header("Items dentro del cofre")]
    public List<ItemUI> inventario;
    private void Start()
    {
        yaAbierto = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            menuInventario.gameObject.SetActive(false);
            gameObject.GetComponent<Animator>().Play("Cerrar");
            //if(inventario.Count == 0) menuInventario.VaciarInventarioCofre();
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            Abrir();
        }
    }

    public void Abrir()
    {
        menuInventario.gameObject.SetActive(true);
        playerInventario.gameObject.SetActive(true);
        //menuInventario.LlenarInventarioCofre(this);
        yaAbierto = true;
        gameObject.GetComponent<Animator>().Play("Abrir");
    }
}
