using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{

    //아이템 습득 위한 스크립트

    private RaycastHit rayHit; //ray 충돌 판정
    private Ray ray; //보이지 않는 광선

    //어느 거리의 물체까지 반응할 것인지
    //private float maxRayDistance = 5000f;


    // Update is called once per frame
    void Update()
    {
        ObjectHit();
    }

    void ObjectHit()
    {
        //ViewportPointToRay << 카메라 시점에서 ray 생성. 해당 벡터값으로 카메라 중앙에 생성함
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Ray 발생

        if(Physics.Raycast(ray, out rayHit))
        {
            

            if (rayHit.collider.gameObject.tag == "FlashLight") //닿은 충돌체의 오브젝트 태그가 손전등이면
            {
                Debug.Log("손전등");
                //FlashLight.GetLight();
            }
        }
    }
}
