using RunTime.Datas.ValueObjects;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Datas.UnityObjects
{
    [CreateAssetMenu(fileName ="new Editor Textures", menuName ="Bus Jam/Create Editor Textures")]
    public class EditorTextures_SO :ScriptableObject
    {
        public List<EditorTexture> cellInfos;
    }
}
