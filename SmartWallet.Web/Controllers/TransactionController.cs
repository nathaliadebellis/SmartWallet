using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartWallet.Application.DTOs.FinancialTransactions;
using SmartWallet.Application.Interfaces;
using SmartWallet.Domain.Enums;
using SmartWallet.Web.ViewModels.Transactions;

namespace SmartWallet.Web.Controllers;

using Microsoft.AspNetCore.Authorization;

[Authorize]
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

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        await _transactionService.CreateAsync(dto, userId);

        TempData["Success"] = "Transação cadastrada com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var transaction = await _transactionService.GetByIdAsync(id, userId);

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

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var existing = await _transactionService.GetByIdAsync(model.Id, userId);
        if (existing is null)
            return NotFound();

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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var transaction = await _transactionService.GetByIdAsync(id, userId);

        if (transaction is null)
            return NotFound();

        return View(transaction);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var existing = await _transactionService.GetByIdAsync(id, userId);
        if (existing is null)
            return NotFound();

        await _transactionService.DeleteAsync(id);

        TempData["Success"] = "Transação excluída com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    private static void LoadLists(TransactionFormViewModel model)
    {
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