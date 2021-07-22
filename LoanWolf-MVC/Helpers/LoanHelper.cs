using LoanWolf_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanWolf_MVC.Helpers
{
    public class LoanHelper
    {
        public Loan GetPayments(Loan loan)
        {
            // Calculate monthly payment
            loan.Payment = CalculatePayment(loan.Amount, loan.Rate, loan.Term);

            // Create a loop from 1 to term length
            // calculate payment schedule
            var balance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var monthlyRate = CalculateMonthlyRate(loan.Rate);

            for ( var i = 1; i <= loan.Term; i++ )
            {
                monthlyInterest = CalculateMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                LoanPayment loanPayment = new();

                loanPayment.Month = i;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrincipal = monthlyPrincipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;

                loan.Payments.Add(loanPayment);
            }

            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest;

            // push payments to loan

            //return loan
            return loan;
        }

        private decimal CalculatePayment(decimal amount, decimal rate, int term)
        {
            decimal payment = 0.0m;

            var rateD = Convert.ToDouble(CalculateMonthlyRate(rate));
            var amountD = Convert.ToDouble(amount);

            var paymentD = ( amountD * rateD ) / ( 1 - Math.Pow(1 + rateD, -term) );

            return Convert.ToDecimal(paymentD);
        }

        private decimal CalculateMonthlyRate(decimal rate)
        {
            return rate / 1200;
        }

        private decimal CalculateMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }
    }
}
