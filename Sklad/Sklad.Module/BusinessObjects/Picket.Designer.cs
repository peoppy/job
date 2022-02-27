﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using DevExpress.Persistent.Validation;

namespace Sklad.Module.BusinessObjects.TEST
{
    /// <summary>
    /// Пикет
    /// </summary>
    public partial class Picket : XPLiteObject
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        int fID;
        [Key(true)]
        public int ID
        {
            get { return fID; }
            set { SetPropertyValue<int>(nameof(ID), ref fID, value); }
        }

        /// <summary>
        /// Номер
        /// </summary>    
        int fNumber;
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public int Number
        {
            get { return fNumber; }
            set { SetPropertyValue<int>(nameof(Number), ref fNumber, value); }
        }

        /// <summary>
        /// Площадка
        /// </summary>
        Platform fIDplatform;
        [Association(@"PicketReferencesPlatform")]             
        public Platform IDplatform
        {
            get { return fIDplatform; }
            set { SetPropertyValue<Platform>(nameof(IDplatform), ref fIDplatform, value); }
        }
    }
}