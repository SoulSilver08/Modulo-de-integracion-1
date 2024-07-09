using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class DEVMovement3D : MonoBehaviour
{
    [SerializeField] float mSpeed = 0;
    public float rotationX;
    public float angulo;
    public Vector2 mov;
    public GameObject c;
    public GameObject camara;
    public bool enTierra = true;

    [SerializeField] private float velocity;

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            angulo = (Mathf.Atan2(mov.x, mov.y) * Mathf.Rad2Deg) + c.transform.rotation.eulerAngles.y;
            transform.localRotation = Quaternion.Euler(0, angulo, 0);
            transform.position += mSpeed * Time.deltaTime * new Vector3(math.sin(angulo * Mathf.Deg2Rad), 0, math.cos(angulo * Mathf.Deg2Rad));
        }

        //gameObject.GetComponent<Rigidbody>().AddForce(50 * Vector3.up);
    }

    private void Update()
    {
        velocity = GetComponent<Rigidbody>().velocity.y;
    }

    public void OnMove(InputValue inputValue)
    {
        mov = inputValue.Get<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context) 
    {
        if (enTierra)
            if (context.performed) GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 8, GetComponent<Rigidbody>().velocity.z);
        //if (context.performed) GetComponent<Rigidbody>().AddForce(Vector3.up * 400);

        if (context.canceled) 
            if(velocity > 4) GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 4, GetComponent<Rigidbody>().velocity.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            enTierra = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            enTierra = false;
        }
    }
}
