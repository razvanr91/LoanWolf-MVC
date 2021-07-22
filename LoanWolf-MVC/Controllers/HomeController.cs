using LoanWolf_MVC.Helpers;
using LoanWolf_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LoanWolf_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Wolf()
        {
            Loan loan = new();

            loan.Payment = 0.0m;
            loan.Rate = 3.5m;
            loan.Term = 60;
            loan.TotalInterest = 0.0m;
            loan.TotalCost = 0.0m;
            loan.Amount = 150000m;

            return View(loan);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Wolf(Loan loan)
        {
            // Calculate the loan
            var loanHelper = new LoanHelper();

            Loan userLoan = loanHelper.GetPayments(loan);

            return View(userLoan);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
