using System.Linq;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Raven.Client.Linq;

namespace Crowbar.Web.Core
{
    public class DocumentStoreTestController : CrowbarControllerBase
    {
        [GET(CrowbarRoute.DocumentStore)]
        public ActionResult Value(string text)
        {
            using (var session = Store.OpenSession())
            {
                int count = session.Query<Model>().Where(x => x.Text == text).Count();
                return Assert(() => count == 1);
            }
        }
    }
}