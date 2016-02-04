using System;
using System.Threading.Tasks;
using System.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TapTapApplication
{
	public class JsonClass
	{
		public JsonClass ()
		{
		}


		public static string JsonCall(string url) {
			string responseText;

			try {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
				request.Method = "GET/POST";
				request.ContentType = "application/json";

				HttpWebResponse myResp = (HttpWebResponse)request.GetResponse ();

				using (var response = request.GetResponse ()) {
					using (var reader = new StreamReader (response.GetResponseStream ())) {
						responseText = reader.ReadToEnd ();
					}
				}
			} catch (WebException e) {
				using (var reader = new StreamReader(e.Response.GetResponseStream())) {
					responseText = reader.ReadToEnd ();
					Console.WriteLine (responseText);
				}
			}

			return responseText;
		}
	}
}

