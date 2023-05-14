using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using static GabbracoonClient.HttpResponseMessageHelper;

namespace GabbracoonClient
{
	public static class HttpResponseMessageHelper
	{

		public sealed class HttpDataResponseEmbed<T>
		{
			public HttpResponseMessage httpResponseMessage;

			public T Value { get; set; }

			public static implicit operator T(HttpDataResponseEmbed<T> httpDataResponseEmbed) => httpDataResponseEmbed.Value;
		}

		public static async Task<HttpDataResponseEmbed<T>> GetForCode<T, T2>(this HttpDataResponseEmbed<T2> httpResponse, HttpStatusCode httpStatusCode, Action<T> action) where T : class {
			return await GetForCode(httpResponse.httpResponseMessage, httpStatusCode, action);
		}
		public static async Task<HttpDataResponseEmbed<T>> GetForCode<T, T2>(this Task<HttpDataResponseEmbed<T2>> httpResponse, HttpStatusCode httpStatusCode, Action<T> action) where T : class {
			return await GetForCode((await httpResponse).httpResponseMessage, httpStatusCode, action);
		}

		public static async Task<HttpDataResponseEmbed<T>> GetForCode<T>(this HttpResponseMessage httpResponse, HttpStatusCode httpStatusCode, Action<T> action) where T : class {
			if (httpResponse is null) {
				return new HttpDataResponseEmbed<T> { httpResponseMessage = httpResponse };
			}
			if (httpResponse.StatusCode != httpStatusCode) {
				return new HttpDataResponseEmbed<T> { httpResponseMessage = httpResponse };
			}

			var httpDataResponse = new HttpDataResponseEmbed<T> {
				httpResponseMessage = httpResponse,
				Value = JsonConvert.DeserializeObject<T>(await httpResponse.Content.ReadAsStringAsync())
			};
			if (action is not null) {
				if (httpDataResponse.Value is not null) {
					action(httpDataResponse.Value);
				}
			}
			return httpDataResponse;
		}

		public static async Task<HttpDataResponseEmbed<T>> GetForCode<T, T2>(this HttpDataResponseEmbed<T2> httpResponse, HttpStatusCode httpStatusCode, Func<T, Task> action = null) where T : class {
			return await GetForCode(httpResponse.httpResponseMessage, httpStatusCode, action);
		}

		public static async Task<HttpDataResponseEmbed<T>> GetForCode<T, T2>(this Task<HttpDataResponseEmbed<T>> httpResponse, HttpStatusCode httpStatusCode, Func<T, Task> action = null) where T : class {
			return await GetForCode((await httpResponse).httpResponseMessage, httpStatusCode, action);
		}


		public static async Task<HttpDataResponseEmbed<T>> GetForCode<T>(this Task<HttpDataResponseEmbed<T>> httpResponse, HttpStatusCode httpStatusCode) where T : class {
			var reqwest = await httpResponse;
			if (reqwest.Value is null) {
				reqwest = await GetForCode<T>(reqwest.httpResponseMessage, httpStatusCode);
			}
			return reqwest;
		}

		public static async Task<HttpDataResponseEmbed<T>> GetForCode<T>(this HttpResponseMessage httpResponse, HttpStatusCode httpStatusCode, Func<T, Task> action = null) where T : class {
			if (httpResponse is null) {
				return new HttpDataResponseEmbed<T> { httpResponseMessage = httpResponse };
			}
			if (httpResponse.StatusCode != httpStatusCode) {
				return new HttpDataResponseEmbed<T> { httpResponseMessage = httpResponse };
			}

			var httpDataResponse = new HttpDataResponseEmbed<T> {
				httpResponseMessage = httpResponse,
				Value = JsonConvert.DeserializeObject<T>(await httpResponse.Content.ReadAsStringAsync())
			};
			if (action is not null) {
				if (httpDataResponse.Value is not null) {
					await action(httpDataResponse.Value);
				}
			}
			return httpDataResponse;
		}

	}
}
