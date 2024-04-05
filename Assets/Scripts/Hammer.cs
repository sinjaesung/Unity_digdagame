using System.Collections;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField]
    private float maxY; //망치의 최대 y 위치
    [SerializeField]
    private float minY; //망치의 최소 y 위치
    [SerializeField]
    private GameObject moleHitEffectPrefab; //두더지 타격 효과 프리팹
    [SerializeField]
    private MoleHitTextViewer[] moleHitTextViewer; //타격한 두더지 위치에 타격 정보 텍스트 출력
    [SerializeField]
    private GameController gameController; //점수 증가를 위한 GameController
    [SerializeField]
    private ObjectDetector objectDetector; //마우스 클릭으로 오브젝트 선택을 위한 ObjectDetector
    private Movement3D movement3D; //망치 오브젝트 이동을 위한 Movement

    private void Awake()
    {
        movement3D = GetComponent<Movement3D>();

        //OnHit 메소드를 ObjectDetector Class의 raycastEvent에 이벤트로 등록
        //ObjectDetector의 raycastEvent.Invoke(hit.transform); 메소드가 호출될때마다 OnHit(Transform target) 메소드가 호출된다
        objectDetector.raycastEvent.AddListener(OnHit);
    }

    private void OnHit(Transform target)
    {
        if (target.CompareTag("Mole"))
        {
            MoleFSM mole = target.GetComponent<MoleFSM>();

            //두더지가 홀 안에 있을때는 공격 불가
            if (mole.molestate == MoleState.UnderGround) return;

            //망치의 위치 설정
            transform.position = new Vector3(target.position.x, minY, target.position.z);

            //망치에 맞았기 때문에 두더지 상태를 바로 "UnderGround"로 설정
            mole.ChangeState(MoleState.UnderGround);

            //카메라 흔들기
            ShakeCamera.Instance.OnShakeCamera(0.1f, 0.1f);

            //두더지 타격 효과 생성 (Particle의 색상을 두더지 색상과 동일하게 설정)
            GameObject clone = Instantiate(moleHitEffectPrefab, transform.position, Quaternion.identity);
            ParticleSystem.MainModule main = clone.GetComponent<ParticleSystem>().main;
            main.startColor = mole.GetComponent<MeshRenderer>().material.color;

            //점수 증가 (+50)
            //gameController.Score += 50;
            //두더지 색상에 따라 처리(점수,시간)
            MoleHitProcess(mole);

            //망치를 위로 이동시키는 코루틴 재생
            StartCoroutine("MoveUp");
        }
    }

    private IEnumerator MoveUp()
    {
        //이동방향 (0,1,0) [위]
        movement3D.MoveTo(Vector3.up);

        while (true)
        {
            if(transform.position.y >= maxY)
            {
                movement3D.MoveTo(Vector3.zero);

                break;
            }

            yield return null;
        }
    }

    private void MoleHitProcess(MoleFSM mole)
    {
        if(mole.MoleType == MoleType.Normal)
        {
            gameController.NormalMoleHitCount++;  //기본 두더지 타격 횟수를 1증가
            gameController.Combo++;
            //gameController.Score += 50;
            //기본 x1에 10콤보당 0.5씩 더한다
            float scoreMultiple = 1 + gameController.Combo / 10 * 0.5f;
            int getScore = (int)(scoreMultiple * 50);
            //계산된 점수 getScore를 Score에 더해준다
            gameController.Score += getScore;

            //MoleIndex로 순번을 설정해 두었기 때문에 같은 자리에 있는 TextGetScore 텍스트 출력
            //하얀색 텍스트로 점수 증가 표현
            //moleHitTextViewer[mole.MoleIndex].OnHit("Score +50", Color.white);
            moleHitTextViewer[mole.MoleIndex].OnHit("Score +" + getScore, Color.white);
        }
        else if(mole.MoleType == MoleType.Red)
        {
            gameController.RedMoleHitCount++;  //빨간색 두더지 타격 횟수를 1증가
            gameController.Combo = 0;
            gameController.Score -= 300;
            //빨간색 텍서트로 점수 감소 표현
            moleHitTextViewer[mole.MoleIndex].OnHit("Score -300", Color.red);
        }
        else if(mole.MoleType == MoleType.Blue)
        {
            gameController.BlueMoleHitCount++;  //파란색 두더지 타격 횟수를 1증가
            gameController.Combo++;
            gameController.CurrentTime += 3;
            //파란색 텍스트로 시간 증가 표현
            moleHitTextViewer[mole.MoleIndex].OnHit("Time +3", Color.blue);
        }
    }
}
