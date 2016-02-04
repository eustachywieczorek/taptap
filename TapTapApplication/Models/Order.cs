using System;

namespace TapTapApplication
{
	public class Order
	{
		public Order() {

		}

		public string Coffee { get; set; }
		public string Size { get; set; }
		public string CafeId { get; set; }
		public string Cafe { get; set; }
		public DateTime OrderTime { get; set; }
		public string OrderStatus { get; set; }
	}
}

