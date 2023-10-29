using System.ComponentModel.DataAnnotations;

namespace prof_tester_api.src.Domain.Entities.Request
{
    public class CreateTestBody
    {
        [Required]
        public Guid DepartmentId { get; set; }
        public string Name { get; set; }
        public List<CreateQuestionBody> Questions { get; set; }
    }

    public class CreateQuestionBody
    {
        public string Name { get; set; }
        public List<string> Answers { get; set; }
        [Range(0, int.MaxValue)]
        public int RightAnswerIndex { get; set; }
    }
}