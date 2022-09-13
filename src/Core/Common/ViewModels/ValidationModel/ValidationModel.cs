namespace Common.ViewModels.ValidationModel
{
    public class ValidationModel
    {
        public string Id { get; set; }
        public int Length { get; set; }
        public string Name { get; set; }
    }
    public class AddValidationField
    {
        public int Length { get; set; }
        public string Name { get; set; }
    }
    public class EditValidationField
    {

        public string Id { get; set; }
        public int Length { get; set; }
        public string Name { get; set; }
    }
}
