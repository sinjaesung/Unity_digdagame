using System.Collections;
using UnityEngine;

//지하에 대기, 지상에 대기, 지하->지상 이동, 지상->지하 이동
public enum MoleState {  UnderGround = 0, OnGround, MoveUp, MoveDown }
//두더지 종류 (기본,점수 - , 시간+)
public enum MoleType { Normal = 0, Red, Blue }

public class MoleFSM : MonoBehaviour
{
    [SerializeField]
    private GameController gameController; //콤보 초기화를 위한 GameController
    [SerializeField]
    private float waitTimeOnGround; //지면에 올라와서 내려가기까지 기다리는 시간
    [SerializeField]
    private float limitMinY; //내려갈 수 있는 최소 y위치
    [SerializeField]
    private float limitMaxY; //올라올 수 있는 최대 y위치

    private Movement3D movement3D; //위,아래 이동을 위한 Movement3D
    private MeshRenderer meshRenderer; //두더지 색상 설정을 위한 MeshRenderer

    private MoleType moletype; //두더지 종류
    private Color defaultColor; //기본 두더지 색상(173,135,24)

    //두더지의 현재 상태 (set은 MoleFSM클래스 내부에서만)
    public MoleState molestate { private set; get; }
    //두더지의 종류 (MoleType에 따라 두더지 색상 변경)
    public MoleType MoleType
    {
        set
        {
            moletype = value;

            switch (moletype)
            {
                case MoleType.Normal:
                    meshRenderer.material.color = defaultColor;
                    break;
                case MoleType.Red:
                    meshRenderer.material.color = Color.red;
                    break;
                case MoleType.Blue:
                    meshRenderer.material.color = Color.blue;
                    break;
            }
        }
        get => moletype;
    }

    //두더지가 배치되어 있는 순번 (왼쪽 상단부터 0)
    [field: SerializeField]
    public int MoleIndex { private set; get; }

    private void Awake()
    {
        movement3D = GetComponent<Movement3D>();
        meshRenderer = GetComponent<MeshRenderer>();

        defaultColor = meshRenderer.material.color; //두더지의 최초 색상 저장

        ChangeState(MoleState.UnderGround);
    }

    public void ChangeState(MoleState newState)
    {
        //열거형 변수를 ToString() 메소드 이용해 문자열로 변환하면
        //UnderGround와 같이 열거형 요소 이름 반환

        //이전에 재생중이던 상태 종료
        StopCoroutine(molestate.ToString());
        //상태 변경
        molestate = newState;
        //새로운 상태 재생
        StartCoroutine(molestate.ToString());
    }

    private IEnumerator UnderGround()
    {
        movement3D.MoveTo(Vector3.zero);

        transform.position = new Vector3(transform.position.x, limitMinY, transform.position.z);

        yield return null;
    }

    private IEnumerator OnGround()
    {
        movement3D.MoveTo(Vector3.zero);

        transform.position = new Vector3(transform.position.x, limitMaxY, transform.position.z);

        yield return new WaitForSeconds(waitTimeOnGround);

        ChangeState(MoleState.MoveDown);
    }

    private IEnumerator MoveUp()
    {
        movement3D.MoveTo(Vector3.up);

        while (true)
        {
            //두더지 y위치가 limitMaxY에 도달하면 상태 변경
            if(transform.position.y >= limitMaxY)
            {
                //OnGround 상태로 변경
                ChangeState(MoleState.OnGround);
                break;
            }

            yield return null;
        }
    }

    private IEnumerator MoveDown()
    {
        movement3D.MoveTo(Vector3.down);

        while (true)
        {
            //두더지 y위치가 limitMinY에 도달하면 반복문 중지
            if(transform.position.y <= limitMinY)
            {
                //UnderGround 상태로 변경
               // ChangeState(MoleState.UnderGround);
                break;
            }

            yield return null;
        }

        //망치에 타격 당하지 않고 자연스레 구멍으로 들어갈 때 호출
        //MoveDown -> UnderGround

        //망치로 때리지 못하고 땅속으로 들어간 두더지의 속성이 Normal이면 콤보 초기화
        if (moletype == MoleType.Normal)
        {
            gameController.Combo = 0;
        }

        //UnderGround 상태로 변경
        ChangeState(MoleState.UnderGround);
    }
}
