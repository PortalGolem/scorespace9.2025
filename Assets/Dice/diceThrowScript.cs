using UnityEngine;
using UnityEngine.InputSystem;

public class diceThrowScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] Camera mainCamera;
    [SerializeField] float throwForce;
    [SerializeField] int maximumNumber;
    [SerializeField] int minimumNumber;
    private DiceThrowData dice;
    public int currentNumber;

    private Vector2 MouseOffset;

    private Vector2 moveDirection;
    private Vector2 oldMousePosition;

    public void grabDice(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Dice"))
            {
                if (!hit.collider.gameObject.GetComponent<DiceThrowData>().isThrown)
                {
                    dice = hit.collider.gameObject.GetComponent<DiceThrowData>();
                    rb = dice.GetComponent<Rigidbody2D>();
                    MouseOffset = (Vector2)dice.transform.position - mousePosition;
                    dice.isHeld = true;
                    Debug.Log("Grabbed");
                }
            }
        }
        if (context.canceled && dice != null && dice.isHeld)
        {
            dice.isHeld = false;
            dice.isThrown = true;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddForce(moveDirection * throwForce);
        }
    }

    public void moveMouse(InputAction.CallbackContext context)
    {
        if (dice != null && dice.isHeld)
        {
            Vector2 mousePosition = context.ReadValue<Vector2>();
            if (oldMousePosition != null)
            {
                moveDirection = (mousePosition - oldMousePosition).normalized;
            }
            oldMousePosition = mousePosition;
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10));
            dice.transform.position = newPosition + (Vector3)MouseOffset;
       }
    }
}
