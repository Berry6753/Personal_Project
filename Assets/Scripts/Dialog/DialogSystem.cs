using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private List<Speaker> speakerList = new List<Speaker>();        // ��ȭ�� �����ϴ� ĳ���͵��� ����Ʈ
    [SerializeField] private List<DialogDate> dialogList = new List<DialogDate>();   // ���� �б��� ��� ��� ����Ʈ
    private bool isFirst;                                   // ���� 1ȸ�� ȣ���ϱ� ���� ����
    private bool isClickBtn;
    private int currentDialogIndex;                         // ���� ��� ����
    private int currentSpeakerIndex;                        // ���� ���� �ϴ� Speaker�� ����Ʈ ����

    [SerializeField] private Button agree;
    private NPCUI npc;
    private Coroutine co;

    private void Awake()
    {
        npc = GetComponentInParent<NPCUI>();
    }

    private void OnEnable()
    {      
        currentDialogIndex = -1;
        currentSpeakerIndex = 0;
        isFirst = true;
        isClickBtn = false;
        GameManager.Instance.PauseGame();
        SetUp();
        co = StartCoroutine(CoDialog());
    }

    private IEnumerator CoDialog()
    {
        yield return new WaitUntil(() => UpdateDialog());
        StopCoroutine(co);
        npc.OnExitInteraction();
    }
    private void SetUp()
    {
        // ��� ��ȭ ���� ������Ʈ ��Ȱ��ȭ
        for (int i = 0; i < speakerList.Count; i++)
        {
            SetActiveObjects(speakerList[i], false);

            // ĳ���� �̹����� ���̵��� ����
            speakerList[i].spriteRenderer.gameObject.SetActive(true);
        }
    }

    public bool UpdateDialog()
    {
        // ��� �бⰡ ���۵� �� 1ȸ�� ȣ��
        if (isFirst)
        {
            // �ʱ�ȭ. ĳ���� �̹����� Ȱ��ȭ�ϰ�, ��� ���� UI�� ��� ��Ȱ��ȭ
            SetUp();

            isFirst = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return true;
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            // ��簡 �������� ��� ���� ��� ����
            if (dialogList.Count > currentDialogIndex + 1)
            {
                SetNextDialog();
            }
            // ��簡 �� �̻� ���� ��� ��� ������Ʈ�� ��Ȱ��ȭ�ϰ� true ��ȯ
            else
            {
                //// ���� ��ȭ�� �����ߴ� ��� ĳ����, ��ȭ ���� UI�� ������ �ʰ� ��Ȱ��ȭ
                //for (int i = 0; i < speakerList.Count; i++)
                //{
                //    SetActiveObjects(speakerList[i], false);

                //    // SetActiveObjects()�� ĳ���� �̹����� ������ �ʰ� �ϴ� �κ��� ���� ������ ������ ȣ��
                //    speakerList[i].spriteRenderer.gameObject.SetActive(false);
                //}
                agree.gameObject.SetActive(true);

                if (isClickBtn)
                    return true;
            }
        }

        return false;
    }

    private void SetNextDialog()
    {
        // ���� ȭ���� ��ȭ ���� ������Ʈ ��Ȱ��ȭ
        SetActiveObjects(speakerList[currentSpeakerIndex], false);

        // ���� ��縦 �����ϵ���
        currentDialogIndex++;

        // ���� ȭ�� ���� ����
        currentSpeakerIndex = dialogList[currentDialogIndex].speakerIndex;

        // ���� ȭ���� ��ȭ ���� ������Ʈ Ȱ��ȭ
        SetActiveObjects(speakerList[currentSpeakerIndex], true);

        // ���� ȭ�� �̸� �ؽ�Ʈ ����
        speakerList[currentSpeakerIndex].textName.text = dialogList[currentDialogIndex].name;

        // ���� ȭ���� ��� �ؽ�Ʈ ����
        //speakerList[currentSpeakerIndex].textDialogue.text = dialogList[currentDialogIndex].dialogue;
        StartCoroutine(CoTyping());
    }

    private IEnumerator CoTyping()
    {
        for (int i = 0; i < dialogList[currentDialogIndex].dialogue.Length; i++)
        {
            speakerList[currentSpeakerIndex].textDialogue.text = dialogList[currentDialogIndex].dialogue.Substring(0, i);

            yield return new WaitForSecondsRealtime(0.05f);
        }
    }


    private void SetActiveObjects(Speaker speaker, bool isActive)
    { 
        speaker.imageDialog.gameObject.SetActive(isActive);
        speaker.textName.gameObject.SetActive(isActive);
        speaker.textDialogue.gameObject.SetActive(isActive);

        // ��ȭ ���� �� ������ UI�� ����ÿ��� ������ ������ �׻� false
        speaker.objectArrow.SetActive(false );

        // ĳ���� ���� �� ����
        Color color = speaker.spriteRenderer.color;
        color.a = isActive ? 1 : 0.2f;
        speaker.spriteRenderer.color = color;   
    }

    public void OnClick_AgreeBtn()
    {
        isClickBtn = true;
    }
}

[System.Serializable]
public struct Speaker
{ 
    public Image spriteRenderer;                // ĳ���� �̹���
    public Image imageDialog;                   // ��ȭâ Image UI
    public TMP_Text textName;                   // ���� ������� ĳ���� �̸�
    public TMP_Text textDialogue;               // ���� ��� ���
    public GameObject objectArrow;              // ��簡 �Ϸ�� �� ��µǴ� ������Ʈ
}

[System.Serializable]
public struct DialogDate
{
    public int speakerIndex;                    // �̸��� ��縦 ����� Speaker�� �迭 Index ��
    public string name;                         // ĳ���� �̸�
    [TextArea(3, 7)]
    public string dialogue;                     // ��系��
}