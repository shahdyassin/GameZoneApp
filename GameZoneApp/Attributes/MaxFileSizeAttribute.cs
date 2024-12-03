namespace GameZoneApp.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int MaxFileSize)
        {
            _maxFileSize = MaxFileSize;
        }

        protected override ValidationResult? IsValid
            (object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if(file.Length > _maxFileSize)
                {
                    return new ValidationResult($"Maximum Allowed Size is {_maxFileSize} Bytes");
                }
            }
            return ValidationResult.Success;
        }
    }
}
