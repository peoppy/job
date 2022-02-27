using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace Sklad.Module.BusinessObjects.TEST
{
    [DefaultClassOptions]
    public partial class Picket
    {
        public Picket(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
