using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    //[Header("¡¡¡DEV!!! Logica del Item")]
    ItemUI cambio;    //item con el que cambiara de posicion
    [HideInInspector] public InvEspacio espacio;
    public TMP_Text numCantidad;  //Texto de la cantidad del mismo item (stack)
    bool arrastrando = false;  //Detecta si el item esta siendo arrastrado
    bool intercambio = false;  //Detecta si el item esta colisionando con otro item con id diferente
    bool dentroSV; //Detecta si se encuentra dentro de un Scrollview
    [HideInInspector] public bool dentroEspacio = true;

    [Header("Valores del Objeto")]
    public int id; //Identificador del item
    public int tipo; //1) Armas, 2) Armadura, 3) Consumibles, 4) Herramientas
    public int cantidad = 1;    //Cantidad del mismo item (Stack)

    //

    [HideInInspector] public bool mouseOver = false;
    public ItemInfo data;

    void Start()
    {
        GetComponent<RawImage>().texture = data.imagenObjeto;
        espacio = GetComponentInParent<InvEspacio>();
        id = data.id;
        tipo = data.tipo;
    }

    void Update()
    {
        if (arrastrando)    //Si se arrastra, la posicion del item sera igual a la del mouse
        {
            GetComponent<BoxCollider2D>().size = new Vector2(5, 5);
            transform.position = Input.mousePosition;

            gameObject.tag = "arrastrando"; //Se cambia el tag del item para identificar entre items que se esten moviendo y items estaticos
        }
        else
        {
            GetComponent<BoxCollider2D>().size = new Vector2(100, 100);
            gameObject.tag = "estatico";
        }

        /////////////////////////////////////////////////////////////////////////////7777777777777

        if (mouseOver) 
        {
            if (GetComponentInParent<Inventario>())
            {
                GetComponentInParent<Inventario>().UIDescription.GetComponentInChildren<TMP_Text>().text = data.Descripcion;
                Soltar();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "estatico":
                cambio = null;
                intercambio = false;
                break;

            case "dentro":
                dentroSV = false;
                break;

        }
    }

    private void OnTriggerStay2D(Collider2D collision)  
    {
        if (collision.GetComponent<InvEspacio>())
        {
            dentroEspacio = true;
        }
        else dentroEspacio = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "espacioInventario":
                transform.SetParent(collision.transform);
                break;

            case "estatico":
                cambio = collision.GetComponent<ItemUI>();
                intercambio = true;
                break;

            case "dentro":
                if (arrastrando == true)
                    transform.SetParent(collision.transform);
                dentroSV = true;
                break;
        }

    }

    //Comportamiento del item al hacer click
    public void OnPointerDown(PointerEventData eventData)
    {
        arrastrando = true;
        if (GetComponentInParent<Inventario>())
        {
            //if (dentroEspacio) transform.SetParent(GetComponentsInParent<Transform>()[4]);    //Deja de ser child del espacio

            GetComponentInParent<Inventario>().objetos.Remove(gameObject.GetComponent<ItemUI>());    //Remueve el item de la lista del inventario/cofre
            GetComponentInParent<Inventario>().iluminarEspacio = false;
        }
        if (dentroSV) GetComponentInParent<ScrollRect>().enabled = false;    //En caso de estar dentro del Scrollview deshabilita la habilidad de hacer scroll
    }
    //Conmportamiento del item al dejar de hacer click
    public void OnPointerUp(PointerEventData eventData)
    {
        arrastrando = false;
        Inventario inventario = GetComponentInParent<Inventario>();
        if (inventario)
        {
            inventario.iluminarEspacio = true;
        }
        if (espacio.GetComponentInParent<Inventario>()) espacio.GetComponentInParent<Inventario>().iluminarEspacio = true;



        //Comportamiento al soltar un item dentro del scrollView
        if (dentroSV && CompareTag("arrastrando"))
        {
            if (dentroEspacio) 
            {
                espacio.AsignarItem(this);
                return;
            }

            GetComponentInParent<ScrollRect>().enabled = true;
            if (!dentroEspacio && !intercambio)    //Si se suelta dentro del scrollView pero fuera de un espacio
            {
                if (espacio.GetComponentInParent<ScrollRect>()) 
                {
                    espacio.AsignarItem(this);
                    return;
                }
                espacio.RemoverItem();
                inventario.AddItem(this);
                dentroEspacio = true;
                return;
            }
        }

        //Comportamiento al soltar un item dentro de un espacio
        if (dentroEspacio || intercambio)
        {
            if (intercambio)    //Comportamiento al soltar un item en un espacio con otro item dentro
            {
                if (cambio.id == id)    //si los items son iguales se suman
                {
                    cambio.cantidad += cantidad;
                    cambio.ActualizarNumero();
                    espacio.RemoverItem();
                    Destroy(gameObject);
                }
                else    //Si los item son diferentes se intercambian las posiciones
                {
                    espacio.AsignarItem(cambio);
                    GetComponentInParent<InvEspacio>().AsignarItem(this);
                }
            }
            else    //Comportamiento al soltar un item en un espacio vacio
            {
                espacio.RemoverItem();
                GetComponentInParent<InvEspacio>().AsignarItem(this);
            }
        }
        else    //Comportamiento al soltar un item fuera de un espacio
        {
            espacio.AsignarItem(this);
            if (espacio.GetComponentInParent<ScrollRect>()) espacio.GetComponentInParent<ScrollRect>().enabled = true;
        }
    }

    public void ActualizarNumero()    //Funcion que actualiza el numero que representa la cantidad del item
    {
        if (cantidad > 1)
            numCantidad.gameObject.SetActive(true);
        else
            numCantidad.gameObject.SetActive(false);

        numCantidad.text = cantidad.ToString();

        if (cantidad < 1)
        {
            espacio.RemoverItem();
            Destroy(gameObject);
        }
    }


    /////////////////////////////////////////////////////////////////


    public void OnPointerEnter(PointerEventData eventData) 
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }

    public void Soltar() 
    {
        if (GetComponentInParent<Inventario>().soltar)
        {
            GetComponentInParent<Inventario>().soltar = false;
            GetComponentInParent<Inventario>().prefabItem3D.referencia = data;
            GameObject objeto3D = Instantiate(GetComponentInParent<Inventario>().prefabItem3D.gameObject, GetComponentInParent<Inventario>().Player.transform.position, new Quaternion(0, 0, 0, 0));
            espacio.RemoverItem();
            Destroy(gameObject);
        }
    }
}
