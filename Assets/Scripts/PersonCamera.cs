using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonCamera : MonoBehaviour
{
    //https://code.tutsplus.com/tutorials/unity3d-third-person-cameras--mobile-11230 Key readout camera
    //Decommisioned

    //https://www.youtube.com/watch?v=xcn7hz7J7sI PlayerFollow

    public Transform PlayerTransform;

    [SerializeField] private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    public bool LookAtPlayer = false;

    public bool RotateAroundPlayer = true;

    public bool RotateVerticalSupport = true;

    public float RotationsSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - PlayerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }

    public static Quaternion ClampQuaternion(Quaternion quaternion, float min, float max)
    {
        Quaternion result = new Quaternion();
        if (quaternion.x > max) quaternion.x = max;
        if (quaternion.x < min) quaternion.x = min;
        result = quaternion;

        return result;
    }

    [SerializeField] Quaternion camTurnAngle;
    [SerializeField] Quaternion camTurnAngleY;
    [SerializeField] Vector3 camNowAngle;
    [SerializeField] Quaternion decamAngle;
    [SerializeField] float camYmin = -80f; //from Standard asset Free look cam
    [SerializeField] float camYmax = 80f;
    [SerializeField] float mouseXdelta;
    [SerializeField] float mouseYdelta;
    [SerializeField] float syncX;
    [SerializeField] float syncY;
    public Vector3 camMoveAngle;
    public bool useMoveVersion;
    private void FixedUpdate()
    {
        mouseXdelta = Input.GetAxis("Mouse X");
        mouseYdelta = Input.GetAxis("Mouse Y");
        syncX += mouseXdelta *RotationsSpeed;
        syncY += mouseYdelta *RotationsSpeed;
        syncY = Mathf.Clamp(syncY, camYmin, camYmax);
        decamAngle = Quaternion.Euler(syncX, syncY, 0);
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            //camTurnAngle.Normalize();
            if (RotateAroundPlayer)
            {
                if (!useMoveVersion)
                {
                    if (RotateVerticalSupport && (transform.rotation.x > camYmin && transform.rotation.x < camYmax))
                    {
                        //https://forum.unity.com/threads/simple-rotation-of-the-camera-with-the-mouse-around-the-player.470278/ Camera Follow Script
                        camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationsSpeed, Vector3.up) * Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * RotationsSpeed, Vector3.left);
                        camTurnAngleY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * RotationsSpeed, Vector3.left);
                    }
                    else
                    {
                        camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationsSpeed, Vector3.up);
                        camTurnAngleY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * RotationsSpeed, Vector3.left);
                    }
                    camNowAngle = camTurnAngle.eulerAngles;
                    _cameraOffset = camTurnAngle * _cameraOffset;
                }
                else //This slides camera instead of orbiting
                {
                    if (RotateVerticalSupport)
                    {
                        camMoveAngle = new Vector3(Input.GetAxis("Mouse X") * RotationsSpeed, Input.GetAxis("Mouse Y") * RotationsSpeed);
                    }
                    else
                    {
                        camMoveAngle = new Vector3(Input.GetAxis("Mouse X") * RotationsSpeed, 0);
                    }
                    _cameraOffset += camMoveAngle;
                }
            }
        }

        Vector3 newPos = PlayerTransform.position + _cameraOffset;
        Vector3 verificatePos = Vector3.Slerp(transform.position, newPos, SmoothFactor);
    
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        if (LookAtPlayer || RotateAroundPlayer)
            transform.LookAt(PlayerTransform);
    }
}
