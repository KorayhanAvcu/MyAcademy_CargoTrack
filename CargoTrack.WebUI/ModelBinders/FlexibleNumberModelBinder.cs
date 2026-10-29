using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace CargoTrack.WebUI.ModelBinders
{
    public class FlexibleNumberModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueResult == ValueProviderResult.None)
                return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);

            var raw = valueResult.FirstValue?.Trim();
            if (string.IsNullOrEmpty(raw))
                return Task.CompletedTask;

            var normalized = raw.Replace(" ", "").Replace(',', '.');
            var styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
            var type = Nullable.GetUnderlyingType(bindingContext.ModelType) ?? bindingContext.ModelType;

            object? result = null;
            if (type == typeof(decimal) && decimal.TryParse(normalized, styles, CultureInfo.InvariantCulture, out var dec))
                result = dec;
            else if (type == typeof(double) && double.TryParse(normalized, styles, CultureInfo.InvariantCulture, out var dbl))
                result = dbl;

            if (result is null)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, $"'{raw}' geçerli bir sayı değil.");
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }

    public class FlexibleNumberModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            var type = Nullable.GetUnderlyingType(context.Metadata.ModelType) ?? context.Metadata.ModelType;
            return type == typeof(decimal) || type == typeof(double) ? new FlexibleNumberModelBinder() : null;
        }
    }
}
