using UnityEngine.UI;

public class UIRaycastReceiver : Graphic
{
    public override void SetMaterialDirty()
    {
        return;
    }

    public override void SetVerticesDirty()
    {
        return;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        return;
    }
}