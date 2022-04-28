using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    static FreeCamera instance;

    float zoom = 1f;
    Transform swivel, stick;
    public float stickMinZoom, stickMaxZoom;
    public float swivelMinZoom, swivelMaxZoom;
    public float moveSpeedMinZoom, moveSpeedMaxZoom;
    private const int zoomWeakenFactor = 3;
    public float rotationnSpeed;
    float rotationAngle;

    void Awake() {
        swivel = transform.GetChild(0);
        stick = swivel.GetChild(0);
    }
    void OnEnable() {
        instance = this;
    }
    public static bool Locked
    {
        set {
            instance.enabled = !value;
        }
    }
    void Update() {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDelta != 0) {
            AdjustZoom(zoomDelta/zoomWeakenFactor);
        }
        float rotationDelta = Input.GetAxis("Rotation");
        if (rotationDelta != 0f) {
            AdjustRotation(rotationDelta);
        }
        float xDelta = Input.GetAxis("Horizontal");
        float zDelta = Input.GetAxis("Vertical");
        if (xDelta != 0f || zDelta != 0f) {
            AdjustPosition(xDelta, zDelta);
        }
    }
    public static void ValidatePosition() {
        instance.AdjustPosition(0f, 0f);
    }
    void AdjustPosition(float xDelta, float zDelta) {
        Vector3 direction = transform.localRotation *
                            new Vector3(xDelta, 0f, zDelta).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));

        float distance = Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) *
                         Time.deltaTime;

        Vector3 position = transform.localPosition;
        position += direction * damping * distance;
        transform.localPosition = position;

    }
    void AdjustRotation(float delta) {
        rotationAngle += delta * rotationnSpeed * Time.deltaTime;
        if (rotationAngle < 0f) {
            rotationAngle += 360f;
        }
        else if (rotationAngle >= 360f) {
            rotationAngle -= 360f;
        }
        transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
    }

    void AdjustZoom(float delta) {
        zoom = Mathf.Clamp01(zoom + delta);
        float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
        stick.localPosition = new Vector3(0f, 0f, distance);
        float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
        swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }
    //Vector3 clampPosition(Vector3 position) {
    //    float xMax = (grid.cellCountX - 0.5f) * (2f * HexMetrics.innerRadius);
    //    position.x = Mathf.Clamp(position.x, 0f, xMax);

    //    float zMax = (grid.cellCountZ - 1) * (1.5f * HexMetrics.outerRadius);

    //    position.z = Mathf.Clamp(position.z, 0f, zMax);

    //    return position;
    //}
}
