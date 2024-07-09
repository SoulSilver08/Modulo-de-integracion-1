using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    [Header("Referencias a UI")]
    [SerializeField] public Inventario inventario;    //Referencia al UI del inventario del jugador
    [SerializeField] GameObject equipo;    //Referencia al UI del inventario del equipo
    [SerializeField] Creacion creacion;     //Referencia al UI de creacion de Items
    [SerializeField] GameObject Descripcion;
    [SerializeField] GameObject menuPausa;

    [HideInInspector]
    public bool invActivo = false;
    [HideInInspector]
    public Vector2 turn;
    [HideInInspector]
    public float rSpeed, angulo;

    [Header("Referencia al Player")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject parent;

    [Header("¡¡¡DEV!!! Referencias Item 3D")]
    [SerializeField] GameObject objetoPrueba;
    [SerializeField] ItemInfo info;

    [HideInInspector] public bool enfocado = false;
    [HideInInspector] public Transform objetivo;

    private int estado = 0;
    void Update()
    {
        transform.parent = parent.transform;
        GetComponentsInParent<Transform>()[1].position = player.transform.position;
        Camera();

    }

    //Abre el inventario del player
    public void AbrirInventario() 
    {
        if (!invActivo) 
        {
            InvActivo();
            return;
        }
        Cerrar();
    }

    //Abre el menu de creacion de items
    public void AbrirCreacion() 
    {
        inventario.gameObject.SetActive(true);
        creacion.gameObject.SetActive(true);
    }

    //controla la camara del player
    public void Camera()
    {
        turn.x += GetComponentInParent<PlayerInput>().actions.FindAction("Camera").ReadValue<Vector2>().x;
        turn.y -= GetComponentInParent<PlayerInput>().actions.FindAction("Camera").ReadValue<Vector2>().y;
        GetComponentsInParent<Transform>()[1].rotation = Quaternion.Euler(turn.y, turn.x, 0);
    }

    public void OnSelect() 
    {
        Debug.Log("Click!");
    }

    public void OnDrop() 
    {
        Debug.Log("Drop");
    }

    //Muestra el UI del inventario 
    public void InvActivo()
    {
        estado = 1;
        inventario.gameObject.SetActive(true);
        Descripcion.SetActive(true);
        invActivo = true;
    }

    //Oculta el UI del inventario
    public void Cerrar()
    {
        if (invActivo)
        {
            inventario.gameObject.SetActive(false);
            Descripcion.SetActive(false);
            equipo.SetActive(false);
            creacion.gameObject.SetActive(false);
            invActivo = false;
            menuPausa.SetActive(false);
        }
        else
        {
            menuPausa.SetActive(true);
            estado = 1;
        }
    }

    ///DEV prueba creacion objeto en inventario
    public void CreacionObjeto() 
    {
        GameObject G = new()
        {
            tag = "estatico"
        };
        G.AddComponent<RawImage>();
        G.GetComponent<RawImage>().rectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        G.AddComponent<BoxCollider2D>();
        G.GetComponent<BoxCollider2D>().isTrigger = true;
        G.AddComponent<Rigidbody2D>();
        G.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        G.AddComponent<ItemUI>();
        G.GetComponent<ItemUI>().id = Random.Range(1, 100);
        G.GetComponent<ItemUI>().tipo = Random.Range(1, 5);
        switch (G.GetComponent<ItemUI>().tipo)
        {
            case 1:
                G.GetComponent<RawImage>().color = new Color(0, 0, Random.Range(.5f, 1));
                break;

            case 2:
                G.GetComponent<RawImage>().color = new Color(Random.Range(.5f, 1), 0, 0);
                break;

            case 3:
                G.GetComponent<RawImage>().color = new Color(0, Random.Range(.5f, 1), 0);
                break;

            case 4:
                G.GetComponent<RawImage>().color = new Color(1, Random.Range(.5f, 1), 0);
                break;
        }
        GameObject T = new();
        T.AddComponent<TextMeshPro>();
        T.GetComponent<TextMeshPro>().text = "";
        T.transform.SetParent(G.transform);
        G.GetComponent<ItemUI>().numCantidad = T.GetComponent<TextMeshPro>();
        inventario.AddItem(G.GetComponent<ItemUI>());
    }

    ///DEV!!!! prueba creacion Item3D en el mundo
    public void CreacionObjeto3D(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            objetoPrueba.GetComponent<Item3D>().referencia = info;
            GameObject objeto3D = Instantiate(objetoPrueba);
        }
    }
}

