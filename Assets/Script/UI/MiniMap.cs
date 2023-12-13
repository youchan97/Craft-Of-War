using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour, IPointerDownHandler
{
    //카메라
    public Camera miniMapCam;

    // 움직일 애
    public GameObject moveCam;
    GameObject character;

    public RectTransform miniMapRect;
    public float camY = 50f;
    public float camWeightZ = 30f;

    ClickMoveController clickMoveController = null;
    LineRenderer lineRenderer = null;

    private void Start()
    {
        character = GameManager.Instance.PlayerHero.gameObject;
        clickMoveController = character.GetComponent<ClickMoveController>();
        lineRenderer = Camera.main.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 5;
    }

    private void Update()
    {
        DrawCameraLine();
    }

    void DrawCameraLine()
    {
        var camera = Camera.main;
        Vector3[] frustumCorners = new Vector3[4];
        camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), camera.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);

        RaycastHit hit;
        for (int i = 0; i < 4; i++)
        {
            var worldSpaceCorner = camera.transform.TransformVector(frustumCorners[i]);
            if (Physics.Raycast(camera.transform.position, worldSpaceCorner, out hit, Mathf.Infinity))
            {
                lineRenderer.SetPosition(i, hit.point);
            }
        }
        lineRenderer.SetPosition(4, lineRenderer.GetPosition(0));
    }

    public Vector2 GetMousePointForMiniMap(PointerEventData eventData)
    {
        Vector2 point  = Vector2.zero;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(miniMapRect,
        eventData.pressPosition, eventData.pressEventCamera, out point)) 
        {
            Texture texture = GetComponent<RawImage>().texture;
            Rect rect = GetComponent<RawImage>().rectTransform.rect;

            if (texture != null && rect != null)
            {
                float coordX = Mathf.Clamp(0, (((point.x - rect.x) * texture.width) / rect.width), texture.width);
                float coordY = Mathf.Clamp(0, (((point.y - rect.y) * texture.height) / rect.height), texture.height);

                float calX = coordX / texture.width;
                float calY = coordY / texture.height;

                point = new Vector2(calX, calY);
            }
        }
        return point;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
            TryMoveCamera(GetMousePointForMiniMap(eventData));

        if (eventData.button == PointerEventData.InputButton.Right)
            TryMoveCharacter(GetMousePointForMiniMap(eventData));
    }

    public void TryMoveCamera(Vector2 point)
    {
        Ray MapRay = miniMapCam.ScreenPointToRay(new Vector2(point.x * miniMapCam.pixelWidth,
            point.y * miniMapCam.pixelHeight));

        RaycastHit miniMapHit;

        if (Physics.Raycast(MapRay, out miniMapHit, Mathf.Infinity))
        {
            if (miniMapCam != null)
                moveCam.transform.position = new Vector3(Mathf.Clamp(miniMapHit.point.x, 35f, 460f),
                    camY, Mathf.Clamp(miniMapHit.point.z - camWeightZ, -20f, 410f));
        }
    }

    public void TryMoveCharacter(Vector2 point)
    {
        Ray MapRay = miniMapCam.ScreenPointToRay(new Vector2(point.x * miniMapCam.pixelWidth,
        point.y * miniMapCam.pixelHeight));

        RaycastHit miniMapHit;

        if (Physics.Raycast(MapRay, out miniMapHit, Mathf.Infinity))
        {
            clickMoveController.IsAutoMove = true;
            clickMoveController.TargetPoint =  new Vector3(miniMapHit.point.x, 0, miniMapHit.point.z);
        }
    }
}