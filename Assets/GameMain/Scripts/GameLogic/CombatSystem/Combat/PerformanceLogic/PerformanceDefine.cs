﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    //////////////////////////////////////////////////////////////////////////
    //Attribute in PerformanceLogic
    [AttributeUsage(AttributeTargets.Class)]
    public class PerformanceLogicDesAttribute : Attribute
    {
        protected string description;
        public PerformanceLogicDesAttribute(string _des)
        {
            description = _des;
        }
        public string Description
        {
            get
            {
                return description;
            }
        }
    }
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class PerformanceLogicItemDesAttribute : Attribute
    {
        protected string description;
        public PerformanceLogicItemDesAttribute(string _des)
        {
            description = _des;
        }
        public string Description
        {
            get
            {
                return description;
            }
        }
    }
}