using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutOutMaskImage : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material material = new(base.materialForRendering);
            material.SetInt("_StencilComp", (int)UnityEngine.Rendering.CompareFunction.NotEqual);
            return material;
        }
    }
}
