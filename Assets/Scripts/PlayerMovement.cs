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
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        PlayerRigidbody.MovePosition(transform.position + moveDirection.normalized * movementSpeed * Time.deltaTime);

        Vector3 rotationDirection = new Vector3(Input.GetAxis("JoystickRightHorizontal"), 0, Input.GetAxis("JoystickRightVertical"));

        if (rotationDirection == Vector3.zero)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                rotationDirection = hit.point;
                rotationDirection.y = transform.position.y;
                transform.LookAt(rotationDirection);
            }
        }
        else
        {
            Quaternion rotation = Quaternion.Slerp(PlayerRigidbody.rotation, Quaternion.LookRotation(rotationDirection, Vector3.up), rotationSpeed * Time.deltaTime);
            PlayerRigidbody.rotation = rotation;
        }
    }
}

