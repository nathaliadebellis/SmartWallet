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

    public TransactionsController(
        IFinancialTransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<IActionResult> Index()
    {
        var transactions = await _transactionService.GetAllAsync();

        return View(transactions);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new TransactionFormViewModel();

        LoadLists(model);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TransactionFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            LoadLists(model);
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

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var transaction = await _transactionService.GetByIdAsync(id);

        if (transaction is null)
            return NotFound();

        var model = new TransactionFormViewModel
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Amount = transaction.Amount,
            TransactionDate = transaction.TransactionDate,
            Type = transaction.Type,
            CategoryId = transaction.CategoryId,
            Notes = transaction.Notes
        };

        LoadLists(model);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TransactionFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            LoadLists(model);
            return View(model);
        }

        var dto = new UpdateFinancialTransactionDto
        {
            Id = model.Id,
            Description = model.Description,
            Amount = model.Amount,
            TransactionDate = model.TransactionDate,
            Type = model.Type,
            CategoryId = model.CategoryId,
            Notes = model.Notes
        };

        await _transactionService.UpdateAsync(dto);

        TempData["Success"] = "Transação atualizada com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var transaction = await _transactionService.GetByIdAsync(id);

        if (transaction is null)
            return NotFound();

        return View(transaction);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _transactionService.DeleteAsync(id);

        TempData["Success"] = "Transação excluída com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    private static void LoadLists(TransactionFormViewModel model)
    {
        // As categorias são carregadas dinamicamente via JavaScript
        // de acordo com o tipo da transação selecionado.
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
    }
}