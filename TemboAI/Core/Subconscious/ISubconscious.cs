using System;
using System.Collections.Generic;
using System.Text;
using TemboAI.Models;
using TemboContext.Models;

namespace TemboAI.Core.Subconscious
{
    public interface ISubconscious
    {
        void Init(List<Position> positions);
        void Init();
        void Evaluate();
        void Evaluate(int count);
        Prediction Predict(Position Position);
    }
}
