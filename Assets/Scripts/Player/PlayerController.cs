using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Transform weaponSocket;
    [SerializeField] private GameObject packagePanel;

    [SerializeField] private float runAcceleration = 0.15f;
    [SerializeField] private float runSpeed = 3f;
    [SerializeField] private float sprintSpeed = 5f;
    [SerializeField] private float drag = 0.1f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float groundedBuffer = 0.05f;
    
    //[SerializeField] private GameObject TrailGroup;

    public LayerMask rayLayer;

    [Header("Camera")]
    public float lookSenseH = 0.1f;
    [SerializeField] private float lookSenseV = 0.1f;
    [SerializeField] private float lookLimitV = 80f;


    public bool canMove;
    public Collider weaponCollider;
    

    private PlayerLocomotionInput _playerLocomotionInput;
    private Animator _animator;
    private float moveSpeedMax;
    private Vector2 _cameraRotation = Vector2.zero;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 horizontalVelocity;
    private float smoothVelocity = 0.25f;
    private float falltimeMax = 0f;
    private float fallTimer = 0f;
    private bool isGrounded = true;
    private bool isGrounded1;
    private bool isGrounded2;
    private float groundedTime;
    private IInteractable targetObject;


    private void Awake()
    {
        PackageLocalData.Instance.Reset();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        _animator = GetComponent<Animator>();
        moveSpeedMax = runSpeed;
        //TrailGroup.SetActive(false);
        _animator.SetBool("isArmed", false);
        canMove = true;
        packagePanel.SetActive(false);
    }

    private void Update()
    {
        OpenPackage();

        if (packagePanel.activeSelf)
        {
            return;
        }
        Interaction();
        Attack();
        

        HandleGrounded();
        if (canMove)
        {
            HandleMovement();
            HandleJump();
        }
        else
        {
            _velocity.x = 0;
            _velocity.z = 0;
        }

        _characterController.Move(_velocity * Time.deltaTime);

        //根据移动速度的判断移动状态
        float moveSpeed = horizontalVelocity.magnitude / sprintSpeed;

        if (moveSpeed > 0.01f)
        {
            _animator.SetFloat("inputX", moveSpeed);
            
        }
        else
        {
            _animator.SetFloat("inputX", 0f);
        }

        bool isArmed = !string.IsNullOrEmpty(PackageLocalData.Instance.weaponRightSlot);
        if(isArmed == true)
        {
            _animator.SetBool("isArmed", true);
        }
    }

    private void LateUpdate()
    {
        if (packagePanel.activeSelf)
        {
            return;
        }
        //摄像机的转向，角色不转
        _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

        _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
    }

    private void HandleMovement()
    {
        //_velocity = _characterController.velocity;

        if (isGrounded)
        {
            //判断是否按下冲刺键，并且有移动输入
            if (_playerLocomotionInput.SprintPressed && horizontalVelocity.magnitude > 0f)
            {
                moveSpeedMax = Mathf.SmoothDamp(moveSpeedMax, sprintSpeed, ref smoothVelocity, 0.1f);
                //TrailGroup.SetActive(true);
            }
            else
            {
                moveSpeedMax = Mathf.SmoothDamp(moveSpeedMax, runSpeed, ref smoothVelocity, 0.1f);
                //TrailGroup.SetActive(false);
            }
        }
        

        //根据摄像机方向计算移动方向
        Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
        Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;

        Vector3 movementDelta = movementDirection * runAcceleration * Time.deltaTime;
        _velocity.x += movementDelta.x;
        _velocity.z += movementDelta.z;

        horizontalVelocity = new Vector3(_velocity.x, 0f, _velocity.z);
        Vector3 currentDrag = horizontalVelocity.normalized * drag * Time.deltaTime;
        horizontalVelocity = (horizontalVelocity.magnitude > currentDrag.magnitude) ? horizontalVelocity - currentDrag : Vector3.zero;

        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, moveSpeedMax);

           
        _velocity.x = horizontalVelocity.x;
        _velocity.z = horizontalVelocity.z;

        //角色转向
        if (movementDirection.magnitude > 0.01f)
        {

            float targetYRotation = Quaternion.LookRotation(movementDirection).eulerAngles.y;

            float smoothYRotation = Mathf.LerpAngle(transform.eulerAngles.y, targetYRotation, 13f * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, smoothYRotation, 0f);
        }
        
    }

    private void HandleJump()
    {
        _animator.SetBool("isGrounded", isGrounded);
        _animator.SetBool("isFalling", false);

        //垂直速度受重力影响衰减
        _velocity.y -= gravity * Time.deltaTime;

        if (_velocity.y <= 0f && isGrounded)
        {
            _velocity.y = 0f;
            _animator.SetBool("isFalling", false);
            fallTimer = 0f;
        }


        if (_playerLocomotionInput.JumpPressed && isGrounded )
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);

            groundedTime = -groundedBuffer;

            fallTimer = _velocity.y / gravity;
            _animator.SetTrigger("isJumping");
        }

        if(!isGrounded && _velocity.y < 0f && fallTimer > falltimeMax)
        {
            _animator.SetBool("isFalling", true);
        }
    }

    private void HandleGrounded()
    {

        isGrounded1 = _characterController.isGrounded;

        if (Physics.Raycast(transform.position, Vector3.down, 0.2f))
        {
            isGrounded2 = true;
        }
        else
        {
            isGrounded2 = false;
        }

        if (isGrounded1 || isGrounded2)
        {
            groundedTime = groundedBuffer;
        }
        else
        {
            groundedTime -= Time.deltaTime;
        }

        isGrounded = groundedTime > 0f;
    }

    private void Interaction()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        

        // 这里输入检测一定要在射线检测前面，不然摁下互动键后InterestingPressed一直都是true，没有走“读后即焚”语句块
        if (_playerLocomotionInput.InteractPressed)
        {
            //Debug.Log($"按[E]拾取{PickedWeapon.name}武器");
            if (Physics.Raycast(ray, out hit, 3.6f, rayLayer))
            {
                targetObject = hit.collider.GetComponent<IInteractable>();
                if(targetObject != null)
                {
                    targetObject.OnInteract(gameObject);
                    _playerLocomotionInput.InteractPressed = false;
                }
                //pickedWeapon = hit.collider.gameObject;

                //pickedWeapon.transform.SetParent(weaponSocket);


                //// 这一块collider有问题，怎么都获取不到刀上的collider，无法设为isTrriger
                //// 建议最后再写这一块，早知如此玩家就也用状态机了TAT

                //Collider[] allCols = pickedWeapon.GetComponentsInChildren<Collider>();
                //pickedWeapon.transform.localPosition = Vector3.zero;
                //pickedWeapon.transform.localRotation = Quaternion.Euler(5f, -90f, -75f);

                //foreach (var col in allCols)
                //{   
                //    if (col.gameObject == pickedWeapon)
                //    {
                //        col.enabled = false;
                //    }
                //    else
                //    {
                //        col.enabled = false;
                //        weaponCollider = col;
                //    }
                //}

                //_playerLocomotionInput.AttackPressed = false;
                //_animator.SetBool("isArmed", true);
            }
        }
    }

    private void Attack()
    {
        if(weaponSocket.childCount != 0 && _playerLocomotionInput.AttackPressed  && packagePanel.activeSelf)
        {
            Debug.Log("玩家正在攻击");
            _animator.SetTrigger("isAttack");

            // 更新攻击和受击不可移动功能
            _velocity = Vector3.zero;
            canMove = false;
            weaponCollider.enabled = true;
            weaponCollider.isTrigger = true;
            
        }
        
    }

    private void OpenPackage()
    {
        if (_playerLocomotionInput.OpenPressed)
        {
            bool isOpen = packagePanel.activeSelf;
            bool newState = !isOpen;

            if (newState)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                canMove = false;
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                canMove = true;
            }

            packagePanel.SetActive(newState);

            
        }
    }


}
