using System.Web.Mvc;
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
	}
}