using UnityEngine;
using UnityEngine.UI;

public abstract class RadialMenuCommandBase : MonoBehaviour
{
    [SerializeField] public Image _image;

    // �q�N���X���Ƃɓ�����ς���������
    public abstract void Move();
}
