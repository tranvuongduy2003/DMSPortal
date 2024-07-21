namespace DMSPortal.Models.Requests;

public class UpdatePermissionByCommandRequest
{
    public string FunctionId { get; set; }

    public string CommandId { get; set; }

    public bool Value { get; set; }
}