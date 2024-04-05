using System.Collections;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    //싱글톤 처리를 위한 instance 변수 선언
    private static ShakeCamera instance;
    //외부에서 Get 접근만 가능하도록 Instance 프로퍼티 선언
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
        //흔들리기 직전의 시작 위치 (흔들림 종료 후 돌아올 위치)
        Vector3 startPosition = transform.position;

        while(shakeTime > 0.0f)
        {
            //특정 축마 변경하길 원하면 아래 코드 이용(이동하지 않을 축은 0값 사용)
            //float x = Random.Range(-1f,1f); float y = Random.Range(-1f,1f); float z = Random.Range(-1f,1f);
            //transform.position = startPosition + new Vector3(x,y,z) * shakeIntensity;

            //초기 위치로부터 구 범위(Size 1) * shakeIntensity의 범위안에서 카메라 위치 변동
            transform.position = startPosition + Random.insideUnitSphere * shakeIntensity;

            //시간 감소
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = startPosition;
    }
}
