using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputAction botonSoltar;

    [Header("Lista de objetos en el inventario")]
    public List<ItemUI> objetos;  //Lista de items en el inventario

    [Header("Referencias")]
    [SerializeField] InvEspacio prefabEspacio;    //Referencia al prefab I_Espacio
    [SerializeField] public Item3D prefabItem3D;
    [SerializeField] public GameObject UIDescription;

    [HideInInspector] ScrollRect scrollView; //Referencia al Scrollview del inventario
    [HideInInspector] public List<GameObject> invEspacios;    //Lista de espacios en el inventario
    [HideInInspector] public bool soltar = false;
    [HideInInspector] public bool iluminarEspacio = true;    //Determina si los espacios del inventario se pueden iluminar
    int filtro = 0;    //almacena el tipo de filtro que se esta aplicando 0) Todos, 1) Armas, 2) armaduras, 3) Consumibles, 4) Herramienntas

    [HideInInspector]
    //public cofreController cofre;
    
    int cuentaHorizontal = 0;    //Cuenta filas del inventario
    int cuentaVertical = 0;    //Cuenta columnas del inventario
    [SerializeField] int tipoInventario;  //Tipo del inventario a mostrar 0 = inventario jugador, 1 = Inventario cofre
    public GameObject Player;

    private void OnEnable()
    {
        botonSoltar.Enable();
    }

    private void OnDisable()
    {
        botonSoltar.Disable();
    }

    private void Start()
    {
        botonSoltar.performed += _ => Soltar();
        scrollView = GetComponentInChildren<ScrollRect>();
        if(tipoInventario == 0) InventarioPlayer();    //Al iniciar llena el inventario del player
    }

    /*private void OnDisable()
    {
        if (tipoInventario == 1) 
        {
            objetos.Clear();
            
            foreach (ItemUI objeto in GetComponentsInChildren<ItemUI>())
            {
                objeto.GetComponent<BoxCollider2D>().enabled = false;
                objeto.GetComponent<Rigidbody2D>().simulated = false;
                objeto.transform.SetParent(cofre.transform);
                objetos.Add(objeto);
            }
            cofre.inventario = objetos;
        }
    }*/

    public Vector2 LlenadoInventario(int tipo)
    {
        if (cuentaHorizontal == 6)
        {
            cuentaVertical += 1;
            cuentaHorizontal = 0;
        }

        Vector2 p = new();
        switch (tipo)
        {
            case 0:
                p = new Vector2(-252 + (100 * cuentaHorizontal), 390 - (100 * cuentaVertical));
                break;

            case 1:
                p = new Vector2(140 + (100 * cuentaHorizontal), 250 - (100 * cuentaVertical));
                break;
        }
        return p;
    }

    public void InventarioPlayer()    //Llena el inventario del  player
    {
        foreach (ItemUI G in objetos)
        {
            //Se instancia el objeto InvEspacio, se hace child del content del Scrollview y se le asigna la posicion en la interfaz
            GameObject nuevoEspacio = Instantiate(prefabEspacio.gameObject, scrollView.GetComponentsInChildren<RectTransform>()[2].transform);
            invEspacios.Add(nuevoEspacio);
            //Se instancia el objeto Item, se hace child del content del Scrollview y se le asigna la posicion en la interfaz
            GameObject objeto = Instantiate(G.gameObject, nuevoEspacio.transform);
            nuevoEspacio.GetComponent<InvEspacio>().itemDentro = objeto.GetComponent<ItemUI>();
            objeto.GetComponent<ItemUI>().dentroEspacio = true;
            cuentaHorizontal += 1;
        }
    }

    /*public void LlenarInventarioCofre(cofreController cofreActual)    //Llena el inventario del cofre
    {
        cofre = cofreActual;
        objetos = cofre.inventario;
        if (cofre.yaAbierto) 
        {
            
            foreach (ItemUI objeto in cofre.GetComponentsInChildren<ItemUI>()) 
            {
                objeto.GetComponent<BoxCollider2D>().enabled = true;
                objeto.GetComponent<Rigidbody2D>().simulated = true;
                objeto.gameObject.transform.SetParent(transform);
            }
            return;
        }
        foreach (ItemUI objeto in objetos)
        {
            //Se instancia el objeto Item y se le asigna la posicion en la interfaz
            GameObject nuevoItem = Instantiate(objeto.gameObject, GetComponentsInChildren<InvEspacio>()[cuentaHorizontal].transform);
            nuevoItem.GetComponent<ItemUI>().dentroEspacio = true;
            nuevoItem.GetComponent<RectTransform>().localPosition = Vector2.zero;
            cuentaHorizontal += 1;
        }

        cuentaHorizontal = 0;
        cuentaVertical = 0;
    }

    public void VaciarInventarioCofre() 
    {
        cofre.inventario = objetos;
        foreach(InvEspacio espacio in GetComponentsInChildren<InvEspacio>())
        {
            if(espacio.itemDentro) 
            {
                espacio.itemDentro.transform.SetParent(cofre.transform);
                espacio.itemDentro.GetComponent<BoxCollider2D>().enabled = false;
                espacio.itemDentro.GetComponent<Rigidbody2D>().simulated = false;
            }
        }
        
        cuentaHorizontal = 0;
        cuentaVertical = 0;
    }*/

    //Funcion encargada de recorrer los espacios del inventario cada vez que se elimina un item
    public void AjustarInventario()
    {
        bool recorre = false;
        Vector2 nuevaPosicion = new();
        Vector2 posicionAnterior;
        foreach (GameObject espacio in invEspacios.ToList())
        {
            if (recorre == true)
            {
                posicionAnterior = espacio.GetComponent<RectTransform>().localPosition;
                espacio.GetComponent<RectTransform>().localPosition = nuevaPosicion;
                nuevaPosicion = posicionAnterior;
            }
            if (espacio.GetComponent<InvEspacio>().itemDentro == null && recorre == false)
            {
                nuevaPosicion = espacio.GetComponent<RectTransform>().localPosition;
                invEspacios.Remove(espacio);
                Destroy(espacio);
                recorre = true;
            }
        }
        if (filtro > 0) OrdenarInventario(filtro);
    }

    //Funcion encargada de agregar items al inventario del player
    public void AddItem(ItemUI item)
    {
        foreach (GameObject espacio in invEspacios) //revisa en el inventario si exite algun espacio vacio o otro item con el mismo ID
        {
            if (!espacio.GetComponentInChildren<ItemUI>())
            {
                espacio.GetComponent<InvEspacio>().itemDentro.transform.SetParent(espacio.transform);
                espacio.GetComponent<InvEspacio>().itemDentro.transform.position = Vector2.zero;
                return;
            }
            if (espacio.GetComponentInChildren<ItemUI>().id == item.id)    //Si el item a agregar tiene el mismo id que otro que ya se encuentra en el inventario 
            {
                espacio.GetComponentInChildren<ItemUI>().cantidad += item.GetComponent<ItemUI>().cantidad;
                espacio.GetComponentInChildren<ItemUI>().ActualizarNumero();
                espacio.GetComponentInChildren<ItemUI>().dentroEspacio = true;
                Destroy(item.gameObject);
                return;
            }
        }
        //Se instancia el objeto InvEspacio, se hace child del content del Scrollview y se le asigna la posicion en la interfaz
        GameObject nuevoEspacio = Instantiate(prefabEspacio.gameObject, scrollView.GetComponentsInChildren<RectTransform>()[2].transform);
        item.gameObject.transform.SetParent(nuevoEspacio.transform);
        item.gameObject.transform.localPosition = Vector2.zero;
        nuevoEspacio.GetComponent<InvEspacio>().itemDentro = item;
        nuevoEspacio.GetComponent<InvEspacio>().AsignarItem(item);
        invEspacios.Add(nuevoEspacio);
        if (filtro > 0) OrdenarInventario(filtro);
        ActualizarLista();
    }

    //Funcion encargada de darle unna posicion a los espacios/items al momento de ser filtrados
    public void OrdenarInventario(int tipo)    //tipo de item a mostrar: 0) todos, 1) armas, 2) armaduras, 3) consumibles, 4) herramientas
    {
        filtro = tipo;
        int numEspacios = 0;
        foreach (GameObject espacio in invEspacios)
        {
            if (tipo == 0)
            {
                espacio.SetActive(true);
                numEspacios++;
            }
            else
            {
                if (espacio.GetComponentInChildren<ItemUI>().tipo == tipo)
                {
                    espacio.SetActive(true);
                    numEspacios++;
                }
                else espacio.SetActive(false);
            }
        }
    }

    public void ActualizarLista()
    {
        objetos.Clear();
        foreach(InvEspacio espacio in GetComponentsInChildren<InvEspacio>())
        {
            if(espacio.itemDentro) objetos.Add(espacio.itemDentro);
        }
    }

    ///

    public void Soltar() 
    {
        soltar = true;
    }
}
