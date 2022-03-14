using System;
using System.Collections.Generic;
using System.Linq;
using TemboContext.Models;
using TemboAI.Models;
using TemboShared.Service;

namespace TemboAI.Core.Subconscious
{
    public class TemboSimple : ISubconscious
    {
        public string Key { get; set; }
        private List<State> SSCollection { get; set; }
        private double[][] Observations { get; set; }
        private double[] Outputs { get; set; }
        private List<Position> TrainingData { get; set; }
        private List<Position> EvaluationData { get; set; }
        private const double PositiveReward = 0.70;
        private const double NegativeReward = -1;
        private const int ReLearnAfter = 1000;
        private int reLearnCountDown = 1000;
        private const double ProbabilityThreshold = 0.6;

        private bool IsTrainningComplete { get; set; }
        public void Init(List<Position> positions)
        {
           
        }

        public void Init()
        {
            TrainingData = CN.Db.Position.All().ToList();
            IsTrainningComplete = false;
            SSCollection = new List<State>();
            Observations = TrainingData.VectorFromPosition();
            Outputs = TrainingData.Outcome();
            Learn();
        }
        public void Init(List<Position> training, List<Position> evaluating)
        {
            TrainingData = training;
            EvaluationData = evaluating;
            IsTrainningComplete = false;
            SSCollection = new List<State>();
            Observations = TrainingData.VectorFromPosition();
            Outputs = TrainingData.Outcome();
            $"Training SIMP {Key} with {training.Count} samples".Log(2);
            Learn();
            $"Finished SIMP {Key}".Log(3);
        }
        public void Evaluate()
        {
            if(EvaluationData==null)return;
            if(EvaluationData.Count<=0)return;
            var correct = 0.0;
            var wrong = 0.0;
            var total = EvaluationData.Count;
            foreach (var position in EvaluationData)
            {
                var ps = Predict(position);
                if (ps.Probability >= ProbabilityThreshold)
                {
                    if (ps.Outcome == position.Outcome)
                    {
                        correct++;
                    }
                    else
                    {
                        wrong++;
                    }
                }
                
            }
            $"SIMP predicted {correct} correctly out of {correct+wrong} a {correct/(correct + wrong):P} win rate".Log(3);
        }
        public void Evaluate(int count)
        {
        }

        public void AddPosition(Position position)
        {
            if (TrainingData == null)
            {
                TrainingData= new List<Position>{position};
                reLearnCountDown--;
            }
            else
            {
                TrainingData.Add(position);
                reLearnCountDown--;
            }

            if (reLearnCountDown <= 0)
            {
                IsTrainningComplete = false;
                SSCollection = new List<State>();
                Observations = TrainingData.VectorFromPosition();
                Outputs = TrainingData.Outcome();
                $"Retraining SIMP {Key} with {TrainingData.Count} samples".Log(4);
                Learn();
                reLearnCountDown = ReLearnAfter;
            }
        }
        public Prediction Predict(Position position)
        {
            if (!IsTrainningComplete)
            {
                return null;
            }
            var ob = new double[1][] { position.PositionToVector() };
            var probability = Probability(ob);
            var pred = new Prediction
            {
                Outcome = probability>=ProbabilityThreshold,
                Probability = (float)probability,
                Score =(float) Rewards(ob),
                PredictedLabel = probability >= ProbabilityThreshold
            };
            return pred;
        }
        private void Learn()
        {
            for (int index = 0; index < Observations.Length; ++index)
            {
                if (!ObservationExists(Observations[index], (int) Outputs[index]))
                {
                    SSCollection.Add(new State
                    {
                        Values = Observations[index],
                        Output = (int)Outputs[index],
                        Occurrence = 1,
                        Reward = Outputs[index]>=1?PositiveReward:NegativeReward
                    });
                }
            }
            IsTrainningComplete = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="observation"></param>
        /// <returns></returns>
        private double Score(double[][] observation)
        {
            List<State> list = SSCollection.Where(s => s.EqualsTo(observation[0])).ToList();
            var totalObservations = SSCollection.Where(s=>s.Output==1).Sum(s => s.Occurrence);
            double winners = list.Where(s => s.Output == 1).Sum(c => c.Occurrence);
            return winners / totalObservations;
        }
        private double Probability(double[][] observation)
        {
            List<State> list = SSCollection.Where(s => s.EqualsTo(observation[0])).ToList();
            double losses= list.Where(s => s.Output == 0).Sum(c => c.Occurrence);
            double winners = list.Where(s => s.Output == 1).Sum(c => c.Occurrence);
            return winners / (losses + winners);
        }

        private double Rewards(double[][] observation)
        {
            return SSCollection.Where(s => s.EqualsTo(observation[0])).Sum(s=>s.Reward);
        }

        private bool ObservationExists(double[] observation, int output)
        {
            foreach (var state in SSCollection)
            {
                if (state.EqualsTo(observation) && state.Output==output)
                {
                    state.Occurrence++;
                    if (output == 1)
                    {
                        state.Reward += PositiveReward;
                    }
                    else
                    {
                        state.Reward += NegativeReward;
                    }

                    return true;
                }

            }
            return false;
        }
    }
}
