using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace Sklad.Module.BusinessObjects
{
    /// <summary>
    /// Склад
    /// </summary>
    [DefaultClassOptions]
    public class Warehouse : BaseObject
    {
        public Warehouse(Session session) : base(session) { }

        string fName;
        /// <summary>  Название </summary>
        [RuleRequiredField(DefaultContexts.Save)]
        [Size(50)]
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>(nameof(Name), ref(fName), value); }
        }

        /// <summary> Коллекция площадок </summary>
        [Association(@"PlatformReferencesWarehouse")]
        public XPCollection<Platform> Platforms { get { return GetCollection<Platform>(nameof(Platforms)); } }
    }


    /// <summary> Площадка </summary>
    [DefaultClassOptions]
    public class Platform : BaseObject
    {
        public Platform(Session session) : base(session) { }

        string fName;
        /// <summary> Название </summary>
        [Size(50)]
        [RuleRequiredField(DefaultContexts.Save)]
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>(nameof(Name), ref(fName), value); }
        }

        float fCargoWeight;
        /// <summary> Вес груза </summary>
        [DefaultValue(0)]        
        public float CargoWeight
        {
            get { return fCargoWeight; }
            set { SetPropertyValue<float>(nameof(CargoWeight), ref(fCargoWeight), value); }
        }

        Cargo fIDCargo;
        /// <summary> Груз </summary>
        [Association(@"PlatformReferencesCargo")]
        [RuleRequiredField(DefaultContexts.Save)]
        public Cargo IDCargo
        {
            get { return fIDCargo; }
            set { SetPropertyValue<Cargo>(nameof(IDCargo), ref(fIDCargo), value); }
        }

        Warehouse fIDWarehouse;
        /// <summary> Склад </summary>
        [Association(@"PlatformReferencesWarehouse")]
        [RuleRequiredField(DefaultContexts.Save)]
        public Warehouse IDWarehouse
        {
            get { return fIDWarehouse; }
            set { SetPropertyValue<Warehouse>(nameof(IDWarehouse), ref(fIDWarehouse), value); }
        }

        bool fIsFormed;
        /// <summary> Флаг формирования </summary>
        /// <remarks> true - площадка сформирована (используется); false - площадка расформирована </remarks>
        public bool IsFormed
        {
            get { return fIsFormed; }
            set { SetPropertyValue<bool>(nameof(IsFormed), ref(fIsFormed), value); }
        }

        /// <summary> Коллекция пикетов </summary>
        [Association(@"PicketReferencesPlatform")]
        public XPCollection<Picket> Pickets { get { return GetCollection<Picket>(nameof(Pickets)); } }
    }


    /// <summary> Пикет </summary>
    [DefaultClassOptions]
    public class Picket : BaseObject
    {
        public Picket(Session session) : base(session) { }

        uint fNumber;
        /// <summary> Номер </summary>
        [RuleUniqueValue(DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public uint Number
        {
            get { return fNumber; }
            
            set { SetPropertyValue<uint>(nameof(Number), ref (fNumber), value); }            
        }

        Platform fIDPlatform;
        /// <summary> Площадка </summary>
        [Association(@"PicketReferencesPlatform")]
        public Platform IDplatform
        {
            get { return fIDPlatform; }
            
            set { SetPropertyValue<Platform>(nameof(IDplatform), ref(fIDPlatform), value); }
        }
    }

    /// <summary> Груз </summary>
    [DefaultClassOptions]
    public class Cargo : BaseObject
    {
        public Cargo(Session session) : base(session) { }

        string fName;
        /// <summary> Тип груза </summary>
        [RuleRequiredField(DefaultContexts.Save)]
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>(nameof(Name), ref(fName), value); }
        }

        /// <summary> Коллекция площадок </summary>
        [Association(@"PlatformReferencesCargo")]
        public XPCollection<Platform> Platforms { get { return GetCollection<Platform>(nameof(Platforms)); } }
    }
}