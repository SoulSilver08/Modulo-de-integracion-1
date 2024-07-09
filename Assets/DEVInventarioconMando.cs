using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEVInventarioconMando : MonoBehaviour
{
    [SerializeField] Inventario inventario;
    public List<GameObject> espacios;
    public InvEspacio espacioActual;
    public int cuenta = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.N)) espacios = inventario.invEspacios;

        if (Input.GetKeyDown(KeyCode.UpArrow)) NavegacionMando('-', 6);

        if (Input.GetKeyDown(KeyCode.RightArrow)) NavegacionMando('+', 1);

        if (Input.GetKeyDown(KeyCode.DownArrow)) NavegacionMando('+', 6);

        if (Input.GetKeyDown(KeyCode.LeftArrow)) NavegacionMando('-', 1);
    }

    public void NavegacionMando(char signo, int cantidad) 
    {
        if (espacios.Count == 0) espacios = inventario.invEspacios;
        switch (signo) 
        {
            case '+':
                cuenta += cantidad;
                if (espacioActual) espacioActual.Highlighted(false);
                if (cuenta < espacios.Count) espacios[cuenta].GetComponent<InvEspacio>().Highlighted(true);
                else
                {
                    cuenta = espacios.Count - 1;
                    espacios[cuenta].GetComponent<InvEspacio>().Highlighted(true);
                }
                espacioActual = espacios[cuenta].GetComponent<InvEspacio>();
                break;

            case '-':
                cuenta -= cantidad;
                if (espacioActual) espacioActual.Highlighted(false);
                if (cuenta >= 0) espacios[cuenta].GetComponent<InvEspacio>().Highlighted(true);
                else
                {
                    cuenta = 0;
                    espacios[cuenta].GetComponent<InvEspacio>().Highlighted(true);
                }
                espacioActual = espacios[cuenta].GetComponent<InvEspacio>();
                break;
        }
    }
}
