using DevExpress.Xpo;
using DevExpress.Persistent.Base;
namespace Sklad.Module.BusinessObjects.TEST
{
    [DefaultClassOptions]
    public partial class Sklad
    {
        public Sklad(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
