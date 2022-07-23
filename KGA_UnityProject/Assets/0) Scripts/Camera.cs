using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [field: SerializeField] public float CamAngleSpeed { get; private set; }
    public void SetHP(float _CamAngleSpeed) { this.CamAngleSpeed = _CamAngleSpeed; }

    void Update()
    {
        LookAround();
    }

    public void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X") * CamAngleSpeed, Input.GetAxis("Mouse Y") * CamAngleSpeed);
        Vector3 camAngle = this.transform.rotation.eulerAngles;

        float camAngleX = camAngle.x - mouseDelta.y;

        if (camAngleX < 180f)
        {
            camAngleX = Mathf.Clamp(camAngleX, -1f, 70f);
        }
        else
        {
            camAngleX = Mathf.Clamp(camAngleX, 340f, 361f);
        }

        this.transform.rotation = Quaternion.Euler(camAngleX, camAngle.y + mouseDelta.x, camAngle.z);
        // camAngle.x - mouseDelta.y << 국내에서 흔하게 사용되는 조작방법으로 사용자에 따라 익숙함이 다를 수 있다
        // +, - 값의 두가지 설정을 두어서 원하는 조작방법으로 조작할 수 있도록 옵션에 추가하자
    }
}
