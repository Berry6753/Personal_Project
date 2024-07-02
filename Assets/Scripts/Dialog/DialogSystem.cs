using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{

}
[System.Serializable]
public struct Speaker
{ 
    public SpriteRenderer spriteRenderer;       // 캐릭터 이미지
    public Image imageDialog;                   // 대화창 Image UI
    public TMP_Text textName;                   // 현재 대사중인 캐릭터 이름
    public TMP_Text textDialogue;               // 현재 대사 출력
    public GameObject objectArrow;              // 대사가 완료될 시 출력되는 오브젝트
}