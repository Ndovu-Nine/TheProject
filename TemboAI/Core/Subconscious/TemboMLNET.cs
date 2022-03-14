using System;
using Microsoft.ML;
using System.Collections.Generic;
using TemboAI.Models;
using TemboContext.Models;
using TemboShared.Service;
using Microsoft.ML.Trainers.LightGbm;
using Microsoft.ML.Data;

namespace TemboAI.Core.Subconscious
{
    public class TemboMLNET : ISubconscious
    {
        private static readonly MLContext MLContext = new MLContext();
        public static ITransformer Model { get; private set; }
        public static bool IsDone { get; private set; }
        //public static List<Position> Positions;
        public string Key { get; set; }

        public void Init()
        {           
        }

        public void Init(List<Position> trainingSet, List<Position> evaluationSet, int numberOfIterations, int numberOfLeaves)
        {
            var trainingData = MLContext.Data.LoadFromEnumerable(trainingSet.FromPosition());
            var testingData = MLContext.Data.LoadFromEnumerable(evaluationSet.FromPosition());
            if (CanLoad())
            {
                DataViewSchema modelSchema;
                Model=MLContext.Model.Load($"{AppDomain.CurrentDomain.BaseDirectory}{Key}.zip", out modelSchema);
                
                var transformedData = Model.Transform(testingData);
                var metrics = MLContext.BinaryClassification.Evaluate(transformedData, "Outcome");
                PrintMetrics(metrics);
                IsDone = true;
                $"Loaded a saved Model {Key}".Log(3);
            }
            else
            {
                
                var options = new LightGbmBinaryTrainer.Options
                {
                    NumberOfIterations = numberOfIterations,
                    LabelColumnName = "Outcome",
                    NumberOfLeaves = numberOfLeaves
                };


                var rawPipe = MLContext.Transforms.Concatenate("Features", new[]
                        {"DayOfWeek", "Hour","Direction","DurationOfTrade","Fractal","Macd","Rainbow","Rsi", "Stoch","Wpr","TrendA","TrendB","TrendC","TrendD","TrendE","TrendF"})
                    .Append(MLContext.BinaryClassification.Trainers.LightGbm(options));
                $"Training MLNET {Key} with {trainingSet.Count} samples, boosting {options.NumberOfIterations}, leaves {options.NumberOfLeaves}".Log(2);
                Model = rawPipe.Fit(trainingData);
                
                var transformedData = Model.Transform(testingData);
                var metrics = MLContext.BinaryClassification.Evaluate(transformedData, "Outcome");
                PrintMetrics(metrics);
                IsDone = true;
                MLContext.Model.Save(Model, trainingData.Schema, $"{Key}.zip");
                $"Finished MLNET {Key}".Log(3);
            }
            
        }

        public bool CanLoad()
        {
            return System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}{Key}.zip");
        }
        private void PrintMetrics(BinaryClassificationMetrics metrics)
        {
            $"PRINTING METRICS FOR {Key}".Log(1);
            $"Accuracy: {metrics.Accuracy:F2}".Log(5);
            $"AUC: {metrics.AreaUnderRocCurve:F2}".Log(5);
            $"F1 Score: {metrics.F1Score:F2}".Log(5);
            $"Negative Precision: {metrics.NegativePrecision:F2}".Log(5);
            $"Negative Recall: {metrics.NegativeRecall:F2}".Log(5);
            $"Positive Precision: {metrics.PositivePrecision:F2}".Log(5);
            $"Positive Recall: {metrics.PositiveRecall:F2}\n".Log(5);
            metrics.ConfusionMatrix.GetFormattedConfusionTable().Log(5);
            $"END PRINTING METRICS FOR {Key}".Log(1);
        }
        public void Init(List<Position> positions)
        {           
        }
        public void Evaluate(int count)
        {
        }
        public void Evaluate()
        {
        }
        public Prediction Predict(Position position)
        {
            if (!IsDone)
            {
                return null;
            }
            var pos = position.FromPosition();
            var eng = MLContext.Model.CreatePredictionEngine<PositionFeature, Prediction>(Model);
            return eng.Predict(pos);
        }

    }
}
