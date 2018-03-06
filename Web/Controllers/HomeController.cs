using System.Web.Mvc;
using Test.BusinessLogic;
using Test.Common;

namespace Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Title = "Test Home";

			var testUsers = TestUserBusinessLogic.GetTestUsers();

			return View(testUsers);
		}

		[HttpGet]
		[Route("/TestUser/{id}")]
		public ActionResult TestUser(int id)
		{
			var testUser = TestUserBusinessLogic.GetTestUser(id);

			return View("TestUser", testUser);
		}

		[HttpPost]
		public ActionResult TestUser(TestUser testUser)
		{
			testUser = TestUserBusinessLogic.AddUpdateTestUser(testUser);
			return View("TestUser", testUser);
		}
	}
}