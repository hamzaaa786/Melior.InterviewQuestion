using Melior.InterviewQuestion.Data;
using Melior.InterviewQuestion.Types;
using System.Configuration;

namespace Melior.InterviewQuestion.Services
{
	/// <summary>
	///		Payment service.
	/// </summary>
	public class PaymentService : IPaymentService
	{
		/// <summary>
		///		Backup account data store.
		/// </summary>
		private readonly IAccountDataStore _backupAccountDataStore;

		/// <summary>
		///		Account data store.
		/// </summary>
		private readonly IAccountDataStore _accountDataStore;

		/// <summary>
		///		Initializes a new instance of the <see cref="PaymentService"/> class.
		/// </summary>
		/// <param name="backupAccountDataStore">
		///		Backup account data store parameter.
		/// </param>
		/// <param name="accountDataStore">
		///		Acount data store parameter.
		/// </param>
		public PaymentService(IAccountDataStore backupAccountDataStore, IAccountDataStore accountDataStore)
		{
			_backupAccountDataStore = backupAccountDataStore;
			_accountDataStore = accountDataStore;
		}

		/// <summary>
		///    Make payment method.
		/// </summary>
		/// <param name="request">
		///		Makes payment request.
		/// </param>
		/// <returns>
		///		Makes payment result.
		/// </returns>
		public MakePaymentResult MakePayment(MakePaymentRequest request)
		{
			var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];
			var accountDataStore = dataStoreType == "Backup" ? _backupAccountDataStore : _accountDataStore;
			var account = accountDataStore.GetAccount(request.DebtorAccountNumber);

			var result = new MakePaymentResult { Success = ValidatePayment(request, account) };

			if (result.Success)
			{
				account.Balance -= request.Amount;

				accountDataStore.UpdateAccount(account);
			}

			return result;
		}

		/// <summary>
		///    Validates payment.
		/// </summary>
		/// <param name="request">
		///		Makes payment request.
		/// </param>
		/// <param name="account">
		///		Account request.
		/// </param>
		/// <returns>
		///		Boolean value that states if payment is validated.
		/// </returns>
		private bool ValidatePayment(MakePaymentRequest request, Account account)
		{
			if (account == null)
				return false;

			switch (request.PaymentScheme)
			{
				case PaymentScheme.Bacs:
					return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs) && account.Balance >= request.Amount;

				case PaymentScheme.FasterPayments:
					return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) && account.Balance >= request.Amount;

				case PaymentScheme.Chaps:
					return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) && account.Status == AccountStatus.Live;

				default:
					return false;
			}
		}
	}
}