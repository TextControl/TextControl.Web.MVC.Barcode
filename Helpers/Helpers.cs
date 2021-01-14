using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using tx_barcode_sample.Models;
using TXTextControl.DocumentServer;

namespace tx_barcode_sample {
	public class Helpers {
		public static string GenerateUID(int length) {

			Random rnd = new Random();

			string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
			StringBuilder result = new StringBuilder(length);
			for (int i = 0; i < length; i++) {
				result.Append(characters[rnd.Next(characters.Length)]);
			}
			return result.ToString();
		}

		public static string MergeWorkorder(Workorder workorder) {

			using (TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl()) {

				tx.Create();

				// load the work order template
				tx.Load(HttpContext.Current.Server.MapPath(
					"~/App_Data/workorder.tx"),
					TXTextControl.StreamType.InternalUnicodeFormat);

				MailMerge mm = new MailMerge() {
					TextComponent = tx
				};
				
				// merge the data into the template
				mm.MergeObject(workorder);

				// generate a unique filename and save the work order
				var sFileName = Helpers.GenerateUID(5);
				tx.Save(HttpContext.Current.Server.MapPath("~/App_Data/" + sFileName + ".tx"), TXTextControl.StreamType.InternalUnicodeFormat);

				return sFileName;
			}
		}

		public static BarcodeView CreateBarcode(string Filename) {

			using (TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl()) {

				tx.Create();

				// assemble the barcode URL 
				BarcodeData data = new BarcodeData() {
					Barcode = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
					"/Home/ViewDocument?document=" +
					Filename
				};

				// load the barcode template
				tx.Load(HttpContext.Current.Server.MapPath("~/App_Data/barcode.tx"),
					TXTextControl.StreamType.InternalUnicodeFormat);

				MailMerge mm = new MailMerge() {
					TextComponent = tx
				};

				// merge the barcode
				mm.MergeObject(data);

				BarcodeView view = new BarcodeView() {
					Url = data.Barcode
				};

				// create an return the barcode image
				foreach (TXTextControl.DataVisualization.BarcodeFrame barcode in tx.Barcodes) {

					byte[] imageArray;

					MemoryStream ms = new MemoryStream();

					((TXTextControl.Barcode.TXBarcodeControl)barcode.Barcode).SaveImage(
						ms, System.Drawing.Imaging.ImageFormat.Png);

					imageArray = new byte[ms.Length];
					ms.Seek(0, System.IO.SeekOrigin.Begin);
					ms.Read(imageArray, 0, (int)ms.Length);

					view.Image = "data:image/png;base64," + Convert.ToBase64String(imageArray);

					return view;
				}

				return null;
			}
		}
	}
}