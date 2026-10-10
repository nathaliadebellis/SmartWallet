using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartWallet.Web.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartWallet.Application.DTOs.FinancialTransactions;
using SmartWallet.Application.Interfaces;
using SmartWallet.Domain.Enums;
using SmartWallet.Domain.Exceptions;
using SmartWallet.Web.ViewModels.Transactions;

namespace SmartWallet.Web.Controllers;

using Microsoft.AspNetCore.Authorization;

[Authorize]
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

    public async Task<IActionResult> Index(TransactionFilterViewModel filter)
    {
        var userId = User.GetUserId();

        if (!TransactionFilterViewModel.PageSizeOptions.Contains(filter.PageSize))
            filter.PageSize = TransactionFilterViewModel.PageSizeOptions[1];

        var result = await _transactionService.SearchAsync(new TransactionFilterDto
        {
            Search = filter.Search,
            Type = filter.Type,
            CategoryId = filter.CategoryId,
            From = filter.From,
            To = filter.To,
            Page = filter.Page,
            PageSize = filter.PageSize
        }, userId);

        var categories = await _categoryService.GetAllAsync(userId);

        var model = new TransactionListViewModel
        {
            Filter = filter,
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages,
            Items = result.Items.Select(t => new TransactionListItemViewModel
            {
                Id = t.Id,
                Type = t.Type,
                Description = t.Description,
                CategoryName = t.CategoryName,
                Amount = t.Amount,
                TransactionDate = t.TransactionDate
            }).ToList(),
            Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == filter.CategoryId
            }),
            TransactionTypes = GetTransactionTypeItems()
        };

        return View(model);
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

        var userId = User.GetUserId();

        try
        {
            await _transactionService.CreateAsync(dto, userId);
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            LoadLists(model);
            return View(model);
        }

        TempData["Success"] = "Transação cadastrada com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = User.GetUserId();

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

        var userId = User.GetUserId();

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

        try
        {
            await _transactionService.UpdateAsync(dto, userId);
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            LoadLists(model);
            return View(model);
        }

        TempData["Success"] = "Transação atualizada com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.GetUserId();

        var transaction = await _transactionService.GetByIdAsync(id, userId);

        if (transaction is null)
            return NotFound();

        return View(transaction);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userId = User.GetUserId();

        var existing = await _transactionService.GetByIdAsync(id, userId);
        if (existing is null)
            return NotFound();

        try
        {
            await _transactionService.DeleteAsync(id, userId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Não foi possível excluir a transação. Tente novamente.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Transação excluída com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    private static void LoadLists(TransactionFormViewModel model)
    {
        model.Categories = Enumerable.Empty<SelectListItem>();

        model.TransactionTypes = GetTransactionTypeItems();
    }

    private static IEnumerable<SelectListItem> GetTransactionTypeItems()
    {
        return Enum
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
            })
            .ToList();
    }
}