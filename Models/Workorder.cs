using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace tx_barcode_sample.Models {
	public class Workorder {
		[Required]
		public string ContractorName { get; set; }
		[Required]
		public string HomeOwner { get; set; }
		[Required]
		public bool Approved { get; set; }
	}

	public class BarcodeData {
		public string Barcode { get; set; }
	}

	public class BarcodeView {
		public string Image { get; set; }
		public string Url { get; set; }
	}
}