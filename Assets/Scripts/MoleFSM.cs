using System.Collections;
using UnityEngine;

//���Ͽ� ���, ���� ���, ����->���� �̵�, ����->���� �̵�
public enum MoleState {  UnderGround = 0, OnGround, MoveUp, MoveDown }
//�δ��� ���� (�⺻,���� - , �ð�+)
public enum MoleType { Normal = 0, Red, Blue }

public class MoleFSM : MonoBehaviour
{
    [SerializeField]
    private GameController gameController; //�޺� �ʱ�ȭ�� ���� GameController
    [SerializeField]
    private float waitTimeOnGround; //���鿡 �ö�ͼ� ����������� ��ٸ��� �ð�
    [SerializeField]
    private float limitMinY; //������ �� �ִ� �ּ� y��ġ
    [SerializeField]
    private float limitMaxY; //�ö�� �� �ִ� �ִ� y��ġ

    private Movement3D movement3D; //��,�Ʒ� �̵��� ���� Movement3D
    private MeshRenderer meshRenderer; //�δ��� ���� ������ ���� MeshRenderer

    private MoleType moletype; //�δ��� ����
    private Color defaultColor; //�⺻ �δ��� ����(173,135,24)

    //�δ����� ���� ���� (set�� MoleFSMŬ���� ���ο�����)
    public MoleState molestate { private set; get; }
    //�δ����� ���� (MoleType�� ���� �δ��� ���� ����)
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

    //�δ����� ��ġ�Ǿ� �ִ� ���� (���� ��ܺ��� 0)
    [field: SerializeField]
    public int MoleIndex { private set; get; }

    private void Awake()
    {
        movement3D = GetComponent<Movement3D>();
        meshRenderer = GetComponent<MeshRenderer>();

        defaultColor = meshRenderer.material.color; //�δ����� ���� ���� ����

        ChangeState(MoleState.UnderGround);
    }

    public void ChangeState(MoleState newState)
    {
        //������ ������ ToString() �޼ҵ� �̿��� ���ڿ��� ��ȯ�ϸ�
        //UnderGround�� ���� ������ ��� �̸� ��ȯ

        //������ ������̴� ���� ����
        StopCoroutine(molestate.ToString());
        //���� ����
        molestate = newState;
        //���ο� ���� ���
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
            //�δ��� y��ġ�� limitMaxY�� �����ϸ� ���� ����
            if(transform.position.y >= limitMaxY)
            {
                //OnGround ���·� ����
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
            //�δ��� y��ġ�� limitMinY�� �����ϸ� �ݺ��� ����
            if(transform.position.y <= limitMinY)
            {
                //UnderGround ���·� ����
               // ChangeState(MoleState.UnderGround);
                break;
            }

            yield return null;
        }

        //��ġ�� Ÿ�� ������ �ʰ� �ڿ����� �������� �� �� ȣ��
        //MoveDown -> UnderGround

        //��ġ�� ������ ���ϰ� �������� �� �δ����� �Ӽ��� Normal�̸� �޺� �ʱ�ȭ
        if (moletype == MoleType.Normal)
        {
            gameController.Combo = 0;
        }

        //UnderGround ���·� ����
        ChangeState(MoleState.UnderGround);
    }
}
