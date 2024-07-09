using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.InputSystem;

public class ManagerEspacioDeGuardado : MonoBehaviour
{
    public string ruta;
    public void ActualizarInformacion(string numero, string nombre, string ubicacion, string fecha, string tiempo, Texture2D imagen, string path)
    {
        GetComponentsInChildren<TMP_Text>()[0].text = numero;     //Numero de guardado
        GetComponentsInChildren<TMP_Text>()[1].text = nombre;     //Nombre
        GetComponentsInChildren<TMP_Text>()[2].text = ubicacion;     //Ubicacion
        GetComponentsInChildren<TMP_Text>()[4].text = fecha;     //Fecha
        GetComponentsInChildren<TMP_Text>()[6].text = tiempo;     //Tiempo

        GetComponentInChildren<RawImage>().texture = imagen;      //Imagen lugar donde guardo el player
    }

    public void Cargar() 
    {
        Debug.Log("Cargar Partida");
        GetComponentInParent<CargadoDePartida>().CargarPartida();
    }

    public void path(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            string[] fileInfo = Directory.GetFiles(Application.persistentDataPath + "/", "*.txt");
            foreach (string file in fileInfo) 
            {
                Debug.Log(Path.GetFileNameWithoutExtension(file));
            }

            //ActualizarInformacion(3, "Skragoll", "Llanura de la calma", "2024/04/24 12:55", "48:55", null);
        }
    }
}
