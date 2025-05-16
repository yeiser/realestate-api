using FluentValidation;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Application.Validators.CustomRules
{
    public static class ImageValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeValidBase64Image<T>(this IRuleBuilder<T, string> ruleBuilder, int maxMegabytes = 20)
        {
            return ruleBuilder.Must((_, base64String) =>
            {
                if (string.IsNullOrWhiteSpace(base64String))
                    return false;

                try
                {
                    var base64 = base64String.Split(',').Last();
                    var bytes = Convert.FromBase64String(base64);

                    var maxBytes = maxMegabytes * 1024 * 1024;
                    if (bytes.Length > maxBytes)
                        return false;

                    using var image = Image.Load<Rgba32>(bytes);
                    return image != null;
                }
                catch
                {
                    return false;
                }
            })
            .WithMessage($"El campo debe ser una imagen base64 válida y pesar menos de {maxMegabytes}MB.");
        }
    }
}
