using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartWallet.Application.DTOs.Categories;
using SmartWallet.Application.Interfaces;
using SmartWallet.Domain.Enums;
using SmartWallet.Web.ViewModels.Categories;

namespace SmartWallet.Web.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();

        return View(categories);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new CategoryFormViewModel();

        LoadTransactionTypes(model);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            LoadTransactionTypes(model);
            return View(model);
        }

        var dto = new CreateCategoryDto
        {
            Name = model.Name,
            Description = model.Description,
            Icon = model.Icon,
            Color = model.Color,
            TransactionType = model.TransactionType
        };

        await _categoryService.CreateAsync(dto);

        TempData["Success"] = "Categoria criada com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category is null)
            return NotFound();

        var model = new CategoryFormViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Icon = category.Icon,
            Color = category.Color,
            TransactionType = category.TransactionType
        };

        LoadTransactionTypes(model);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            LoadTransactionTypes(model);
            return View(model);
        }

        var dto = new UpdateCategoryDto
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Icon = model.Icon,
            Color = model.Color,
            TransactionType = model.TransactionType
        };

        await _categoryService.UpdateAsync(dto);

        TempData["Success"] = "Categoria atualizada com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category is null)
            return NotFound();

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _categoryService.DeleteAsync(id);

        TempData["Success"] = "Categoria removida com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    private static void LoadTransactionTypes(CategoryFormViewModel model)
    {
        model.TransactionTypes = Enum
            .GetValues<TransactionType>()
            .Select(type => new SelectListItem
            {
                Value = type.ToString(),
                Text = GetTransactionTypeDisplayName(type)
            });
    }

    private static string GetTransactionTypeDisplayName(TransactionType type)
    {
        return type switch
        {
            TransactionType.Income => "Receita",
            TransactionType.Expense => "Despesa",
            _ => type.ToString()
        };
    }

    [HttpGet]
    public async Task<IActionResult> GetByTransactionType(TransactionType transactionType)
    {
        var categories = await _categoryService
            .GetByTransactionTypeAsync(transactionType);

        var result = categories.Select(category => new
        {
            id = category.Id,
            name = category.Name
        });

        return Json(result);
    }
}