using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//https://www.youtube.com/watch?v=5NTmxDSKj-Q
public struct ViewCastInfo
{
    public Vector3 point;
    public float dst;
    public float angle;

    public ViewCastInfo(Vector3 _point, float _dst, float _angle)
    {
        point = _point;
        dst = _dst;
        angle = _angle;
    }
}


public class FieldOfView : MonoBehaviour
{
    public GameObject fov;
    // 시야 영역의 반지름과 시야 각도
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;
    public float meshResolution;
    [Range(0, 100)] public float realMeshSize;

    //Mesh viewMesh;
    //public MeshFilter viewMeshFilter;

    //void Start()
    //{
    //    viewMesh = new Mesh();
    //    viewMesh.name = "View Mesh";
    //    viewMeshFilter.mesh = viewMesh;
    //}
    // y축 오일러 각을 3차원 방향 벡터로 변환한다.
    private void Awake()
    {
        if (this.gameObject.GetComponent<PhotonView>().IsMine == false)
            realMeshSize = 0;
    }


    private void Start()
    {
        fov.transform.localScale = Vector3.one * realMeshSize;
    }
    public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
    }

    void DrawFieldOfView()
    {
        // 샘플링할 점과 샘플링으로 나뉘어지는 각의 크기를 구한다.
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        // 샘플링한 점으로 향하는 좌표를 계산하여 stepCount개의 각도를 계산해서 선의 정보를 만든다.
        // 선이 가지고 있는 거리만큼의 지점의 point를 저장해준다.
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;

            Vector3 dir = DirFromAngle(angle, true);
            ViewCastInfo newViewCast = new ViewCastInfo(transform.position + dir * viewRadius, viewRadius, angle);
            viewPoints.Add(newViewCast.point);
        }

        ////메쉬를 생성한다.
        //int vertexCount = viewPoints.Count + 1;
        //Vector3[] vertices = new Vector3[vertexCount];
        //int[] triangles = new int[(vertexCount-2)*3];
        //vertices[0] = Vector3.zero;

        ////버텍스 인덱스 연결해주는 부분
        //for (int i = 0; i < vertexCount - 1; i++)
        //{
        //    vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
        //    if (i < vertexCount - 2)
        //    {
        //        triangles[i * 3] = 0;
        //        triangles[i * 3 + 1] = i + 1;
        //        triangles[i * 3 + 2] = i + 2;
        //    }
        //}

        ////메쉬 초기화
        //viewMesh.Clear();
        //viewMesh.vertices = vertices;
        //viewMesh.triangles = triangles;
        //viewMesh.RecalculateNormals(); //이부분은 기본 노멀 주는 부분인 듯.
    }

    //void LateUpdate()
    //{
    //    //DrawFieldOfView(); // 매 프레임 메쉬 생성
    //}
}
