using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickMoveController : MonoBehaviour
{
    Camera viewCam;
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
        target.Agent.acceleration = 240f;
        isMove = false;

        lineRenderer.enabled = false;
        anim = GetComponent<Animator>();
        path = new NavMeshPath();
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

    }
    void StoreAvailability()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("눌렀다");
            if (shopDetection.ShopAvailability)
            {
                RaycastHit hit;
                Ray ray = viewCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, shopLayer))
                {
                    shopDetection.ShopUse = true;
                        Debug.Log("상점이 열린다!");
                   
                }
            }
        }
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
                target.Agent.SetDestination(hitInfo.point);
                isMove = true;
            }
        }
    }
}
