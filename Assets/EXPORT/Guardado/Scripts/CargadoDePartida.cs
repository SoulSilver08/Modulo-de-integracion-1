using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using static UnityEngine.UI.Button;

public class CargadoDePartida : MonoBehaviour
{
    [SerializeField] ManagerEspacioDeGuardado prefabEspacioGuardado;
    GameObject espacioGuardado;
    [SerializeField] private GameObject confirmationScreen;

    /*[FormerlySerializedAs("onClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();*/
    // Start is called before the first frame update
    void Start()
    {
        string[] fileInfo = Directory.GetFiles(Application.persistentDataPath + "/", "*.txt");
        foreach (string file in fileInfo)
        {
            string nombre = Path.GetFileNameWithoutExtension(file);
            Debug.Log(nombre);
            espacioGuardado = Instantiate(prefabEspacioGuardado.gameObject, transform);
            espacioGuardado.GetComponent<ManagerEspacioDeGuardado>().ActualizarInformacion(nombre, "Skragoll", "Llanura de la calma", "2024/04/24 12:55", "48:55", null, file);
        }
    }

    public void CargarPartida() 
    {
        Debug.Log("Seguro?");
        confirmationScreen.SetActive(true);
    }

    public void SiCarga() 
    {
        SceneManager.LoadScene("MockUpInv");
    }

    public void NoCarga() 
    {
        confirmationScreen.SetActive(false);
    }
}
