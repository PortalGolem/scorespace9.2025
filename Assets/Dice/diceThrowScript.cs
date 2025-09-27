using UnityEngine;
using UnityEngine.InputSystem;

public class diceThrowScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] Camera mainCamera;
    [SerializeField] float throwForce;
    [SerializeField] int maximumNumber;
    [SerializeField] int minimumNumber;
    public int currentNumber;
    public bool isThrown;
    public bool hasLanded;
    public bool isHeld;

    private Vector2 moveDirection;
    private Vector2 oldMousePosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void grabDice(InputAction.CallbackContext context)
    {
        if (context.performed && !isThrown)
        {
            Debug.Log("Grabbed");
            isHeld = true;
        }
        if (context.canceled && isHeld)
        {
            isHeld = false;
            isThrown = true;
            rb.simulated = true;
            rb.AddForce(moveDirection * throwForce);
        }
    }

    public void moveMouse(InputAction.CallbackContext context)
    {
        if (isHeld)
        {
            Vector2 mousePosition = context.ReadValue<Vector2>();
            if (oldMousePosition != null)
            {
                moveDirection = (mousePosition - oldMousePosition).normalized;
            }
            oldMousePosition = mousePosition;
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10));
            transform.position = newPosition;
       }
    }
}
