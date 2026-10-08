using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SmartWallet.Web.Binders;

/// <summary>
/// Inputs type="number" always post "12.50" regardless of the UI culture, while
/// users may also type "12,50". The default pt-BR binder rejects the former.
/// </summary>
public class DecimalModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (result == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, result);

        var raw = result.FirstValue?.Trim();

        if (string.IsNullOrEmpty(raw))
            return Task.CompletedTask;

        if (raw.Contains(',') && raw.Contains('.'))
            raw = raw.Replace(".", string.Empty);

        raw = raw.Replace(',', '.');

        if (decimal.TryParse(raw, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var value))
        {
            bindingContext.Result = ModelBindingResult.Success(value);
        }
        else
        {
            bindingContext.ModelState.TryAddModelError(
                bindingContext.ModelName,
                "Informe um valor numérico válido.");
        }

        return Task.CompletedTask;
    }
}

public class DecimalModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        return context.Metadata.UnderlyingOrModelType == typeof(decimal)
            ? new DecimalModelBinder()
            : null;
    }
}
