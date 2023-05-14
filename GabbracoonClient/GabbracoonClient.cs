namespace GabbracoonClient
{
	public sealed class GabbracoonClient
	{
		private readonly GabbracoonClientManager _gabbracoonClientManager;
		public Guid UniqueInstanceIdentifier;

		internal GabbracoonClient(GabbracoonClientManager gabbracoonClientManager) { 
			UniqueInstanceIdentifier = Guid.NewGuid();
			_gabbracoonClientManager = gabbracoonClientManager;
		}
		
		internal GabbracoonClient(GabbracoonClientManager gabbracoonClientManager,Guid uniqueInstanceIdentifier) {
			UniqueInstanceIdentifier = uniqueInstanceIdentifier;
			_gabbracoonClientManager = gabbracoonClientManager;
		}

	}
}