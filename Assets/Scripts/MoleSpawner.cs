using System.Collections;
using UnityEngine;

public class MoleSpawner : MonoBehaviour
{
    [SerializeField]
    private MoleFSM[] moles; //�ʿ� �����ϴ� �δ�����
    [SerializeField]
    private float spawnTime; //�δ��� ���� �ֱ�

    //�δ��� ���� Ȯ�� (Normal : 85%, Red: 10%, Blue: 5%)
    private int[] spawnPercents = new int[3] { 85, 10, 5 };
    //�ѹ��� �����ϴ� �ִ� �δ��� ��
    public int MaxSpawnMole { set; get; } = 1;

    public void Setup()
    {
        StartCoroutine("SpawnMole");
    }

    private IEnumerator SpawnMole()
    {
        while (true)
        {
            //���ù޾��� moveUp��Ҵ� moveUp->onGround�������->moveDown->underGround���� ���� spawnole�� ���ؼ� ȣ��Ǳ�������(moveup)���
            //0~moles.length-1 �� ���� ���� ����
            //int index = Random.Range(0, moles.Length);

            //���õ� �δ����� �Ӽ� ����
            //moles[index].MoleType = SpawnMoleType();

            //index��° �δ��� ���¸� "MoveUp"���� ����
            //moles[index].ChangeState(MoleState.MoveUp);

            //MaxSpawnMole ���ڸ�ŭ �δ��� ����
            StartCoroutine("SpawnMultiMoles");

            //spawnTime �ð����� ���
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private MoleType SpawnMoleType()
    {
        int percent = Random.Range(0, 100);
        float cumulative = 0;

        for(int i=0; i<spawnPercents.Length; ++i)
        {
            cumulative += spawnPercents[i];

            if(percent < cumulative)
            {
                return (MoleType)i;
            }
        }

        return MoleType.Normal;
    }

    private IEnumerator SpawnMultiMoles()
    {
        //0~moles.length-1 ������ ��ġ�� �ʴ� ������ ��� ����
        int[] indexs = RandomNumberics(moles.Length, moles.Length);
        int currentSpawnMole = 0; //���� ������ �δ��� ����
        int currentIndex = 0; //indexs �迭 �ε���

        //���� �����ؾ��� �δ��� ���ڸ�ŭ �δ��� ����
        while(currentIndex < indexs.Length)
        {
            //�δ����� �ٴڿ� ���� ���� ���� ���� (���� ������ �δ����� ������� �ʵ���)
            if (moles[indexs[currentIndex]].molestate == MoleState.UnderGround)
            {
                //���õ� �δ��� �Ӽ� ����
                moles[indexs[currentIndex]].MoleType = SpawnMoleType();
                //���õ� �δ��� ���¸� "MoveUp"���� ����
                moles[indexs[currentIndex]].ChangeState(MoleState.MoveUp);
                //������ �δ��� ���� 1 ����
                currentSpawnMole++;

                yield return new WaitForSeconds(0.1f);
            }

            //�ִ� ���� ���ڸ�ŭ ���������� SpawnMultiMoles() �ڷ�ƾ �Լ� ����
            if(currentSpawnMole == MaxSpawnMole)
            {
                break;
            }

            currentIndex++;

            yield return null;
        }
    }

    private int[] RandomNumberics(int maxCount, int n)
    {
        //0~maxCount ������ ���� �� ��ġ�� �ʴ� n�� ������ �ʿ��� �� ���
        int[] defaults = new int[maxCount]; //0~maxCount���� ������� �����ϴ� �迭
        int[] results = new int[n]; //��� ������ �����ϴ� �迭

        //�迭 ��ü�� 0~maxCount�� ���� ������� ����
        for(int i=0; i<maxCount; ++i)
        {
            defaults[i] = i;
        }

        for(int i=0; i<n; ++i)
        {
            int index = Random.Range(0, maxCount);

            results[i] = defaults[index];
            defaults[index] = defaults[maxCount - 1];

            maxCount--;
        }

        return results;
    }
}
