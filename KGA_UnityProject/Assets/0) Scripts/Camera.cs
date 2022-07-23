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
        // camAngle.x - mouseDelta.y << �������� ���ϰ� ���Ǵ� ���۹������ ����ڿ� ���� �ͼ����� �ٸ� �� �ִ�
        // +, - ���� �ΰ��� ������ �ξ ���ϴ� ���۹������ ������ �� �ֵ��� �ɼǿ� �߰�����
    }
}
