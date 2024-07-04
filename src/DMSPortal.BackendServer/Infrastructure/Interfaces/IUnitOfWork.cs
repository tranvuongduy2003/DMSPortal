﻿using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IAttendancesRepository Attendances { get; }
    
    IBranchesRepository Branches { get; }
    
    IClassesRepository Classes { get; }
    
    IClassInShiftsRepository ClassInShifts { get; }
    
    ICommandInFunctionsRepository CommandInFunctions { get; }
    
    ICommandsRepository Commands { get; }
    
    IFunctionsRepository Functions { get; }
    
    INotesRepository Notes { get; }
    
    IPermissionsRepository Permissions { get; }
    
    IPitchesRepository Pitches { get; }
    
    IPitchGroupsRepository PitchGroups { get; }
    
    IShiftsRepository Shifts { get; }
    
    IStudentInClassesRepository StudentInClasses { get; }
    
    IStudentsRepository Students { get; }
    
    void Dispose();
    
    Task<int> CommitAsync();
}