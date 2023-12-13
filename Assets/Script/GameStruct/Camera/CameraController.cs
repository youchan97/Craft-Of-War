using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCam;
    [SerializeField]
    private GameObject scrollCam;

    //움직이는 속도
    [SerializeField]
    private float speed;
    [SerializeField]
    private int movePointSize;
    //마우스 커서 좌표
    float x;
    float y;
    float wheelSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        wheelSpeed = 1500;//스크롤 속도
    }
    private void Update()
    {
        Move();
    }

    public void Move()
    {
        x = Input.mousePosition.x;
        y = Input.mousePosition.y;
        if (x < movePointSize)
        {
            if ((mainCam.transform.position.x > 20))
                mainCam.transform.Translate(-speed * Time.fixedDeltaTime * 100, 0, 0, Space.World);
        }
        if (x > Screen.width - movePointSize)
        {
            if ((mainCam.transform.position.x < 480))
                mainCam.transform.Translate(speed * Time.fixedDeltaTime * 100, 0, 0, Space.World);
        }
        if (y < movePointSize)
        {
            if ((mainCam.transform.position.z > -20))
                mainCam.transform.Translate(0, 0, -speed * Time.fixedDeltaTime * 100, Space.World);
        }
        if (y > Screen.height - movePointSize)
        {
            if ((mainCam.transform.position.z < 460))
                mainCam.transform.Translate(0, 0, speed * Time.fixedDeltaTime * 100, Space.World);
        }
        Scroll();
    }
    void Scroll()
    {
        //Clamp();
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        scrollCam.transform.Translate(0, 0, scrollWheel * Time.fixedDeltaTime * wheelSpeed);
    }

    void Clamp()
    {
        if (mainCam.transform.position.y < -40)
        {
            mainCam.transform.position = new Vector3(mainCam.transform.position.x, -40, mainCam.transform.position.z);
        }
        if (mainCam.transform.position.y > 30)
        {
            mainCam.transform.position = new Vector3(mainCam.transform.position.x, 30, mainCam.transform.position.z);
        }
    }
}
