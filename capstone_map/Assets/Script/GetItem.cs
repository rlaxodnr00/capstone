using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{

    //������ ���� ���� ��ũ��Ʈ

    private RaycastHit rayHit; //ray �浹 ����
    private Ray ray; //������ �ʴ� ����

    //��� �Ÿ��� ��ü���� ������ ������
    //private float maxRayDistance = 5000f;


    // Update is called once per frame
    void Update()
    {
        ObjectHit();
    }

    void ObjectHit()
    {
        //ViewportPointToRay << ī�޶� �������� ray ����. �ش� ���Ͱ����� ī�޶� �߾ӿ� ������
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Ray �߻�

        if(Physics.Raycast(ray, out rayHit))
        {
            

            if (rayHit.collider.gameObject.tag == "FlashLight") //���� �浹ü�� ������Ʈ �±װ� �������̸�
            {
                Debug.Log("������");
                //FlashLight.GetLight();
            }
        }
    }
}
