using UnityEngine;

public class RadialMenuCommandTest : RadialMenuCommandBase
{
    public override void Move()
    {
        if (Input.GetKeyDown(KeyCode.O)) _image.color = Color.black;
        if (Input.GetKeyDown(KeyCode.P)) _image.color = Color.white;

        if (_image.color == Color.white) _image.color = Color.black;
        else if (_image.color == Color.black) _image.color = Color.white;

        Debug.Log("é¿çsÇ≥ÇÍÇ‹ÇµÇΩ");
    }
}
