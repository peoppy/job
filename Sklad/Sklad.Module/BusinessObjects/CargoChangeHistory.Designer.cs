using System;
using DevExpress.Xpo;
namespace Sklad.Module.BusinessObjects.TEST
{
    public partial class CargoChangeHistory : XPLiteObject
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
        /// Название площадки
        /// </summary>
        string fnamePlatform;
        public string namePlatform
        {
            get { return fnamePlatform; }
            set { SetPropertyValue<string>(nameof(namePlatform), ref fnamePlatform, value); }
        }

        /// <summary>
        /// Вес груза
        /// </summary>
        float fCargo;
        public float Cargo
        {
            get { return fCargo; }
            set { SetPropertyValue<float>(nameof(Cargo), ref fCargo, value); }
        }
        
        /// <summary>
        /// Дата с
        /// </summary>
        DateTime fDateSince;
        public DateTime DateSince
        {
            get { return fDateSince; }
            set { SetPropertyValue<DateTime>(nameof(DateSince), ref fDateSince, value); }
        }

        /// <summary>
        /// Дата по
        /// </summary>
        DateTime fDateBy;
        public DateTime DateBy
        {
            get { return fDateBy; }
            set { SetPropertyValue<DateTime>(nameof(DateBy), ref fDateBy, value); }
        }
    }
}
