using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class newPersonCamera : MonoBehaviour
{
    public Transform target;
    public Camera Cam3rdPerson;
    public Camera Cam1stPerson;

    public float ControlHorizontal;
    public float ControlVertical;
    public float ControlScroll;

    public Vector3 _LocalRotation;
    public float _CameraDistance = 10f;

    public bool CameraDisabled;
    public float MouseSensitivity = 4f;
    public float ScrollSensitvity = 2f;
    public float OrbitDampening = 10f;
    public float MoveDampening = 10f;
    public float ScrollDampening = 6f;
    public float ConstrainUp = 90f;
    public float ConstrainDown = -90f;

    public enum SelectCamera { Person3rd, Person1st};
    SelectCamera selectCamera;

    private void Awake()
    {
        GameObject[] PersonCams = GameObject.FindGameObjectsWithTag("PersonCamera");

        if (PersonCams.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            GameObject findSHanpe = GameObject.FindGameObjectWithTag("Player");
            if (findSHanpe)
            {
                target = findSHanpe.transform;
            }
        }
    }

    private void FixedUpdate()
    {
        //Emergent Saga
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, MoveDampening * Time.deltaTime);
        }
        ControlHorizontal = CrossPlatformInputManager.GetAxis("Mouse X");
        ControlVertical = CrossPlatformInputManager.GetAxis("Mouse Y");
        ControlScroll = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");

        if (!CameraDisabled)
        {
            //Rotation of the Camera based on Mouse Coordinates
            if (ControlHorizontal != 0 || ControlVertical != 0)
            {
                _LocalRotation.x += ControlHorizontal * MouseSensitivity;
                _LocalRotation.y += ControlVertical * MouseSensitivity;

                //Clamp the y Rotation to horizon and not flipping over at the top
                if (_LocalRotation.y < ConstrainDown)
                    _LocalRotation.y = ConstrainDown;
                else if (_LocalRotation.y > ConstrainUp)
                    _LocalRotation.y = ConstrainUp;
            }
            //Zooming Input from our Mouse Scroll Wheel
            if (ControlScroll != 0f)
            {
                float ScrollAmount = ControlScroll * ScrollSensitvity;

                ScrollAmount *= _CameraDistance * 0.3f;

                _CameraDistance += ScrollAmount * -1f;

                _CameraDistance = Mathf.Clamp(_CameraDistance, 1.5f, 100f);
            }
        }

        //Actual Camera rigg
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, QT, Time.deltaTime * OrbitDampening);
        if (Cam3rdPerson.transform.localPosition.z != _CameraDistance * -1f)
        {
            Cam3rdPerson.transform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(Cam3rdPerson.transform.localPosition.z, _CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }
    }

    public void setTarget()
    {
        GameObject findSHanpe = GameObject.FindGameObjectWithTag("Player");
        if (findSHanpe)
        {
            target = findSHanpe.transform;
        }
    }

    public void setTarget(GameObject whichWho)
    {
        target = whichWho.transform;
    }
}
