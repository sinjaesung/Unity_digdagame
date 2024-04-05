using System.Collections;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    //�̱��� ó���� ���� instance ���� ����
    private static ShakeCamera instance;
    //�ܺο��� Get ���ٸ� �����ϵ��� Instance ������Ƽ ����
    public static ShakeCamera Instance => instance;

    private float shakeTime;
    private float shakeIntensity;

    public ShakeCamera()
    {
        instance = this;
    }

    public void OnShakeCamera(float shakeTime=1.0f, float shakeIntensity = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine("ShakeByPosition");
        StartCoroutine("ShakeByPosition");
    }

    private IEnumerator ShakeByPosition()
    {
        //��鸮�� ������ ���� ��ġ (��鸲 ���� �� ���ƿ� ��ġ)
        Vector3 startPosition = transform.position;

        while(shakeTime > 0.0f)
        {
            //Ư�� �ึ �����ϱ� ���ϸ� �Ʒ� �ڵ� �̿�(�̵����� ���� ���� 0�� ���)
            //float x = Random.Range(-1f,1f); float y = Random.Range(-1f,1f); float z = Random.Range(-1f,1f);
            //transform.position = startPosition + new Vector3(x,y,z) * shakeIntensity;

            //�ʱ� ��ġ�κ��� �� ����(Size 1) * shakeIntensity�� �����ȿ��� ī�޶� ��ġ ����
            transform.position = startPosition + Random.insideUnitSphere * shakeIntensity;

            //�ð� ����
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = startPosition;
    }
}
