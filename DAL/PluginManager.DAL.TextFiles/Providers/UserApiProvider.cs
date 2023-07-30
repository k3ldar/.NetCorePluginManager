using Middleware;

using PluginManager.DAL.TextFiles.Tables;

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Providers
{
	internal class UserApiProvider : IUserApiProvider
	{
		private const int MaximumAttemptsToObtainUniqueMerchantId = 3000;
		private readonly ISimpleDBOperations<UserApiDataRow> _userApiDataRow;

		public UserApiProvider(ISimpleDBOperations<UserApiDataRow> userApiDataRow)
		{
			_userApiDataRow = userApiDataRow ?? throw new ArgumentNullException(nameof(userApiDataRow));
		}

		public bool AddApi(long userId, string apiKey, string secret)
		{
			UserApiDataRow currUserData = _userApiDataRow.Select().FirstOrDefault(x => x.UserId == userId);
			string merchantId;

			if (currUserData == null)
				merchantId = CreateUniqueMerchantId();
			else
				merchantId = currUserData.MerchantId;

			UserApiDataRow newApi = new UserApiDataRow()
			{
				UserId = userId,
				MerchantId = merchantId,
				ApiKey = apiKey,
				Secret = secret,
			};

			_userApiDataRow.Insert(newApi);

			return _userApiDataRow.Select().Any(a => a.UserId == userId && a.ApiKey.Equals(apiKey) && a.Secret.Equals(secret));
		}

		public string GetMerchantId(long userId)
		{
			UserApiDataRow userApiDataRow = _userApiDataRow.Select().FirstOrDefault(a => a.UserId == userId);

			if (userApiDataRow == null)
				return null;

			return userApiDataRow.MerchantId;
		}

		private string CreateUniqueMerchantId()
		{
			string Result = "mer" + Shared.Utilities.GetRandomWord(12, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
			int attempts = 0;

			while (_userApiDataRow.Select().Any(x => x.MerchantId.Equals(Result, StringComparison.InvariantCultureIgnoreCase)))
			{
				Result = "mer" + Shared.Utilities.GetRandomWord(12, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
				attempts++;

				if (attempts > MaximumAttemptsToObtainUniqueMerchantId)
					throw new InvalidOperationException("Failed to secure unique merchant id");
			}

			return Result;
		}
	}
}
