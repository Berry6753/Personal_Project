using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{

}
[System.Serializable]
public struct Speaker
{ 
    public SpriteRenderer spriteRenderer;       // ĳ���� �̹���
    public Image imageDialog;                   // ��ȭâ Image UI
    public TMP_Text textName;                   // ���� ������� ĳ���� �̸�
    public TMP_Text textDialogue;               // ���� ��� ���
    public GameObject objectArrow;              // ��簡 �Ϸ�� �� ��µǴ� ������Ʈ
}