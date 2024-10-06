using DMSPortal.BackendServer.Abstractions.Repository.Contracts;
using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Repositories;

namespace DMSPortal.BackendServer.Abstractions.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IAttendancesRepository Attendances { get; private set; }    
    public IBranchesRepository Branches { get; private set; }    
    public IClassesRepository Classes { get; private set; }    
    public IClassInShiftsRepository ClassInShifts { get; private set; }    
    public ICommandInFunctionsRepository CommandInFunctions { get; private set; }    
    public ICommandsRepository Commands { get; private set; }    
    public IFunctionsRepository Functions { get; private set; }    
    public INotesRepository Notes { get; private set; }    
    public IPermissionsRepository Permissions { get; private set; }    
    public IPitchesRepository Pitches { get; private set; }    
    public IPitchGroupsRepository PitchGroups { get; private set; }    
    public IShiftsRepository Shifts { get; private set; }    
    public IStudentInClassesRepository StudentInClasses { get; private set; }    
    public IStudentsRepository Students { get; private set; }
    
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Attendances = new AttendancesRepository(_context);    
        Branches = new BranchesRepository(_context);    
        Classes = new ClassesRepository(_context);    
        ClassInShifts = new ClassInShiftsRepository(_context);    
        CommandInFunctions = new CommandInFunctionsRepository(_context);    
        Commands = new CommandsRepository(_context);    
        Functions = new FunctionsRepository(_context);    
        Notes = new NotesRepository(_context);    
        Permissions = new PermissionsRepository(_context);    
        Pitches = new PitchesRepository(_context);    
        PitchGroups = new PitchGroupsRepository(_context);    
        Shifts = new ShiftsRepository(_context);    
        StudentInClasses = new StudentInClassesRepository(_context);    
        Students = new StudentsRepository(_context);
    }

    public void Dispose() => _context.Dispose();

    public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
}