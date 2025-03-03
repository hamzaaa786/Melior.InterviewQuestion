using Melior.InterviewQuestion.Types;

namespace Melior.InterviewQuestion.Services
{
	/// <summary>
	///		Interface for payment service
	/// </summary>
	public interface IPaymentService
    {
        MakePaymentResult MakePayment(MakePaymentRequest request);
    }
}
