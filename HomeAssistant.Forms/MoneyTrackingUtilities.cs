using Newtonsoft.Json;

namespace HomeAssistant.Forms
{
    internal class MoneyTrackingUtilities
    {

        private static List<List<TransactionRecordJson>> _history;

        public static List<List<TransactionRecordJson>> GetHistory(bool update = false)
        {
            if(!update && _history != null)
            {
                return _history;
            }

            List<string> transactionFiles = new List<string>();
            string prefix = "Transaction_";

            // Get all files in the current directory
            string[] allFiles = Directory.GetFiles("./");

            // Iterate through each file and check if it starts with the given prefix
            foreach (string file in allFiles)
            {
                // Get the file name without the path
                string fileName = Path.GetFileName(file);
                if (fileName.StartsWith(prefix))
                {
                    transactionFiles.Add(file);
                }
            }

            if(update || _history == null)
            {
                List<List<TransactionRecordJson>> history = new List<List<TransactionRecordJson>>();

                foreach (string file in transactionFiles)
                {
                    string content = File.ReadAllText(file);

                    List<TransactionRecordJson> jsonData = JsonConvert.DeserializeObject<List<TransactionRecordJson>>(content) ?? new List<TransactionRecordJson>();

                    history.Add(jsonData);
                }

                _history = history;
            }

            return _history;
        }
    }
}
