using AutoMapper;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.Models.DTOs.Branch;
using DMSPortal.Models.DTOs.Class;
using DMSPortal.Models.DTOs.Command;
using DMSPortal.Models.DTOs.Function;
using DMSPortal.Models.DTOs.Note;
using DMSPortal.Models.DTOs.Pitch;
using DMSPortal.Models.DTOs.PitchGroup;
using DMSPortal.Models.DTOs.Student;
using DMSPortal.Models.DTOs.User;
using DMSPortal.Models.Requests.Branch;
using DMSPortal.Models.Requests.Class;
using DMSPortal.Models.Requests.Note;
using DMSPortal.Models.Requests.Pitch;
using DMSPortal.Models.Requests.PitchGroup;
using DMSPortal.Models.Requests.Student;

namespace DMSPortal.BackendServer;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, IncludedUserDto>();

        CreateMap<Command, CommandDto>().ReverseMap();

        CreateMap<Function, FunctionDto>().ReverseMap();

        CreateMap<PitchGroup, PitchGroupDto>().ReverseMap();
        CreateMap<PitchGroup, IncludedPitchGroupDto>();
        CreateMap<CreatePitchGroupRequest, PitchGroup>();
        CreateMap<UpdatePitchGroupRequest, PitchGroup>().ReverseMap();

        CreateMap<Branch, BranchDto>().ReverseMap();
        CreateMap<Branch, IncludedBranchDto>();
        CreateMap<CreateBranchRequest, Branch>();
        CreateMap<UpdateBranchRequest, Branch>().ReverseMap();

        CreateMap<Class, ClassDto>().ReverseMap();
        CreateMap<CreateClassRequest, Class>();
        CreateMap<UpdateClassRequest, Class>().ReverseMap();

        CreateMap<Pitch, PitchDto>().ReverseMap();
        CreateMap<Pitch, IncludedPitchDto>();
        CreateMap<CreatePitchRequest, Pitch>();
        CreateMap<UpdatePitchRequest, Pitch>().ReverseMap();


        CreateMap<Note, NoteDto>().ReverseMap();
        CreateMap<CreateNoteRequest, Note>();
        CreateMap<UpdateNoteRequest, Note>().ReverseMap();

        CreateMap<Student, StudentDto>().ReverseMap();
        CreateMap<CreateStudentRequest, Student>();
        CreateMap<UpdateStudentRequest, Student>().ReverseMap();
    }
}