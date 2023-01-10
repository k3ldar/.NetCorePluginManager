using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Classes.Search
{
	/// <summary>
	/// Base search provider
	/// </summary>
	public abstract class BaseSearchProvider
	{
		private const string ExcludedWords = "a\tabout\tactually\talmost\talso\talthough\talways\tam\tan\tand\t" +
			"any\tare\tas\tat\tbe\tbecame\tbecome\tbut\tby\tcan\tcould\tdid\tdo\tdoes\teach\teither\telse\tfor\t" +
			"from\thad\thas\thave\thence\thow\ti\tif\tin\tis\tit\tits\tjust\tmay\tmaybe\tme\tmight\tmine\tmust\tmy\t" +
			"mine\tmust\tmy\tneither\tnor\tnot\tof\toh\tok\twhen\twhere\twhereas\twherever\twhenever\twhether\t" +
			"which\twhile\twho\twhom\twhoever\twhose\twhy\twill\twith\twithin\twithout\twould\tyes\tyet\tyou\tyour\t";

		/// <summary>
		/// Validates a word to see if it should be excluded
		/// </summary>
		/// <param name="word"></param>
		/// <returns></returns>
		protected static bool ExcludeWordFromSearch(string word)
		{
			if (String.IsNullOrWhiteSpace(word))
				return true;

			if (ExcludedWords.IndexOf($"{word.ToLower()}\t") > -1) 
				return true;

			return false;
		}
	}
}
