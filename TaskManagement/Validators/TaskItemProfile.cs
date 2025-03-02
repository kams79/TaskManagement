using AutoMapper;
using TaskManagement.Entities;
using TaskManagement.Models;

namespace TaskManagement.Validators
{
    public class TaskItemProfile : Profile
    {
        public TaskItemProfile()
        {
            CreateMap<TaskItemDto, TaskItem>();
            CreateMap<TaskItem, TaskItemDto>();
            //CreateMap<TaskItemDetailsDto, TaskItem>()
            //    .ForMember(d => d.AssignedUser, o => o.MapFrom(s => s.AssignedUser != null ? new User { Email = s.AssignedUser.Email, Username = s.AssignedUser.Username} : null));
            CreateMap<TaskItem, TaskItemDetailsDto>()
                .ForMember(d => d.AssignedUser, o => o.MapFrom(s => s.TaskOwner != null ? new UserDto { Email = s.TaskOwner.Email, Username = s.TaskOwner.Username } : null));
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<User, UserWithoutTaskDto>();
            CreateMap<UserWithoutTaskDto, User>();

        }
    }
}
