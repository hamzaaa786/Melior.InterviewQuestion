using Melior.InterviewQuestion.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melior.InterviewQuestion.Data
{
	/// <summary>
	///		Interface for account data store
	/// </summary>
	public interface IAccountDataStore
	{
		/// <summary>
		///    Get account by account number
		/// </summary>
		/// <param name="accountNumber">
		///		Acount number as string.
		/// </param>
		/// <returns>
		///		Get account by account number
		/// </returns>
		Account GetAccount(string accountNumber);

		/// <summary>
		///		Update account
		/// </summary>
		/// <param name="account">
		///		Acount to update.
		/// </param>
		void UpdateAccount(Account account);
	}
}
