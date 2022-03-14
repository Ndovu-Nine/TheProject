using Microsoft.ML.Data;

namespace TemboAI.Models
{
    public class Prediction
    {
        // Original label.
        [ColumnName("Outcome")]
        public bool Outcome { get; set; }
        // Predicted label from the trainer.
        public bool PredictedLabel { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }        
    }
}
