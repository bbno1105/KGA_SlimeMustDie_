using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPUI : MonoBehaviour
{
    Camera camera;

    Monster monster;
    Slider slider;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        monster = this.transform.parent.GetComponent<Monster>();
        slider = this.transform.GetComponentInChildren<Slider>();
    }

    void Update()
    {
        this.transform.LookAt(this.transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.down);
        slider.value = monster.HP / (float)monster.MaxHP;
    }
}
