using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    [SerializeField]
    private Transform eyes;
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float sensitivity = 1;
    [SerializeField]
    private float eyesLimitAngle = 90;
    private float eyesAngle;

    [SerializeField]
    private Rigidbody rigidbody;
    //[SerializeField]
    //private CharacterController characterController;
    private Vector3 motion;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        LookRotation();
        ReadMoveInput();
    }

    private void FixedUpdate()
    {
        MovePosition();
    }

    private void MovePosition()
    {
        if (motion == Vector3.zero)
            return;

        motion *= speed * Time.fixedDeltaTime;
        //rigidbody.MovePosition(rigidbody.position + motion * speed * Time.fixedDeltaTime);
        motion = transform.forward * motion.z + transform.right * motion.x;

        //transform.position += motion;
        rigidbody.MovePosition(rigidbody.position + motion);
    }

    private void ReadMoveInput()
    {
        float deltaX = Input.GetAxis("Horizontal");
        float deltaY = Input.GetAxis("Vertical");

        motion = new Vector3(deltaX, 0f, deltaY);

        if (motion.magnitude > 1)
            motion = motion.normalized;
    }

    private void LookRotation()
    {
        //Horisontal
        float deltaX = Input.GetAxis("Mouse X") * sensitivity;
        transform.Rotate(0f, deltaX, 0f);

        //Vertical
        eyesAngle -= Input.GetAxis("Mouse Y") * sensitivity;

        eyesAngle = Math.Clamp(eyesAngle, -eyesLimitAngle, eyesLimitAngle);

        eyes.localRotation = Quaternion.Euler(eyesAngle, 0f, 0f);
    }
}
