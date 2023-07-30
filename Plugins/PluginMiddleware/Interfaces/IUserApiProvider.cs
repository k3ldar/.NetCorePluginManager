namespace Middleware
{
	/// <summary>
	/// Interface for user api provider
	/// </summary>
	public interface IUserApiProvider
	{
		/// <summary>
		/// Adds a new api key/secret for a user
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="apiKey"></param>
		/// <param name="secret"></param>
		/// <returns></returns>
		bool AddApi(long userId, string apiKey, string secret);

		/// <summary>
		/// Retrieves the merchant id for a user
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		string GetMerchantId(long userId);
	}
}
