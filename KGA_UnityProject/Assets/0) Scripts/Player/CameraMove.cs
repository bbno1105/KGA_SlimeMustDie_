using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraMove : MonoBehaviour
{

    [field: SerializeField] public float CamAngleSpeed { get; private set; }
    public void SetHP(float _CamAngleSpeed) { this.CamAngleSpeed = _CamAngleSpeed; }

    GameObject camera;
    Vector3 cameraPOS;

    public GameObject CharacterObj;

    float defaultCameraDistance;

    void Start()
    {
        camera = this.transform.GetChild(0).gameObject;
        defaultCameraDistance = (this.transform.position - camera.transform.position).magnitude;
        cameraPOS = camera.transform.localPosition;
    }

    void Update()
    {
        LookAround();
        ObstacleMove();

        Vector3 nowCameraDistance = this.transform.position - camera.transform.position;

        if (nowCameraDistance.magnitude < 1.5f)
        {
            CharacterObj.SetActive(false);

            //for (int i = 0; i < meshRenderer.Length; i++)
            //{
            //    meshRenderer[i].material.SetFloat("Tweak_transparency", -0.5f);
            //}
        }
        else
        {
            CharacterObj.SetActive(true);

            //for (int i = 0; i < meshRenderer.Length; i++)
            //{
            //    meshRenderer[i].material.SetFloat("Tweak_transparency", 0f);
            //}
        }

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
            camAngleX = Mathf.Clamp(camAngleX, 300f, 361f);
        }

        this.transform.rotation = Quaternion.Euler(camAngleX, camAngle.y + mouseDelta.x, camAngle.z);
        // camAngle.x - mouseDelta.y << 국내에서 흔하게 사용되는 조작방법으로 사용자에 따라 익숙함이 다를 수 있다
        // +, - 값의 두가지 설정을 두어서 원하는 조작방법으로 조작할 수 있도록 옵션에 추가하자
    }

    void ObstacleMove()
    {
        RaycastHit hit;
        if(Physics.Raycast(this.transform.position,(camera.transform.position-this.transform.position).normalized, out hit, defaultCameraDistance))
        {
            if (hit.transform.gameObject != camera && hit.transform.gameObject.tag != "Monster" && hit.transform.gameObject.tag != "DamagedMonster" && hit.transform.gameObject.tag != "Player" && hit.transform.gameObject.tag != "Trap")
            {
                camera.transform.position = hit.point;
            }
        }
        else
        {
            camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, cameraPOS, Time.deltaTime * 5f);
        }

        if((this.transform.position - camera.transform.position).magnitude < 0.1f)
        {
        }
    }
}
