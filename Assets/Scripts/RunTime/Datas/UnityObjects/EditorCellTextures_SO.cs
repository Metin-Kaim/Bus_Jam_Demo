using RunTime.Datas.ValueObjects;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Datas.UnityObjects
{
    [CreateAssetMenu(fileName ="new Editor Cell Textures",menuName ="Bus Jam/Create Editor Cell Textures")]
    public class EditorCellTextures_SO : ScriptableObject
    {
        public List<EditorCellTexture> editorCellTextures;
    }
}
