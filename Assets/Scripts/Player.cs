using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public float rotationSpeed = 10.0f;
    public Light SpotLight;
    private Rigidbody PlayerRigidbody;
    private AttackingEntity AttackingEntity;
    public PlayerAttack[] AttackData;
    private Color AttackBaseColor;
    private int AttackNumber;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        AttackingEntity = GetComponent<AttackingEntity>();

        //Get Base Data
        AttackNumber = 0;
        AttackingEntity.selectedAttack = AttackData[AttackNumber].attackData;
        AttackBaseColor = AttackData[AttackNumber].uiAttack.GetComponent<Image>().color;

        //Set Init Data
        AttackData[AttackNumber].uiAttack.GetComponent<Image>().color = new Color(46, 155, 62, 190);
        SpotLight.range = AttackingEntity.selectedAttack.Range;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire1") == 1)
        {
            AttackingEntity.Attack();
        }

        Movement();
        CheckSelectedAttack();
    }

    void Movement()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (moveDirection.sqrMagnitude > 1) moveDirection = moveDirection.normalized;
        PlayerRigidbody.MovePosition(transform.position + moveDirection * movementSpeed * Time.deltaTime);

        Vector3 rotationDirection = new Vector3(Input.GetAxis("JoystickRightHorizontal"), 0, Input.GetAxis("JoystickRightVertical"));

        if (!GameManager.Instance.Controller)
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

    void CheckSelectedAttack()
    {
        var _attackNumber = AttackNumber;
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            _attackNumber = (AttackNumber + 1);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            _attackNumber = (AttackNumber - 1);
        }
        if (_attackNumber < AttackData.Length && _attackNumber >= 0)
        {
            foreach (var attackData in AttackData) {
                attackData.uiAttack.GetComponent<Image>().color = AttackBaseColor;
            }

            AttackingEntity.selectedAttack = AttackData[_attackNumber].attackData;
            AttackData[AttackNumber].uiAttack.GetComponent<Image>().color = new Color32(46, 155, 62, 190); 
            SpotLight.range = AttackingEntity.selectedAttack.Range;
            AttackNumber = _attackNumber;
        }
    }
}

