using Melior.InterviewQuestion.Types;
using Moq;
using System.Configuration;

namespace Melior.InterviewQuestion.Tests
{
	[TestClass]
	public sealed class PaymentServiceTests
	{
		[TestMethod]
		public void Ensure_MakePayment_WithValidBacsPayment_ShouldReturnSuccess()
		{
			// Arrange
			var mock = new PaymentServiceMock();
			mock.SetupDefaultAccount();

			// Act
			ConfigurationManager.AppSettings["DataStoreType"] = "Primary";
			var result = mock.SystemUnderTest.MakePayment(mock.makePaymentRequest);

			// Assert
			Assert.IsTrue(result.Success);
			mock._mockAccountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
		}

		[TestMethod]
		public void Ensure_MakePayment_WithInvalidBacsPayment_ShouldReturnFailure()
		{
			// Arrange
			var mock = new PaymentServiceMock();
			mock.SetupInvalidBacsPayment();
			ConfigurationManager.AppSettings["DataStoreType"] = "Primary";

			// Act
			var result = mock.SystemUnderTest.MakePayment(mock.makePaymentRequest);

			// Assert
			Assert.IsFalse(result.Success);
			mock._mockAccountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
		}

		[TestMethod]
		public void Ensure_MakePayment_WithValidBacsPaymentAndBackupDataStore_ShouldReturnSuccess()
		{
			// Arrange
			var mock = new PaymentServiceMock();
			mock.SetupBackUpAccountDataStore();
			ConfigurationManager.AppSettings["DataStoreType"] = "Backup";

			// Act
			var result = mock.SystemUnderTest.MakePayment(mock.makePaymentRequest);

			// Assert
			Assert.IsTrue(result.Success);
			mock._mockBackupAccountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
		}

		[TestMethod]
		public void Ensure_MakePayment_WithInvalidBacsPaymentAndBackupDataStore_ShouldReturnFailure()
		{
			// Arrange
			var mock = new PaymentServiceMock();
			mock.SetupInvalidBacsPaymentUsingBackUpAccount();
			ConfigurationManager.AppSettings["DataStoreType"] = "Backup";

			// Act
			var result = mock.SystemUnderTest.MakePayment(mock.makePaymentRequest);

			// Assert
			Assert.IsFalse(result.Success);
			mock._mockBackupAccountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
		}

		[TestMethod]
		public void Ensure_MakePayment_WithInvalidAccount_ShouldReturnFailure()
		{
			// Arrange
			var mock = new PaymentServiceMock();
			mock.SetupInvalidBacsPaymentUsingBackUpAccount();
			mock._mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns<Account>(null);
			ConfigurationManager.AppSettings["DataStoreType"] = "Primary";

			// Act
			var result = mock.SystemUnderTest.MakePayment(mock.makePaymentRequest);

			// Assert
			Assert.IsFalse(result.Success);
			mock._mockAccountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
		}
	}
}
