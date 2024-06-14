using RunTime.Enums;
using RunTime.Signals;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Others
{
    [System.Serializable]
    public class ColorPatternInfos
    {
        public EntityTypes entityType;
        public Color color;
    }

    public class ColorPattern : MonoBehaviour
    {
        [SerializeField] List<ColorPatternInfos> colorPatternInfos;

        Dictionary<EntityTypes, Color> colorPatternDictionary = new();

        private void Awake()
        {
            for (int i = 0; i < colorPatternInfos.Count; i++)
            {
                colorPatternDictionary.Add(colorPatternInfos[i].entityType, colorPatternInfos[i].color);
            }
        }

        private void OnEnable()
        {
            ColorSignals.Instance.onGetColor += OnGetColor;
        }

        private Color OnGetColor(EntityTypes entityTypes)
        {
            return colorPatternDictionary[entityTypes];
        }

        private void OnDisable()
        {
            ColorSignals.Instance.onGetColor -= OnGetColor;
        }
    }
}