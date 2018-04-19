using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Test.BusinessLogic;
using Test.Common;

namespace Web.Controllers
{
	public class HomeController : Controller
	{
		private TestUserBusinessLogic testUserBusinessLogic = new TestUserBusinessLogic();

		public ActionResult Index()
		{
			this.ViewBag.Title = "Test Home";
			var testUsers = this.testUserBusinessLogic.GetTestUsers();
			return this.View(testUsers);
		}

		[HttpGet]
		[Route("/TestUser/{id}")]
		public ActionResult TestUser(int id)
		{
			var testUser = this.testUserBusinessLogic.GetTestUser(id);
			return this.View("TestUser", testUser);
		}

		[HttpPost]
		public ActionResult TestUser(TestUser testUser)
		{
			this.testUserBusinessLogic.AddUpdateTestUser(testUser);
			return this.RedirectToAction("Index");
		}

		public ActionResult DeleteTestUser(int id)
		{
			this.testUserBusinessLogic.DeleteTestUser(id);
			return this.RedirectToAction("Index");
		}

		public ActionResult ExportTestUsers()
		{
			this.Response.ContentType = "application/vnd.ms-excel";
			this.Response.AddHeader("Content-Disposition", "attachment; filename = TestUsersExport.xls");
			this.Response.ContentEncoding = Encoding.UTF8;

			var gridView = new GridView
			{
				DataSource = this.testUserBusinessLogic.GetTestUsers()
			};

			gridView.DataBind();
			var stringWriter = new StringWriter();
			var htmlTextWriter = new HtmlTextWriter(stringWriter);
			gridView.RenderControl(htmlTextWriter);
			this.Response.Write(stringWriter.ToString());
			this.Response.End();
			return this.File(this.Response.OutputStream, this.Response.ContentType);

		}
	}
}