using UnityEngine;
using UnityEngine.Events;

public class ObjectDetector : MonoBehaviour
{
    [System.Serializable]
    public class RaycastEvent : UnityEvent<Transform> { } 

    [HideInInspector]
    public RaycastEvent raycastEvent = new RaycastEvent();  //�̺�Ʈ Ŭ���� �ν��Ͻ� ���� �� �޸� �Ҵ�

    private Camera mainCamera; //���� �����ϱ� ���� Camera
    private Ray ray; //������ ���� ���� ������ ���� Ray
    private RaycastHit hit; //������ �ε��� ������Ʈ ���� ������ ���� RaycastHit

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //���콺 ���� ��ư�� ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            //ī�޶� ��ġ���� ȭ���� ���콺 ��ġ�� �����ϴ� ���� ����
            //ray.origin : ������ ���� ��ġ(=ī�޶���ġ)
            //ray.direction : ������ �������
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //2d����͸� ���� 3d ������ ������Ʈ�� ���콺�� �����ϴ� ���
            //������ �ε����� ������Ʈ�� �����ؼ� hit�� ����
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //�ε��� ������Ʈ�� Transform ������ �Ű������� �̺�Ʈ ȣ��
                raycastEvent.Invoke(hit.transform);
            }
        }
    }
}
