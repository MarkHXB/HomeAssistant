using HomeAssistant.Lib.Features;
using HomeAssistant.Lib.Features.Features;
using Microsoft.AspNetCore.Mvc;
using SubSystemComponent;
using Todo;

namespace HomeAssistant.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FeatureController : ControllerBase
    {
        private readonly FeatureActions _featureActions;

        public FeatureController()
        {
            _featureActions = new FeatureActions();
        }

        [HttpGet(Name = "Feature")]
        public IActionResult Get(string featureName)
        {
            var feature = _featureActions.Get(featureName);

            if(feature is null)
            {
                return NotFound(featureName);
            }

            return Ok(feature);
        }

        [HttpGet(Name = "Features")]
        public IActionResult GetAll()
        {
            try
            {
                var features = _featureActions.GetAll(); // rossz mert athivas van a subsystemre
                return Ok(features);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }

        [HttpPost(Name = "ExecuteFeatureAction")]
        public async Task<IActionResult> Execute(string featureName, string? actionName, Dictionary<string,string>? parameters)
        {
            try
            {
                ExecuteAction executeAction = (ExecuteAction)Enum.Parse(typeof(ExecuteAction), actionName?.ToUpper() ?? string.Empty);
                await _featureActions.Execute(featureName, executeAction, parameters);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpGet(Name = "TodoSystem")]
        public async Task<IActionResult> Execute(string command, string? itemId, string? newItemTitle,
            string? newItemDueToDate, string? newItemReminderDate, string? newItemIsCompleted)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                return BadRequest("You must specify the command for ToDo subsystem");
            }

            try
            {
                Dictionary<string, string> @params = new()
                {
                    {"todo_system_command", command},
                    {"todo_system_id", itemId ?? string.Empty},
                    {"todo_system_new_item_title", newItemTitle ?? string.Empty},
                    { "todo_system_new_item_duetodate", newItemDueToDate?? string.Empty},
                    {"todo_system_new_item_reminderdate", newItemReminderDate?? string.Empty}, 
                    {"todo_system_new_item_iscompleted", newItemIsCompleted ?? string.Empty},
                };

                var todoSystem = SubSystemFactory<TodoSystem>.Create(@params);

                SubsystemPool.AddSubsystem(todoSystem);

                await SubsystemPool.RunAllAsync(new CancellationToken());

                return Ok(todoSystem.TodoItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("audio")]
        public IActionResult ReceiveAudio(string featureName, string subsystemName)
        {
            try
            {
                // Receive the audio file here
                using (var stream = new MemoryStream())
                {
                    Request.Body.CopyTo(stream);
                    // You can save the audio file to disk or perform any other processing here
                    // Example: Save the audio to a file named "received_audio.wav"
                    //string path = System.IO.File.WriteAllBytes("output.wav", stream.ToArray());
                }

                Console.WriteLine("Audio file received and saved successfully");
                return Ok("Audio file received and saved successfully");
            }
            catch (Exception ex)
            {
                // Log any errors
                Console.WriteLine($"Error receiving audio: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
