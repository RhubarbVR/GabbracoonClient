using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc;

namespace RequestModels
{
	public sealed class LocalText
	{
		public string Key { get; set; }
		public string[] Prams { get; set; }

		public string DataString => $"{Key};{string.Join(";", Prams)}";

		public LocalText(string key,params string[] prams) {
			Key = key;
			Prams = prams;
		}

		public LocalText() { }
	}
}
