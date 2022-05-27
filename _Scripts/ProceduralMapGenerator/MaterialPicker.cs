using System;
using System.Collections.Generic;
using UnityEngine;

public struct MaterialPicker
{
    public List<Material> materials;

    public Material GetRandomMaterial()
    {
        return materials[UnityEngine.Random.Range(0, materials.Count)];
    }
}

