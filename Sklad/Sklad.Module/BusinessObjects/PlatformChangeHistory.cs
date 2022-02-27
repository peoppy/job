using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using System.ComponentModel;

namespace Sklad.Module.BusinessObjects.TEST
{
    [DefaultClassOptions]
    public partial class PlatformChangeHistory
    {
        public PlatformChangeHistory(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
