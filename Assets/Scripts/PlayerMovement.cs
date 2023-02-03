using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public float rotationSpeed = 10.0f;
    private Rigidbody PlayerRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontalTranslation = Input.GetAxis("Horizontal") * movementSpeed;
        var verticalTranslation = Input.GetAxis("Vertical") * movementSpeed;
        Vector3 moveInput = new Vector3(horizontalTranslation, 0, verticalTranslation);

        PlayerRigidbody.MovePosition(transform.position + moveInput * Time.deltaTime);

        var horizontalStickRotation = Input.GetAxis("JoystickRightHorizontal");
        var verticalStickRotation = Input.GetAxis("JoystickRightVertical");
        Vector3 stickMovementDirection = new Vector3(horizontalStickRotation, 0, verticalStickRotation);
        var horizontalMouseRotation = Input.GetAxis("Mouse X");
        var verticalMouseRotation = Input.GetAxis("Mouse Y");
        Vector3 mouseMovementDirection = new Vector3(horizontalMouseRotation, 0, verticalMouseRotation);

        if (stickMovementDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.Slerp(PlayerRigidbody.rotation, Quaternion.LookRotation(stickMovementDirection, Vector3.up), rotationSpeed * Time.deltaTime);
            PlayerRigidbody.rotation = rotation;
        }

    }
}

