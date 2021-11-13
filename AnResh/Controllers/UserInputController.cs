using System.Web.Mvc;
using AnResh.CsharpScripts;
using AnResh.Models;

namespace AnResh.Controllers
{
    public class UserInputController : Controller
    {
        public ActionResult Index()
        {
            var userInput = new UserInput();
            return View(userInput);
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "UpdateFromFile")]
        public ActionResult UpdateFromFile(UserInput userInput)
        {
            userInput.UpdateFromFile();
            return View(userInput);
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "SaveToFile")]
        public ActionResult SaveToFile(UserInput userInput, string text)
        {
            userInput.SaveToFile(text);
            return View(userInput);
        }
    }
}