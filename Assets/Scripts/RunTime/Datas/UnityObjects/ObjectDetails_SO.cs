﻿using RunTime.Datas.ValueObjects;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Datas.UnityObjects
{
    [CreateAssetMenu(fileName = "new Object Details", menuName = "Bus Jam/Create Object Details")]
    public class ObjectDetails_SO : ScriptableObject
    {
        public List<ObjectDetail> objectDetails;
        public List<ObstacleDetail> obstacleDetails;
    }
}
