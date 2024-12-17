using SubSystemComponent;
using Todo;

namespace ResultObjectComponents
{
    public class ResultObjectHandlerSystem : Subsystem
    {
        public ResultObjectHandlerSystem(params Subsystem[] dependencies) :
            base(ConfigObject.LogFilePath, dependencies)
        {

        }

        public override void Initialize()
        {
        }

        public override async Task TaskObject(CancellationToken cancellationToken)
        {
            var  @params = new Dictionary<string, string>();
            var resultObject = GetSubsystem("ResultObjectResolverSystem")?.GetOutput<ResultObjectTodoSystem>(false, defaultValue: null);

            if(resultObject == null)
            {
                return;
            }

            switch (resultObject)
            {
                case ResultObjectTodoSystem todoSystem:
					@params.Add("todo_system_command", todoSystem.Command.ToString());
					@params.Add("todo_system_id", todoSystem.Id);
					@params.Add("todo_system_new_item_title", todoSystem.Title);
					@params.Add("todo_system_new_item_duetodate", todoSystem.DueDate.ToString());
					@params.Add("todo_system_new_item_reminderdate", todoSystem.ReminderDate.ToString());
					@params.Add("todo_system_new_item_iscompleted", todoSystem.IsCompleted.ToString());
                    await SubsystemPool.RunAsync(SubSystemFactory<TodoSystem>.Create(), cancellationToken);
                    break;
            }

        }
    }
}