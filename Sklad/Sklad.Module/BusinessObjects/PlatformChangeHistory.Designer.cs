using System;
using DevExpress.Xpo;
namespace Sklad.Module.BusinessObjects.TEST
{
    public partial class PlatformChangeHistory : XPLiteObject
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
        /// Название склада
        /// </summary>
        string fnameSklad;
        public string nameSklad
        {
            get { return fnameSklad; }
            set { SetPropertyValue<string>(nameof(nameSklad), ref fnameSklad, value); }
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
