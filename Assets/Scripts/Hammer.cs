using System.Collections;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField]
    private float maxY; //��ġ�� �ִ� y ��ġ
    [SerializeField]
    private float minY; //��ġ�� �ּ� y ��ġ
    [SerializeField]
    private GameObject moleHitEffectPrefab; //�δ��� Ÿ�� ȿ�� ������
    [SerializeField]
    private MoleHitTextViewer[] moleHitTextViewer; //Ÿ���� �δ��� ��ġ�� Ÿ�� ���� �ؽ�Ʈ ���
    [SerializeField]
    private GameController gameController; //���� ������ ���� GameController
    [SerializeField]
    private ObjectDetector objectDetector; //���콺 Ŭ������ ������Ʈ ������ ���� ObjectDetector
    private Movement3D movement3D; //��ġ ������Ʈ �̵��� ���� Movement

    private void Awake()
    {
        movement3D = GetComponent<Movement3D>();

        //OnHit �޼ҵ带 ObjectDetector Class�� raycastEvent�� �̺�Ʈ�� ���
        //ObjectDetector�� raycastEvent.Invoke(hit.transform); �޼ҵ尡 ȣ��ɶ����� OnHit(Transform target) �޼ҵ尡 ȣ��ȴ�
        objectDetector.raycastEvent.AddListener(OnHit);
    }

    private void OnHit(Transform target)
    {
        if (target.CompareTag("Mole"))
        {
            MoleFSM mole = target.GetComponent<MoleFSM>();

            //�δ����� Ȧ �ȿ� �������� ���� �Ұ�
            if (mole.molestate == MoleState.UnderGround) return;

            //��ġ�� ��ġ ����
            transform.position = new Vector3(target.position.x, minY, target.position.z);

            //��ġ�� �¾ұ� ������ �δ��� ���¸� �ٷ� "UnderGround"�� ����
            mole.ChangeState(MoleState.UnderGround);

            //ī�޶� ����
            ShakeCamera.Instance.OnShakeCamera(0.1f, 0.1f);

            //�δ��� Ÿ�� ȿ�� ���� (Particle�� ������ �δ��� ����� �����ϰ� ����)
            GameObject clone = Instantiate(moleHitEffectPrefab, transform.position, Quaternion.identity);
            ParticleSystem.MainModule main = clone.GetComponent<ParticleSystem>().main;
            main.startColor = mole.GetComponent<MeshRenderer>().material.color;

            //���� ���� (+50)
            //gameController.Score += 50;
            //�δ��� ���� ���� ó��(����,�ð�)
            MoleHitProcess(mole);

            //��ġ�� ���� �̵���Ű�� �ڷ�ƾ ���
            StartCoroutine("MoveUp");
        }
    }

    private IEnumerator MoveUp()
    {
        //�̵����� (0,1,0) [��]
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
            gameController.NormalMoleHitCount++;  //�⺻ �δ��� Ÿ�� Ƚ���� 1����
            gameController.Combo++;
            //gameController.Score += 50;
            //�⺻ x1�� 10�޺��� 0.5�� ���Ѵ�
            float scoreMultiple = 1 + gameController.Combo / 10 * 0.5f;
            int getScore = (int)(scoreMultiple * 50);
            //���� ���� getScore�� Score�� �����ش�
            gameController.Score += getScore;

            //MoleIndex�� ������ ������ �ξ��� ������ ���� �ڸ��� �ִ� TextGetScore �ؽ�Ʈ ���
            //�Ͼ�� �ؽ�Ʈ�� ���� ���� ǥ��
            //moleHitTextViewer[mole.MoleIndex].OnHit("Score +50", Color.white);
            moleHitTextViewer[mole.MoleIndex].OnHit("Score +" + getScore, Color.white);
        }
        else if(mole.MoleType == MoleType.Red)
        {
            gameController.RedMoleHitCount++;  //������ �δ��� Ÿ�� Ƚ���� 1����
            gameController.Combo = 0;
            gameController.Score -= 300;
            //������ �ؼ�Ʈ�� ���� ���� ǥ��
            moleHitTextViewer[mole.MoleIndex].OnHit("Score -300", Color.red);
        }
        else if(mole.MoleType == MoleType.Blue)
        {
            gameController.BlueMoleHitCount++;  //�Ķ��� �δ��� Ÿ�� Ƚ���� 1����
            gameController.Combo++;
            gameController.CurrentTime += 3;
            //�Ķ��� �ؽ�Ʈ�� �ð� ���� ǥ��
            moleHitTextViewer[mole.MoleIndex].OnHit("Time +3", Color.blue);
        }
    }
}
