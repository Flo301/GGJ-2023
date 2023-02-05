using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public float rotationSpeed = 10.0f;
    public Light SpotLight;
    private Rigidbody PlayerRigidbody;
    public AttackingEntity AttackingEntity { get; private set; }
    public GameObject Ground;
    private Animator PlayerAnimator;

    //Attack Data
    public List<PlayerAttack> Attacks;
    private PlayerAttack selectedAttack
    {
        get => Attacks[selectedAttackIndex];
    }
    private int selectedAttackIndex;

    void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        AttackingEntity = GetComponent<AttackingEntity>();
        PlayerAnimator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator.SetBool("Walking", false);
        PlayerAnimator.SetBool("Meele Short", false);
        PlayerAnimator.SetBool("Meele Long", false);
        PlayerAnimator.SetBool("Throwable", false);
        PlayerAnimator.SetBool("Trigger Single", false);
        PlayerAnimator.SetBool("Trigger Double", false);

        //Get Base Data
        selectedAttackIndex = -1;
        ChangeWeapon(true);
    }

    // Update is called once per frame
    void Update()
    {
        CheckSelectedAttack();
        CheckWeapon();

        if (Input.GetButtonDown("Jump"))
        {
            PlayerAnimator.SetTrigger("Flipping Bird");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            PlayerAnimator.SetBool("Rapid Fire", true);
        }

        if (Input.GetButton("Fire1"))
        {
            AttackingEntity.Attack();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            PlayerAnimator.SetBool("Rapid Fire", false);
        }

        Movement();
    }

    void CheckWeapon()
    {
        if (selectedAttack.Name == "Meele")
        {
            PlayerAnimator.SetBool("Meele Short", true);
            PlayerAnimator.SetBool("Throwable", false);
            PlayerAnimator.SetBool("Trigger Single", false);
            PlayerAnimator.SetBool("Trigger Double", false);
        }
        else if (selectedAttack.Name == "Throwable")
        {
            PlayerAnimator.SetBool("Meele Short", false);
            PlayerAnimator.SetBool("Throwable", true);
            PlayerAnimator.SetBool("Trigger Single", false);
            PlayerAnimator.SetBool("Trigger Double", false);
        }
        else if (selectedAttack.Name == "Flare Gun")
        {
            PlayerAnimator.SetBool("Meele Short", false);
            PlayerAnimator.SetBool("Throwable", false);
            PlayerAnimator.SetBool("Trigger Single", true);
            PlayerAnimator.SetBool("Trigger Double", false);
        }
        else if (selectedAttack.Name == "Flame Thrower")
        {
            PlayerAnimator.SetBool("Meele Short", false);
            PlayerAnimator.SetBool("Throwable", false);
            PlayerAnimator.SetBool("Trigger Single", false);
            PlayerAnimator.SetBool("Trigger Double", true);
        }
    }

    void Movement()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (moveDirection.sqrMagnitude > 1) moveDirection = moveDirection.normalized;
        PlayerRigidbody.MovePosition(transform.position + moveDirection * movementSpeed * Time.deltaTime);

        PlayerAnimator.SetBool("Walking", moveDirection.sqrMagnitude > 0f);
        PlayerAnimator.SetFloat("Speed", moveDirection.sqrMagnitude);

        Vector3 rotationDirection = new Vector3(Input.GetAxis("JoystickRightHorizontal"), 0, Input.GetAxis("JoystickRightVertical"));

        if (!GameManager.Instance.Controller)
        {
            Ray target = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool success = false;
            RaycastHit hit;
            if (null != Ground)
            {
                success = Ground.GetComponent<Collider>().Raycast(target, out hit, float.PositiveInfinity);
            }
            else
            {
                success = Physics.Raycast(target, out hit);
            }
            if (success)
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
        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetButtonDown("NextWeapon"))
        {
            ChangeWeapon(true);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetButtonDown("PrevWeapon"))
        {
            ChangeWeapon(false);
        }
    }

    public void ChangeWeapon(bool up)
    {
        int index = selectedAttackIndex;
        while (true)
        {
            index = (index + (up ? 1 : -1)) % Attacks.Count;
            index = index < 0 ? Attacks.Count - 1 : index;
            if (Attacks[index].isUnlocked)
                break;
        }

        //update weapon infos on change
        if (selectedAttackIndex != index)
        {
            selectedAttackIndex = index;

            //Reset Color and Mesh for all weapons
            foreach (var attack in Attacks)
            {
                Image img = attack.uiAttack.GetComponent<Image>();
                if (null != img)
                {
                    img.color = attack.isUnlocked ? new Color32(100, 100, 100, 190) : new Color32(165, 0, 0, 190);
                }

                attack.WeaponMesh.SetActive(false);
            }

            //Update properties for current weapon
            AttackingEntity.selectedAttack = selectedAttack.attackData;
            selectedAttack.WeaponMesh.SetActive(true);
            selectedAttack.uiAttack.GetComponent<Image>().color = new Color32(46, 155, 62, 190);
            SpotLight.range = selectedAttack.attackData.Range;
        }
    }
}
