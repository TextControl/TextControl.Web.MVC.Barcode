using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using tx_barcode_sample.Models;
using TXTextControl.DocumentServer;

namespace tx_barcode_sample.Controllers {
	public class HomeController : Controller {
		public ActionResult Index() {

			Workorder workorder = new Workorder() {
				Approved = false,
				ContractorName = "Peter Petersen"
			};

			return View(workorder);
		}

		public ActionResult ViewDocument(string document) {

			DocumentView view = new DocumentView() {
				DocumentPath = Server.MapPath("~/App_Data/" + document + ".tx"),
				Id = document
			};

			return View(view);
		}

		[HttpPost]
		public ActionResult Register(Workorder workorder) {
			if (ModelState.IsValid) {

				string sFileName = Helpers.MergeWorkorder(workorder);

				return View(Helpers.CreateBarcode(sFileName));
			}
			else {

				return View(new BarcodeView());
			}
		}
	}
}