using Melior.InterviewQuestion.Data;
using Melior.InterviewQuestion.Services;
using Melior.InterviewQuestion.Types;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melior.InterviewQuestion.Tests
{
	public class PaymentServiceMock
	{
		internal PaymentService SystemUnderTest { get; set; }

		internal Mock<IAccountDataStore> _mockBackupAccountDataStore;

		internal Mock<IAccountDataStore> _mockAccountDataStore;

		public MakePaymentRequest makePaymentRequest { get; set; }

		public Account account { get; set; }

		public PaymentServiceMock() 
		{
			_mockBackupAccountDataStore = new Mock<IAccountDataStore>();
			_mockAccountDataStore = new Mock<IAccountDataStore>();
			SystemUnderTest = new PaymentService(_mockBackupAccountDataStore.Object, _mockAccountDataStore.Object);
		}

		public void SetupDefaultAccount()
		{
			makePaymentRequest = new MakePaymentRequest
			{
				DebtorAccountNumber = "12345678",
				Amount = 100,
				PaymentScheme = PaymentScheme.Bacs
			};

			account = new Account
			{
				AccountNumber = "12345678",
				Balance = 200,
				AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
			};

			_mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);			
		}

		public void SetupInvalidBacsPayment()
		{
			makePaymentRequest = new MakePaymentRequest
			{
				DebtorAccountNumber = "12345678",
				Amount = 100,
				PaymentScheme = PaymentScheme.Bacs
			};

			account = new Account
			{
				AccountNumber = "12345678",
				Balance = 50, // Insufficient balance for the payment
				AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
			};

			_mockBackupAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
		}

		public void SetupBackUpAccountDataStore()
		{
			makePaymentRequest = new MakePaymentRequest
			{
				DebtorAccountNumber = "12345678",
				Amount = 100,
				PaymentScheme = PaymentScheme.Bacs
			};

			account = new Account
			{
				AccountNumber = "12345678",
				Balance = 200,
				AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
			};

			_mockBackupAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
		}

		public void SetupInvalidBacsPaymentUsingBackUpAccount()
		{
			makePaymentRequest = new MakePaymentRequest
			{
				DebtorAccountNumber = "12345678",
				Amount = 100,
				PaymentScheme = PaymentScheme.Bacs
			};

			account = new Account
			{
				AccountNumber = "12345678",
				Balance = 50, // Insufficient balance for the payment
				AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
			};

			_mockBackupAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
		}
	}
}
