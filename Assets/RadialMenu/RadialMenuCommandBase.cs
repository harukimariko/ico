using UnityEngine;
using UnityEngine.UI;

public abstract class RadialMenuCommandBase : MonoBehaviour
{
    [SerializeField] public Image _image;

    // ŽqƒNƒ‰ƒX‚²‚Æ‚É“®‚«‚ð•Ï‚¦‚½‚¢•”•ª
    public abstract void Move();
}
