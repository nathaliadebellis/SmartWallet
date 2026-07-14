using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartWallet.Application.DTOs.FinancialTransactions;
using SmartWallet.Application.Interfaces;
using SmartWallet.Domain.Enums;
using SmartWallet.Web.ViewModels.Transactions;

namespace SmartWallet.Web.Controllers;

public class TransactionsController : Controller
{
    private readonly IFinancialTransactionService _transactionService;
    private readonly ICategoryService _categoryService;

    public TransactionsController(
        IFinancialTransactionService transactionService,
        ICategoryService categoryService)
    {
        _transactionService = transactionService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var transactions = await _transactionService.GetAllAsync();

        return View(transactions);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new TransactionFormViewModel();

        await LoadListsAsync(model);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TransactionFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadListsAsync(model);
            return View(model);
        }

        var dto = new CreateFinancialTransactionDto
        {
            Description = model.Description,
            Amount = model.Amount,
            TransactionDate = model.TransactionDate,
            Type = model.Type,
            CategoryId = model.CategoryId,
            Notes = model.Notes
        };

        await _transactionService.CreateAsync(dto);

        TempData["Success"] = "Transação cadastrada com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    private Task LoadListsAsync(TransactionFormViewModel model)
    {
        // As categorias serão carregadas via JavaScript,
        // de acordo com o tipo selecionado.
        model.Categories = Enumerable.Empty<SelectListItem>();

        model.TransactionTypes = Enum
            .GetValues<TransactionType>()
            .Select(type => new SelectListItem
            {
                Value = type.ToString(),
                Text = type switch
                {
                    TransactionType.Income => "Receita",
                    TransactionType.Expense => "Despesa",
                    _ => type.ToString()
                }
            });

        return Task.CompletedTask;
    }
}