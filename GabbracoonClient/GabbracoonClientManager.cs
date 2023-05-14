using System.Net.Http.Json;

using RequestModels;

namespace GabbracoonClient
{
	public sealed class GabbracoonClientManager
	{
		private readonly List<GabbracoonClient> _gabbracoonClients = new();

		public async Task<(bool, Uri)> IsVailedPlatform(string targetPlatform) {
			targetPlatform = targetPlatform.ToLower();
			if (string.IsNullOrEmpty(targetPlatform)) {
				return (false, null);
			}
			try {
				if (!(targetPlatform.Contains('.') && Uri.TryCreate(targetPlatform, new UriCreationOptions(), out var targetURI))) {
					if (targetPlatform == "rhubarbvr") {
						targetURI = new Uri("https://api.rhubarbvr.net/");
					}
					else if (targetPlatform == "localhost") {
						targetURI = new Uri("http://localhost:5000/");
					}
					else if (targetPlatform.StartsWith("localhost:")) {
						targetURI = new Uri($"http://{targetPlatform}/");
					}
					else {
						targetURI = new Uri("https://" + targetPlatform + ".com/");
					}
				}

				using var tempClient = new HttpClient();
				tempClient.BaseAddress = targetURI;
				return ((await tempClient.GetAsync("gabbracoon_health")).StatusCode == System.Net.HttpStatusCode.OK, targetURI);
			}
			catch {
				return (false, null);
			}
		}

		public async Task<(bool created, LocalText error_msg)> RegisterAccount(Uri targetPlatform, RegisterAccount registerAccount) {
			using var tempClient = new HttpClient();
			tempClient.BaseAddress = targetPlatform;
			var reqwest = await tempClient.PostAsync("Authentication/Register", JsonContent.Create(registerAccount));
			var reqwestData = await reqwest.GetForCode<LocalText>(System.Net.HttpStatusCode.OK).GetForCode(System.Net.HttpStatusCode.BadRequest);
			return (reqwestData.httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK, reqwestData);
		}

	}
}