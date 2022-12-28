namespace InternalPortal.Web.Models.Shared
{
    public class QuestionViewModel
    {
        public string? Question { get; set; }
        public string? Hint { get; set; }
        public string? Type { get; set; }

        public QuestionViewModel()
        {
        }

        public QuestionViewModel(string questionText, string? hint = null, string? type = null)
        {
            Question = questionText;
            Hint = hint;
            Type = type;
        }
    }
}
