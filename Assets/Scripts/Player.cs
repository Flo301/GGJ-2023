using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public float rotationSpeed = 10.0f;
    public Light SpotLight;
    private Rigidbody PlayerRigidbody;
    private AttackingEntity AttackingEntity;
    public List<PlayerAttack> AttackData;
    private Color AttackBaseColor;
    private int AttackNumber;
    private Animator PlayerAnimator;
    public GameObject Ground;
    private PlayerAttack selectedAttack;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        AttackingEntity = GetComponent<AttackingEntity>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        PlayerAnimator.SetBool("Walking", false);
        PlayerAnimator.SetBool("Meele Short", false);
        PlayerAnimator.SetBool("Meele Long", false);
        PlayerAnimator.SetBool("Throwable", false);
        PlayerAnimator.SetBool("Trigger Single", false);
        PlayerAnimator.SetBool("Trigger Double", false);

        //Get Base Data
        AttackNumber = 0;
        selectedAttack = AttackData[AttackNumber];
        AttackingEntity.selectedAttack = selectedAttack.attackData;
        AttackBaseColor = AttackData[AttackNumber].uiAttack.GetComponent<Image>().color;

        //Set Init Data
        AttackData[AttackNumber].uiAttack.GetComponent<Image>().color = new Color(46, 155, 62, 190);
        SpotLight.range = AttackingEntity.selectedAttack.Range;
    }

    // Update is called once per frame
    void Update()
    {
        CheckSelectedWeapon();
        CheckWeapon();

        if (Input.GetButtonDown("Jump"))
        {
            PlayerAnimator.SetTrigger("Flipping Bird");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            AttackingEntity.Attack();
            PlayerAnimator.SetTrigger("Fire");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            PlayerAnimator.SetBool("Rapid Fire", true);
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

    /*     void CheckSelectedAttack()
        {
            var _attackNumber = AttackNumber;
            if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetButtonDown("NextWeapon"))
            {
                _attackNumber = (AttackNumber + 1);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetButtonDown("PrevWeapon"))
            {
                _attackNumber = (AttackNumber - 1);
            }
            if (_attackNumber < AttackData.Length && _attackNumber >= 0)
            {
                foreach (var attackData in AttackData)
                {
                    Image img = attackData.uiAttack.GetComponent<Image>();
                    if (null != img)
                    {
                        img.color = AttackBaseColor;
                    }

                    if (!attackData.isUnlocked)
                    {
                        attackData.uiAttack.GetComponent<Image>().color = new Color32(165, 0, 0, 255);
                    }

                    attackData.WeaponMesh.SetActive(false);
                }

                AttackNumber = _attackNumber;


                selectedAttack = AttackData[AttackNumber];

                AttackingEntity.selectedAttack = selectedAttack.attackData;

                selectedAttack.WeaponMesh.SetActive(true);
                selectedAttack.uiAttack.GetComponent<Image>().color = new Color32(46, 155, 62, 190);

                SpotLight.range = AttackingEntity.selectedAttack.Range;
            }
        } */

    void CheckSelectedWeapon()
    {
        var availableWeapons = new List<PlayerAttack>();

        foreach (var attack in AttackData)
        {
            Image img = attack.uiAttack.GetComponent<Image>();
            if (null != img)
            {
                img.color = AttackBaseColor;
            }

            if (attack.isUnlocked)
            {
                availableWeapons.Add(attack);
            }
            else
            {
                attack.uiAttack.GetComponent<Image>().color = new Color32(165, 0, 0, 255);
            }

            attack.WeaponMesh.SetActive(false);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetButtonDown("NextWeapon"))
        {
            if (AttackNumber + 1 < availableWeapons.Count)
            {
                AttackNumber = AttackNumber + 1;
                selectedAttack = availableWeapons[AttackNumber];
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetButtonDown("PrevWeapon"))
        {
            if (AttackNumber - 1 >= 0)
            {
                AttackNumber = AttackNumber - 1;
                selectedAttack = availableWeapons[AttackNumber];
            }
        }

        AttackingEntity.selectedAttack = selectedAttack.attackData;
        selectedAttack.WeaponMesh.SetActive(true);
        selectedAttack.uiAttack.GetComponent<Image>().color = new Color32(46, 155, 62, 190);

        SpotLight.range = selectedAttack.attackData.Range;
    }

}


