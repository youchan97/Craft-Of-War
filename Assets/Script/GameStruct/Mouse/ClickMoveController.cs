using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickMoveController : MonoBehaviour
{
    [SerializeField]
    float camSpeed;
    bool camModSwitch;
    Camera viewCam;
    Transform cameraTrans; //이동시
    IControllable target;
    Vector3 clickPoint;
    public bool isMove;

    bool isAutoMove = false;
    Vector3 targetPoint;

    public LineRenderer lineRenderer;

    private NavMeshPath path;
    public Animator anim;


    public bool IsAutoMove
    { get => isAutoMove; set => isAutoMove = value; }

    public Vector3 TargetPoint
    {get => targetPoint; set => targetPoint = value;}

    public ShopDetection shopDetection;
    public LayerMask shopLayer;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        target = GetComponent<IControllable>();
        target.Agent = GetComponent<NavMeshAgent>();
        target.Agent.speed = target.MoveSpeed;
        viewCam = Camera.main; // mainCam 태그를 통해 가져옴
        cameraTrans = viewCam.transform.parent;

        target.Agent.acceleration = 240f;
        isMove = false;

        lineRenderer.enabled = false;
        anim = GetComponent<Animator>();
        path = new NavMeshPath();
        shopDetection = FindObjectOfType<ShopDetection>();
    }

    private void FixedUpdate()
    {
        // velocity 값이 변하면 run, 아니면 idle - 보람
        if (target.Agent.velocity != Vector3.zero)
            anim.SetBool("IsMove", true);
        else
            anim.SetBool("IsMove", false);
    }

    private void Update()
    {
        if (target.Agent.isStopped)
        {
            isAutoMove = false;
            isMove = false;
            target.Agent.SetDestination(this.transform.position);
        }

        if (lineRenderer.enabled)
        {
            NavMesh.CalculatePath(transform.position, targetPoint, NavMesh.AllAreas, path);

            if(path.corners.Length > 0)
            lineRenderer.positionCount = path.corners.Length - 1;

            for (int i = 0; i < path.corners.Length - 1; i++)
                lineRenderer.SetPosition(i, path.corners[i]);
        }

        if (IsAutoMove)
        {
            target.Agent.SetDestination(TargetPoint);
            lineRenderer.enabled = true;
            isMove = true;

            TryMove(); // 더 좋은 방법은 생각나지 않는다 ㅠ
        }
        else
        {
            TryMove();
        }

        if (target.Agent.remainingDistance <= 0.1f)
        {
            isAutoMove = false;
            isMove = false;
        }

        StoreAvailability();
        CamModChange();
    }
    void StoreAvailability()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            if (shopDetection.ShopAvailability)
            {
                RaycastHit hit;
                Ray ray = viewCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, shopLayer))
                {
                    shopDetection.ShopUse = true;              
                }
            }
        }
    }
    void CamModChange()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            camModSwitch = !camModSwitch;
        if(camModSwitch)
            cameraTrans.position = Vector3.MoveTowards(cameraTrans.position, new Vector3(transform.position.x, cameraTrans.position.y, transform.position.z - 50f), camSpeed * Time.deltaTime);      
    }
    void TryMove()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lineRenderer.enabled = false;
            clickPoint = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(clickPoint);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.transform.gameObject.layer == 14)
                {
                    target.Agent.SetDestination(hitInfo.point);
                    isMove = true;
                }
            }
        }
    }
}
