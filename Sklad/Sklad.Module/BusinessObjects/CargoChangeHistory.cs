using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using System.ComponentModel;

namespace Sklad.Module.BusinessObjects.TEST
{
    [DefaultClassOptions]
    public partial class CargoChangeHistory
    {
        public CargoChangeHistory(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
