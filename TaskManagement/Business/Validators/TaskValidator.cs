using FluentValidation;
using TaskManagement.Business.Models;
using TaskManagement.Entities;

namespace TaskManagement.Business.Validators
{
    public class TaskValidator : AbstractValidator<TaskItemDto>
    {
        public TaskValidator()
        {
            RuleFor(task => task.Title).NotEmpty().NotNull().WithMessage("Title is required");
            RuleFor(task => task.Description).NotEmpty().NotNull().WithMessage("Description is required");
            RuleFor(task => task.DueDate)
                .NotEmpty().NotNull().WithMessage("Due date is required")
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Due date cannot be in the past");
            RuleFor(task => task.Priority).NotNull().WithMessage("Priority is required");
            RuleFor(task => task.Status).NotNull().WithMessage("Status is required");
        }
    }
}
